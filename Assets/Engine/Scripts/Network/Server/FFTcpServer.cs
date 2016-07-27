using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Net.Sockets;
using System.Net;
using System.Threading;

using FF.Network.Message;
using System;

namespace FF.Network
{
    internal delegate void ServerWrapperCallback(FFServerWrapper a_ffclient);
    internal delegate void IPEndPointCallback(IPEndPoint a_ep);

    internal class FFTcpServer
	{
        protected static int CURRENT_ID = 0;
        internal static int NEXT_ID
        {
            get
            {
                int val = CURRENT_ID;
                CURRENT_ID++;
                return val;
            }
        }

        static internal int MOCK_ID = -127;
        #region Properties
        #region Server
        protected IPEndPoint _endPoint;
		protected TcpListener _tcpListener;
		protected Thread _listeningThread;
        
        protected bool _isListening = false;

        internal IPEndPoint LocalEndpoint
        {
            get
            {
                return _endPoint;
            }
        }

        internal int Port
        {
            get
            {
                return _endPoint.Port;
            }
        }
        #endregion

        #region Clients
        protected Dictionary<int, FFNetworkClient> _clients;
        internal Dictionary<int, FFNetworkClient> Clients
        {
            get
            {
                return _clients;
            }
        }
        
        internal FFNetworkClient ClientForID(int a_id)
        {
            FFNetworkClient client = null;
            _clients.TryGetValue(a_id, out client);
            return client;
        }
        #endregion

        #region Loopback
        protected FFLoopbackClient _loopbackClient;
        internal FFLoopbackClient LoopbackClient
        {
            get
            {
                return _loopbackClient;
            }
        }
        #endregion
        #endregion

        internal FFTcpServer(IPAddress a_ipv4, int a_port)
		{
			try
			{
				_isListening = false;
				
				_clients = new Dictionary<int, FFNetworkClient>();

                _newClients = new List<FFNetworkClient>();
				_removedClients = new List<FFNetworkClient>();
                _reconnectClients = new Dictionary<int, FFNetworkClient>();

                _tcpListener = new TcpListener(a_ipv4, a_port);
				_tcpListener.Start();
				_endPoint = (IPEndPoint)_tcpListener.Server.LocalEndPoint;

                _loopbackClient = new FFLoopbackClient(NEXT_ID,
                                                        _endPoint,
                                                        new IPEndPoint(IPAddress.Loopback, 12345));
                _loopbackClient.GenereateMirror();
                AddNewClient(_loopbackClient);

                StartListening();

                FFLog.Log(EDbgCat.ServerTcp, "Server started on address : " + _endPoint.Address + " & port : " + _endPoint.Port);
			}
			catch(SocketException e)
			{
				FFLog.LogError(EDbgCat.ServerTcp, "Couldn't create server TCPListener." + e.Message);
			}
		}
		
		internal void Close()
		{
		    StopListening();
			
			try
			{
                if(_tcpListener != null)
				    _tcpListener.Stop();
			}
			catch(SocketException e)
			{
				FFLog.LogError(EDbgCat.ServerListening, "Couldn't Stop server." + e.Message);
			}
			
			foreach(FFNetworkClient client in _clients.Values)
			{
				if(client != null)
					client.Close();
			}
			_clients.Clear();
        }

        #region Client Acceptation
        protected bool _shouldRegenerateListener = false;
        internal void PauseAcceptingConnections()
        {
            if (_tcpListener != null)
            {
                _tcpListener.Stop();
                _tcpListener = null;
                FFLog.Log(EDbgCat.ServerListening, "Pause Accepting connections");
            }

            _shouldRegenerateListener = false;
        }

        internal void ResumeAcceptingConnections()
        {
            if (_tcpListener == null)
            {
                _shouldRegenerateListener = true;
                FFLog.Log(EDbgCat.ServerListening, "Resume Accepting connections");
            }
        }
#endregion

#region Listening Work
        protected void StartListening()
        {
            if (!_isListening)
            {
                _isListening = true;
                _listeningThread = new Thread(new ThreadStart(ListeningTask));
                _listeningThread.IsBackground = true;
                _listeningThread.Start();
            }
            else
            {
                FFLog.LogError(EDbgCat.ServerListening, "Server is already listening");
            }
        }

        /// <summary>
        /// Called when the server shutdown
        /// </summary>
        protected void StopListening()
        {
            if (_isListening)
            {
                _isListening = false;
                FFLog.Log(EDbgCat.ServerListening, "Stop thread");
            }
            else
            {
                FFLog.LogWarning(EDbgCat.ServerListening, "Server isn't listening");
            }
        }

        protected void ListeningTask()
		{
			while(_isListening)
			{
                //Try regenerate tcp listener
                if (_shouldRegenerateListener && _tcpListener == null)
                {
                    try
                    {
                        _tcpListener = new TcpListener(_endPoint.Address, _endPoint.Port);
                        _tcpListener.Start();

                        _shouldRegenerateListener = false;
                    }
                    catch (Exception e)
                    {

                    }
                }

                //Listen to pending connection
                if(_tcpListener != null)
                {
                    HandlePendingConnections();
                }
				Thread.Sleep(5);
			}
			
			FFLog.Log(EDbgCat.ServerListening, "Stoping Listener Thread");
		}
		
