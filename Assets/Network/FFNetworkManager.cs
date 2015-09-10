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
		protected Dictionary<ZeroconfRoom,FFTcpClient> _clients;
		protected ZeroconfRoom _mainRoom;
		protected FFTcpClient _mainClient;
		#endregion
		
		
		#region Main methods
		internal FFNetworkManager()
		{
			_mainRoom = null;
			_mainClient = null;
			_clients = new Dictionary<ZeroconfRoom, FFTcpClient>();
		}
		
		internal void Destroy()
		{
			FFLog.LogError("Destroy Network Manager");
	
			StopBroadcastingGame();
			StopLookingForGames();
			StopServer();
			
			foreach(ZeroconfRoom room in _clients.Keys)
			{
				_clients[room].Close();
			}
			_clients.Clear();
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
		
		internal bool IsConnectedToLan()
		{
			IPAddress ip = NetworkIP;
			return !IPAddress.IsLoopback(ip);
		}
		#endregion
		
		
		#region Server
		internal FFTcpServer Server
		{
			get
			{
				return _server;
			}
		}
		
		internal void StartBroadcastingGame(string a_roomName = "My Zeroconf Room")
		{
			if(_server == null)
			{
				_server = new FFTcpServer(NetworkIP);
				ZeroconfManager.Instance.Host.StartAdvertising(GAME_PROTOCOL, a_roomName, _server.Port);
				ZeroconfManager.Instance.Host.onStartAdvertisingSuccess += OnStartAdvertisingSuccess;
				ZeroconfManager.Instance.Host.onStartAdvertisingFailed += OnStartAdvertisingFailed;
				_server.StartAcceptingConnections();
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
			ZeroconfManager.Instance.Client.StartDiscovery(GAME_PROTOCOL);
			ZeroconfManager.Instance.Client.onStartDiscoverySuccess += OnStartDiscoverySuccess;
			ZeroconfManager.Instance.Client.onStartDiscoveryFailed += OnStartDiscoveryFailed;
		}
		
		internal void StopLookingForGames()
		{
			ZeroconfManager.Instance.Client.StopDiscovery();
			ZeroconfManager.Instance.Client.onStartDiscoverySuccess -= OnStartDiscoverySuccess;
			ZeroconfManager.Instance.Client.onStartDiscoveryFailed -= OnStartDiscoveryFailed;
			ZeroconfManager.Instance.Client.onRoomAdded -= OnRoomAdded;
			ZeroconfManager.Instance.Client.onRoomLost -= OnRoomLost;
		}
		
		internal FFTcpClient MainClient
		{
			get
			{
				return _mainClient;
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
		
		#region Room
		internal void JoinZeroconfRoom(ZeroconfRoom a_room)
		{
			/*if(_clients.TryGetValue(a_room, out _mainClient))
			{
				_mainRoom = a_room;
			}
			else
			{
				LeaveCurrentRoom();
			}*/
		}
		
		internal void LeaveCurrentRoom()
		{
			_mainRoom = null;
			_mainClient = null;
		}
		
		protected FFTcpClient ContactRoom(ZeroconfRoom a_room)
		{
			FFTcpClient newClient = new FFTcpClient(new IPEndPoint(NetworkIP,0),
			                                    	 a_room.EndPoint);
			newClient.Connect();
			newClient.StartWorkers();
			return newClient;
		}
		
		protected void OnRoomAdded(ZeroconfRoom a_room)
		{
			_clients.Add(a_room, ContactRoom(a_room));
			FFLog.Log(EDbgCat.Networking, "New room : " + a_room.roomName.ToString() + " - " + a_room.EndPoint.ToString());
		}
		
		protected void OnRoomLost(ZeroconfRoom a_room)
		{
			_clients[a_room].Close();
			_clients.Remove(a_room);
			FFLog.Log(EDbgCat.Networking, "Removing room : " + a_room.roomName.ToString());
		}
		#endregion
	}
}