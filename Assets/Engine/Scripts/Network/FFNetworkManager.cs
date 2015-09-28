using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;

using Zeroconf;

namespace FF.Networking
{
	internal class FFNetworkManager
	{
		private static string GAME_PROTOCOL = "_pong._tcp.";
		
		#region Properties
		protected FFTcpServer _server;
		internal FFTcpServer Server
		{
			get
			{
				return _server;
			}
		}
		
		protected Dictionary<IPEndPoint, FFTcpClient> _clients;
		protected Dictionary<IPEndPoint, FFRoom> _rooms;
		
		protected FFTcpClient _mainClient;
		internal FFTcpClient MainClient
		{
			get
			{
				return _mainClient;
			}
		}
		
		protected FFNetworkPlayer _player;
		internal FFNetworkPlayer Player
		{
			get
			{
				return _player;
			}
		}
		
		protected FFRoom _currentRoom;
		internal FFRoom CurrentRoom
		{
			get
			{
				return _currentRoom;
			}
		}
		#endregion
		
		
		#region Main methods
		internal FFNetworkManager()
		{
			_mainClient = null;
			_clients = new Dictionary<IPEndPoint, FFTcpClient>();
			_rooms = new Dictionary<IPEndPoint, FFRoom>();
		}
		
		internal void Destroy()
		{
			FFLog.LogError("Destroy Network Manager");
	
			StopBroadcastingGame();
			StopLookingForGames();
			StopServer();
			
			foreach(IPEndPoint endpoint in _clients.Keys)
			{
				_clients[endpoint].Close();
			}
			_clients.Clear();
			
			_rooms.Clear();
			
			if(_mainClient != null)
				_mainClient.Close();
		}
	
		internal void DoUpdate()
		{
			foreach(FFTcpClient each in _clients.Values)
			{
				if(each != null)
					each.DoUpdate();
			}
			if(_server != null)
				_server.DoUpdate();
				
			if(_mainClient != null)
			{
				_mainClient.DoUpdate();
			}
		}
		#endregion
		
		#region IP
		internal IPAddress NetworkIP
		{
			get
			{
				IPAddress[] ipAddresses = Dns.GetHostAddresses (Dns.GetHostName());
				foreach(IPAddress each in ipAddresses)
				{
					if(each.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(each))
					{
						return each;
					}
				}
				
				return IPAddress.Loopback;
			}
		}
		#endregion
		
		
		#region Server
		internal bool IsServer
		{
			get
			{
				return _server != null;
			}
		}
		
		internal void StartBroadcastingGame(string a_roomName = "My Zeroconf Room")
		{
			if(_server == null)
			{
				_server = new FFTcpServer(NetworkIP);
				
				_player = new FFNetworkPlayer(_server.LocalEndpoint, FFEngine.Game.player);
				_player.isHost = true;
				_player.useTV = FFEngine.MultiScreen.UseTV;
				
				_currentRoom = PrepareRoom(a_roomName);
				ZeroconfManager.Instance.Host.onStartAdvertisingSuccess += OnStartAdvertisingSuccess;
				ZeroconfManager.Instance.Host.onStartAdvertisingFailed += OnStartAdvertisingFailed;
				ZeroconfManager.Instance.Host.StartAdvertising(GAME_PROTOCOL, a_roomName, _server.Port);
			}
			else
			{
				FFLog.LogWarning(EDbgCat.Networking, "You're trying to start another server.");
			}
			
		}
		
		internal void StopBroadcastingGame()
		{
			ZeroconfManager.Instance.Host.onStartAdvertisingSuccess -= OnStartAdvertisingSuccess;
			ZeroconfManager.Instance.Host.onStartAdvertisingFailed -= OnStartAdvertisingFailed;
			if(_server != null)
			{
				ZeroconfManager.Instance.Host.StopAdvertising();
			}
			else
			{
				FFLog.LogWarning(EDbgCat.Networking, "Server wasn't running.");
			}
		}
		
		internal void StopServer()
		{
			if(_server != null)
			{
				_server.Close();
				_server = null;
			}
		}
		#endregion
		
		#region Zeroconf Host
		protected void OnStartAdvertisingSuccess()
		{
			ZeroconfManager.Instance.Host.onStartAdvertisingSuccess -= OnStartAdvertisingSuccess;
			ZeroconfManager.Instance.Host.onStartAdvertisingFailed -= OnStartAdvertisingFailed;
			_server.StartAcceptingConnections();
			FFLog.Log(EDbgCat.Networking, "Start advertising success. Awaiting connections.");
		}
		
		protected void OnStartAdvertisingFailed()
		{
			ZeroconfManager.Instance.Host.onStartAdvertisingSuccess -= OnStartAdvertisingSuccess;
			ZeroconfManager.Instance.Host.onStartAdvertisingFailed -= OnStartAdvertisingFailed;
			StopServer();
			FFLog.LogWarning(EDbgCat.Networking, "Start advertising failed. Closing server.");
		}
		#endregion
		
		
		#region Clients
		internal void StartLookingForGames()
		{
			ZeroconfManager.Instance.Client.onStartDiscoverySuccess += OnStartDiscoverySuccess;
			ZeroconfManager.Instance.Client.onStartDiscoveryFailed += OnStartDiscoveryFailed;
			ZeroconfManager.Instance.Client.StartDiscovery(GAME_PROTOCOL);
		}
		