		protected void HandlePendingConnections()
		{
            while (_isListening && _tcpListener.Pending())
			{
				FFLog.Log(EDbgCat.ServerListening, "Pending connection.");
				TcpClient newClient = _tcpListener.AcceptTcpClient();
				ConnectClient(newClient);
			}
		}

        /// <summary>
        /// Called from listening task
        /// </summary>
        protected void ConnectClient(TcpClient a_client)
        {
            FFServerWrapper ffClient = new FFServerWrapper(a_client, NEXT_ID);
            FFLog.Log(EDbgCat.ServerTcp, "ConnectingClient : " + ffClient.NetworkID.ToString());
            AddNewClient(ffClient);
        }
#endregion

#region Communication
#region Message Management
        internal void DoUpdate()
		{
            lock(_newClients)
            {
                foreach (FFNetworkClient each in _newClients)
                {
                    AddClientOnMt(each);
                }
                _newClients.Clear();
            }

            lock (_removedClients)
            {
                foreach (FFNetworkClient each in _removedClients)
                {
                    RemoveClientOnMt(each);
                }
                _removedClients.Clear();
            }

            lock (_reconnectClients)
            {
                foreach (KeyValuePair<int, FFNetworkClient> pair in _reconnectClients)
                {
                    ReconnectClientOnMt(pair.Value, pair.Key);
                }
                _reconnectClients.Clear();
            }

            foreach (FFNetworkClient each in _clients.Values)
            {
                if (each != null)
                    each.DoUpdate();
            }
		}
#endregion
#endregion

#region Client Management
#region Client Callback
        protected void RegisterClientCallback(FFNetworkClient a_client)
        {
            a_client.onIdCheckCompleted += OnClientReady;

            a_client.onConnectionEnded += OnClientDisconnection;
        }

        protected void UnregisterClientCallback(FFNetworkClient a_client)
        {
            a_client.onIdCheckCompleted -= OnClientReady;

            a_client.onConnectionEnded -= OnClientDisconnection;
        }
#endregion

#region Adding Client
        protected List<FFNetworkClient> _newClients;
        protected void AddNewClient(FFNetworkClient a_ffclient)
        {
            _newClients.Add(a_ffclient);
            _removedClients.Remove(a_ffclient);
            
            FFLog.Log(EDbgCat.ServerTcp, "AddNewClient : " + a_ffclient.NetworkID.ToString());
        }

        private void AddClientOnMt(FFNetworkClient a_client)
        {
            RegisterClientCallback(a_client);
            _clients.Add(a_client.NetworkID, a_client);

            FFLog.Log(EDbgCat.ServerTcp, "Add client on mt : " + a_client.NetworkID.ToString());
        }
#endregion

#region Id
        internal FFIdCheckClientCallback onClientReady = null;
        protected void OnClientReady(FFNetworkClient a_client, int a_serverId, int a_playerId)
        {
            FFLog.Log(EDbgCat.ServerTcp, "OnNewClientReady : " + a_client.ToString());

            if (onClientReady != null)
                onClientReady(_clients[a_client.NetworkID], a_serverId, a_playerId);
        }
#endregion

#region Reconnect
        protected Dictionary<int, FFNetworkClient> _reconnectClients;
        internal void ReconnectClient(FFNetworkClient a_client, int a_serverId, int a_playerId)
        {
            a_client.NetworkID = a_playerId;
            _reconnectClients.Add(a_serverId, a_client);
        }

        protected void ReconnectClientOnMt(FFNetworkClient a_client, int a_serverId)
        {
            _clients.Remove(a_serverId);
            _clients.Add(a_client.NetworkID, a_client);
        }
#endregion

#region Disconnecting Client
        protected List<FFNetworkClient> _removedClients;
		internal FFClientCallback onClientRemoved = null;
		protected void RemoveClient(int a_id)
		{
            FFNetworkClient client = null;
            if(_clients.TryGetValue(a_id, out client))
            {
                _newClients.Remove(client);
                _removedClients.Add(client);

                FFLog.Log(EDbgCat.ServerTcp, "RemoveClient : " + a_id.ToString());
            }
		}

        protected void OnClientDisconnection(FFNetworkClient a_client)
        {
            RemoveClient(a_client.NetworkID);

            FFLog.Log(EDbgCat.ServerTcp, "OnClientDisconnection : " + a_client.NetworkID);
        }

        private void RemoveClientOnMt(FFNetworkClient a_client)
        {
            _clients.Remove(a_client.NetworkID);
            UnregisterClientCallback(a_client);

            if (onClientRemoved != null)
                onClientRemoved(a_client);
        }
#endregion
#endregion
    }
}