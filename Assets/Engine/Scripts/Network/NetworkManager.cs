using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Net.Sockets;
using System.Net;
using Zeroconf;

using FF.Multiplayer;
using FF.Network.Message;

namespace FF.Network
{
    internal class NetworkManager : BaseManager
    {
        private static string GAME_PROTOCOL = "_pong._tcp.";

        /// <summary>
        /// Size 3 : 1.0.0
        /// </summary>
        protected FFVersion _version;
        internal FFVersion NetworkVersion
        {
            get
            {
                if (_version == null)
                    _version = new FFVersion(1, 0, 0);
                return _version;
            }
        }

        #region Server Properties
        protected FFTcpServer _tcpServer;
        internal FFTcpServer TcpServer
        {
            get
            {
                return _tcpServer;
            }
        }

        protected FFGameServer _gameServer;
        internal FFGameServer GameServer
        {
            get
            {
                return _gameServer;
            }
        }
        #endregion

        #region Client Properties
        protected FFClientWrapper _mainClient;
        internal FFClientWrapper MainClient
        {
            get
            {
                return _mainClient;
            }
        }

        /// <summary>
        /// Returns the loopback client ID if server.
        /// Returns the main client ID if client.
        /// </summary>
        internal int NetworkId
        {
            get
            {
                if (IsServer)
                {
                    return _tcpServer.LoopbackClient.NetworkID;
                }
                else if (_mainClient != null)
                {
                    return _mainClient.NetworkID;
                }

                return -1;
            }
        }

        internal FFNetworkPlayer NetPlayer
        {
            get
            {
                return CurrentRoom.PlayerForId(NetworkId);
            }
        }

        protected long _requestId = 0L;
        internal long NextRequestId
        {
            get
            {
                
                return _requestId++;
            }
        }
        #endregion

        #region Room Manager
        protected Room _currentRoom;
        internal Room CurrentRoom
        {
            get
            {
                return _currentRoom;
            }
        }

        protected ClientRoomManager _clientRoomManager;
        internal ClientRoomManager ClientRoomManager
        {
            get
            {
                return _clientRoomManager;
            }
        }

        protected ServerRoomManager _serverRoomManager;
        #endregion

        #region Manager
        internal NetworkManager()
		{
			_mainClient = null;
            _clientRoomManager = new ClientRoomManager(this);
            _serverRoomManager = new ServerRoomManager();
        }

        internal override void TearDown()
        {
            _clientRoomManager.TearDown();
            _serverRoomManager.TearDown();

            if(IsServer)
			    StopServer();
			
			if(_mainClient != null)
				_mainClient.Close();
		}
	
		internal override void DoFixedUpdate()
		{
            if (_tcpServer != null)
				_tcpServer.DoUpdate();

            _clientRoomManager.DoUpdate();
            _serverRoomManager.DoUpdate();

            if (_mainClient != null)
                _mainClient.DoUpdate();
		}
        #endregion

        #region IP
        protected int _preferedPort = 0;
        internal int PreferedPort
        {
            set
            {
                _preferedPort = value;
            }
            get
            {
                return _preferedPort;
            }
        }

        protected IPAddress _preferedNetworkAddress = null;
        internal IPAddress PreferedNetworkAddress
        {
            set
            {
                if (NetworkAddresses.Contains(value))
                {
                    _preferedNetworkAddress = value;
                }
                else
                {
                    FFLog.LogWarning(EDbgCat.Networking, "Couldn't set prefered network address : " + value.ToString());
                }
            }
            get
            {
                if (_preferedNetworkAddress == null)
                {
                    List<IPAddress> addresses = NetworkAddresses;
                    if (addresses.Count > 0)
                        _preferedNetworkAddress = addresses[0];
                    else
                        _preferedNetworkAddress = IPAddress.Loopback;
                }

                return _preferedNetworkAddress;
            }
        }

		internal List<IPAddress> NetworkAddresses
		{
			get
			{
				IPAddress[] ipAddresses = Dns.GetHostAddresses (Dns.GetHostName());
                List<IPAddress> addresses = new List<IPAddress>();
				foreach(IPAddress each in ipAddresses)
				{
					if(each.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(each))
					{
                        addresses.Add(each);
                    }
				}
				return addresses;
			}
		}
		#endregion
		
		
		#region Server
		internal bool IsServer
		{
			get
			{
				return _tcpServer != null && _gameServer != null;
			}
		}