		internal void StopLookingForGames()
		{
			ZeroconfManager.Instance.Client.StopDiscovery();
			ZeroconfManager.Instance.Client.onStartDiscoverySuccess -= OnStartDiscoverySuccess;
			ZeroconfManager.Instance.Client.onStartDiscoveryFailed -= OnStartDiscoveryFailed;
			ZeroconfManager.Instance.Client.onRoomAdded -= OnRoomAdded;
			ZeroconfManager.Instance.Client.onRoomLost -= OnRoomLost;
			foreach(FFTcpClient client in _clients.Values)
			{
				if(client != _mainClient)
					client.Close();
			}
			_clients.Clear();
			_rooms.Clear();
		}
		
		internal void TryCloseClient(IPEndPoint a_endpoint)
		{
			FFTcpClient client = null;
			if(_clients.TryGetValue(a_endpoint, out client))
			{
				client.Close();
				_clients.Remove(client.Remote);
				_rooms.Remove(client.Remote);
			}
			
			if(_mainClient != null && _mainClient.Remote == a_endpoint)
			{
				LeaveCurrentRoom();
				_clients.Remove(client.Remote);
				_rooms.Remove(client.Remote);
			}
		}
		#endregion
		
		#region Zeroconf Client
		protected void OnStartDiscoverySuccess()
		{
			ZeroconfManager.Instance.Client.onStartDiscoverySuccess -= OnStartDiscoverySuccess;
			ZeroconfManager.Instance.Client.onStartDiscoveryFailed -= OnStartDiscoveryFailed;
			FFLog.Log(EDbgCat.Networking, "Start discovery success. Looking for rooms.");
			
			ZeroconfManager.Instance.Client.onRoomAdded += OnRoomAdded;
			ZeroconfManager.Instance.Client.onRoomLost += OnRoomLost;
		}
		
		protected void OnStartDiscoveryFailed()
		{
			ZeroconfManager.Instance.Client.onStartDiscoverySuccess -= OnStartDiscoverySuccess;
			ZeroconfManager.Instance.Client.onStartDiscoveryFailed -= OnStartDiscoveryFailed;
			FFLog.LogWarning(EDbgCat.Networking, "Start discovery failed.");
		}
		#endregion
		
		#region Room Discovery
		protected FFTcpClient ContactRoom(ZeroconfRoom a_room)
		{
			FFTcpClient newClient = new FFTcpClient(new IPEndPoint(NetworkIP,0),
			                                    	 a_room.EndPoint);
			newClient.Connect();
			return newClient;
		}
		
		protected void OnRoomAdded(ZeroconfRoom a_room)
		{
			_clients.Add(a_room.EndPoint, ContactRoom(a_room));
			FFLog.Log(EDbgCat.Networking, "New room : " + a_room.roomName.ToString() + " - " + a_room.EndPoint.ToString());
		}
		
		internal FFRoomCallback onRoomLost;
		protected void OnRoomLost(ZeroconfRoom a_room)
		{
            FFLog.Log(EDbgCat.Networking, "Trying to remove room : " + a_room.roomName.ToString());
            if (a_room.EndPoint != null)
            {
                FFTcpClient client = null;
                if (_clients.TryGetValue(a_room.EndPoint, out client))
                {
                    _clients[a_room.EndPoint].Close();
                    _clients.Remove(a_room.EndPoint);
                }

                FFRoom room = null;
                if (_rooms.TryGetValue(a_room.EndPoint, out room))
                {
                    if (onRoomLost != null)
                        onRoomLost(room);
                    _rooms.Remove(a_room.EndPoint);
                }
            }
		}
		
		protected FFRoom PrepareRoom(string a_roomName)
		{
			_currentRoom = new FFRoom();
			_currentRoom.roomName = a_roomName;
			_currentRoom.AddTeam(new FFTeam("Left Side", 2));
			_currentRoom.AddTeam(new FFTeam("Right Side", 2));
			_currentRoom.SetPlayer(0,0,FFEngine.Network.Player);
			return _currentRoom;
		}
		#endregion
		
		#region Rooms Join Update & Quit
		internal FFJoinRoomRequest JoinGame(FFRoom a_room)
		{
			FFLog.LogError("Trying to join room : " + a_room.roomName);
			if(_clients.TryGetValue(a_room.serverEndPoint, out _mainClient))
			{
				_currentRoom = a_room;
				FFNetworkPlayer player = new FFNetworkPlayer(_mainClient.Local, FFEngine.Game.player);
				FFJoinRoomRequest joinRequest = new FFJoinRoomRequest(player);
				_mainClient.QueueMessage(joinRequest);
				return joinRequest;
			}
			else
			{
                FFLog.LogError(EDbgCat.Networking, "Trying to connect main client while already used.");
			}
			
			return null;
		}
		
		internal void LeaveCurrentRoom()
		{
			_mainClient = null;
			_currentRoom = null;
		}
		
		internal FFRoomCallback onRoomInfoReceived;
		
		internal void OnRoomInfosReceived(FFRoom a_room)
		{
			//CURRENT ROOM UPDATED
			if(_mainClient != null && a_room.serverEndPoint == _mainClient.Remote)
			{
				FFLog.Log(EDbgCat.Networking, "Updating main room infos");
				_currentRoom.UpdateWithRoom(a_room);
			}
			else if(_rooms.ContainsKey(a_room.serverEndPoint))//A room in the list was updated
			{
				FFLog.Log(EDbgCat.Networking, "Updating room infos");
				_rooms[a_room.serverEndPoint].UpdateWithRoom(a_room);
			}
			else//A new room can be display in the list
			{
				FFLog.Log(EDbgCat.Networking, "New room infos received");
				_rooms.Add(a_room.serverEndPoint,a_room);
				if(onRoomInfoReceived != null)
					onRoomInfoReceived(a_room);
			}
		}
		#endregion
	}
}