        internal void StartServer(string a_roomName = "My Zeroconf Room")
        {
            if (_tcpServer == null)
            {
                _tcpServer = new FFTcpServer(PreferedNetworkAddress, PreferedPort);
                _gameServer = new FFGameServer(_tcpServer);

                _currentRoom = _serverRoomManager.PrepareRoom(a_roomName);

                FFNetworkPlayer netPlayer = new FFNetworkPlayer(_tcpServer.LoopbackClient.NetworkID,
                                                                Engine.Game.Player);
                netPlayer.isHost = true;
                _currentRoom.SetPlayer(new SlotRef(0, 0),
                                        netPlayer);


                _serverRoomManager.RegisterReceivers();
            }
            else
            {
                FFLog.LogWarning(EDbgCat.Networking, "You're trying to start another server.");
            }
        }

        internal void StopServer()
		{
			if(_tcpServer != null)
			{
				_tcpServer.Close();
				_tcpServer = null;
			}
            if (_gameServer != null)
            {
                _gameServer.Close();
                _gameServer = null;
            }
            _serverRoomManager.UnregisterReceivers();

            if (_currentRoom != null)
            {
                _currentRoom.TearDown();
                _currentRoom = null;
            }
        }

        internal void StartBroadcastingGame(string a_gameName)
        {
            if (IsServer)
            {
                _serverRoomManager.StartBroadcastingGame(GAME_PROTOCOL, a_gameName, _tcpServer.Port, OnBroadcastingStartSuccess, null);
            }
            else
            {
                FFLog.LogWarning(EDbgCat.Networking, "Can't start broadcasting game : server isn't start. Call StartServer first.");
            }
        }

        internal void StopBroadcastingGame()
        {
            if (IsServer)
            {
                _serverRoomManager.StopBroadcastingGame();
            }
            else
            {
                FFLog.LogWarning(EDbgCat.Networking, "Can't stop broadcasting game : server isn't start. Call StartServer first.");
            }
        }

        protected void OnBroadcastingStartSuccess()
        {
            if(IsServer)
                _tcpServer.ResumeAcceptingConnections();
        }
        #endregion

        #region Client
        internal void StartLookingForGames()
        {
            _clientRoomManager.StartLookingForGames(GAME_PROTOCOL);
        }

        internal void StopLookingForGames()
        {
            _clientRoomManager.StopLookingForGames();
        }

        /// <summary>
        /// Extract room & client matching given endpoint and set the CurrentRoom & MainClient value.
        /// </summary>
        internal void JoinRoom(IPEndPoint a_roomEndpoint)
        {
            if (_currentRoom == null)
            {
                _clientRoomManager.JoinRoom(a_roomEndpoint,
                                            ref _currentRoom,
                                            ref _mainClient);
            }
            else
            {
                FFLog.LogError(EDbgCat.Networking, "Can't join two room at the same time.");
            }
        }

        /// <summary>
        /// Kill current room & close main client
        /// </summary>
        internal void LeaveCurrentRoom(bool a_needsEvent = false)
        {
            if (_currentRoom != null)
            {
                _currentRoom.TearDown();
                _currentRoom = null;

                _clientRoomManager.LeaveCurrentRoom();
            }
            else
            {
                FFLog.LogError(EDbgCat.Networking, "Can't leave current room -> Not set.");
            }

            if (_mainClient != null)
            {            
                if(!a_needsEvent)
                    _mainClient.onConnectionEnded = null;
                _mainClient.Close();
                _mainClient = null;
            }
            else
            {
                FFLog.LogError(EDbgCat.Networking, "Can't close main client -> Not set.");
            }

        }

        #region Direct Connect
        internal FFClientWrapper DirectConnect(IPEndPoint a_endpoint)
        {
            _mainClient = new FFClientWrapper(new IPEndPoint(PreferedNetworkAddress, 0),
                                                            a_endpoint);
            _mainClient.Connect();
            return _mainClient;
        }

        internal void DirectConnectSetRoom(Room a_room)
        {
            _currentRoom = a_room;
            _clientRoomManager.RegisterRoomReceivers();
        }
        #endregion
        #endregion
    }
}