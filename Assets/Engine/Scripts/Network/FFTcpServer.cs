﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace FF.Networking
{
	internal delegate void TcpClientCallback(FFTcpClient a_ffclient);
    internal delegate void IPEndPointCallback(IPEndPoint a_ep);

    internal class FFTcpServer
	{
		#region Properties
		protected IPEndPoint _endPoint;
		protected TcpListener _tcpListener;
		protected Thread _listeningThread;
		protected Dictionary<IPEndPoint,FFTcpClient> _clients;
        protected Dictionary<int, IPEndPoint> _idMapping;
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
		
		internal FFTcpServer(IPAddress a_ipv4)
		{
			try
			{
				_isListening = false;
				
				_clients = new Dictionary<IPEndPoint, FFTcpClient>();
                _idMapping = new Dictionary<int, IPEndPoint>();

                _newClients = new List<FFTcpClient>();
				_reconnectedClients = new List<FFTcpClient>();
				_removedClients = new List<FFTcpClient>();
				_lostClients = new List<FFTcpClient>();
                _replacedClients = new List<IPEndPoint>();
				
				_tcpListener = new TcpListener(a_ipv4, 0);
				_tcpListener.Start();
				_endPoint = (IPEndPoint)_tcpListener.Server.LocalEndPoint;

                FFMessageNetworkID.onNetworkIdReceived += OnIdReceived;
				
				FFLog.Log(EDbgCat.Networking, "Server started on address : " + _endPoint.Address + " & port : " + _endPoint.Port);
			}
			catch(SocketException e)
			{
				FFLog.LogError(EDbgCat.Networking, "Couldn't create server TCPListener." + e.Message);
			}
		}
		
		internal void Close()
		{
			if(IsAcceptingConnections)
			{
				StopAcceptingConnections();
			}
			
			try
			{
				_tcpListener.Stop();
			}
			catch(SocketException e)
			{
				FFLog.LogError(EDbgCat.Networking, "Couldn't Stop server." + e.Message);
			}
			
			foreach(FFTcpClient client in _clients.Values)
			{
				if(client != null)
					client.Close();
			}
			_clients.Clear();

            FFMessageNetworkID.onNetworkIdReceived -= OnIdReceived;
        }
		
		#region Client Acceptation
		internal void StartAcceptingConnections()
		{
			if(!_isListening)
			{
				FFLog.Log(EDbgCat.Networking,"Server start listening");
				_isListening = true;
				_listeningThread = new Thread(new ThreadStart(ListeningTask));
				_listeningThread.IsBackground = true;
				_listeningThread.Start();
			}
			else
			{
				FFLog.LogError(EDbgCat.Networking,"Server is already listening");
			}
		}
		
		internal void StopAcceptingConnections()
		{
			if(_isListening)
			{
				_isListening = false;
				FFLog.Log(EDbgCat.Networking,"Stop thread");
			}
			else
			{
				FFLog.LogError(EDbgCat.Networking,"Server isn't listening");
			}
		}
		
		internal bool IsAcceptingConnections
		{
			get
			{
				return _isListening;
			}
		}
		#endregion
		
		#region Listening Work
		internal void ListeningTask()
		{
			while(_tcpListener != null && _isListening)
			{
				HandlePendingConnections();
				Thread.Sleep(50);
			}
			
			FFLog.LogError(EDbgCat.Networking, "Stoping Listener Thread");
		}
		
		protected void HandlePendingConnections()
		{
			if(_isListening && _tcpListener.Pending())
			{
				FFLog.Log(EDbgCat.Networking, "Pending connection.");
				TcpClient newClient = _tcpListener.AcceptTcpClient();
				ConnectClient(newClient);
			}
		}

        protected void ConnectClient(TcpClient a_client)
        {
            FFTcpClient ffClient = new FFTcpClient(a_client);
            ffClient.StartWorkers();
            IPEndPoint newEp = a_client.Client.RemoteEndPoint as IPEndPoint;

            if (_clients.ContainsKey(newEp))
            {
                ReconnectClient(ffClient);
            }
            else
            {
                AddNewClient(ffClient);
            }
        }
        #endregion

        #region Message Management
        internal void DoUpdate()
		{
			while(_newClients.Count > 0)
            {
                FFTcpClient client = _newClients[0];
                _newClients.RemoveAt(0);
                AddClientOnMt(client);
            }

            while (_removedClients.Count > 0)
            {
                FFTcpClient client = _removedClients[0];
                _removedClients.RemoveAt(0);
                DisconnectClientOnMt(client);
            }

            while (_lostClients.Count > 0)
            {
                FFTcpClient client = _lostClients[0];
                _lostClients.RemoveAt(0);
                ClientConnectionLostOnMt(client);
            }

            while (_reconnectedClients.Count > 0)
            {
                FFTcpClient client = _reconnectedClients[0];
                _reconnectedClients.RemoveAt(0);
                ReconnectClientOnMt(client);
            }

            while (_replacedClients.Count > 0)
            {
                _clients.Remove(_replacedClients[0]);
                _replacedClients.RemoveAt(0);
            }

            foreach (FFTcpClient each in _clients.Values)
            {
                if (each != null)
                    each.DoUpdate();
            }
		}

       

        internal void RegisterClientCallback(FFTcpClient a_client)
        {
            a_client.onConnectionEnded += OnClientDisconnection;
        }

        internal void UnregisterClientCallback(FFTcpClient a_client)
        {
            a_client.onConnectionEnded -= OnClientDisconnection;
        }
        #endregion

        #region Sending Message
        internal void BroadcastMessage(FFMessage a_message)
		{
			foreach(IPEndPoint endpoint in _clients.Keys)
			{
                SendMessageToClient(endpoint, a_message);
			}
		}
		
		internal void SendMessageToClient(IPEndPoint a_endpoint, FFMessage a_message)
		{
            FFTcpClient target = null;
            if(_clients.TryGetValue(a_endpoint, out target))
            {
                if(target != null)
                    target.QueueMessage(a_message);
            }
		}
		#endregion

        #region Lost Connection Clients
        protected List<FFTcpClient> _lostClients;
		internal TcpClientCallback onClientLost = null;
		protected void OnClientConnectionLost(FFTcpClient a_ffclient)
		{
			a_ffclient.onConnectionLost -= OnClientConnectionLost;
			
			_newClients.Remove(a_ffclient);
			_removedClients.Remove(a_ffclient);
			_reconnectedClients.Remove(a_ffclient);
            _lostClients.Add(a_ffclient);
			
			FFLog.LogError(EDbgCat.Networking, "Connection lost with Client : " + a_ffclient.Remote.ToString());
		}

        private void ClientConnectionLostOnMt(FFTcpClient a_client)
        {
            _clients[a_client.Remote] = null;
            UnregisterClientCallback(a_client);
            a_client.Close();
            if (onClientLost != null)
                onClientLost(a_client);
        }
        #endregion

        #region Disconnecting Client
        protected List<FFTcpClient> _removedClients;
		internal TcpClientCallback onClientRemoved = null;
		internal void DisconnectClient(IPEndPoint a_endPoint)
		{
			FFTcpClient client = _clients[a_endPoint];
			
			_newClients.Remove(client);
			_removedClients.Add(client);
			_reconnectedClients.Remove(client);
            _lostClients.Remove(client);
			
			FFLog.LogError(EDbgCat.Networking, "Disconnected Client : " + a_endPoint.ToString());
		}
        protected void OnClientDisconnection(FFTcpClient a_client, string a_reason)
        {
            DisconnectClient(a_client.Remote);
        }

        private void DisconnectClientOnMt(FFTcpClient a_client)
        {
            _clients.Remove(a_client.Remote);
            UnregisterClientCallback(a_client);
            a_client.Close();
            if (onClientRemoved != null)
                onClientRemoved(a_client);
        }
        #endregion

        #region Adding Client
        protected List<FFTcpClient> _newClients;
		internal TcpClientCallback onClientAdded = null;
		protected void AddNewClient(FFTcpClient a_ffclient)
		{
			a_ffclient.onConnectionLost += OnClientConnectionLost;
			
			_newClients.Add(a_ffclient);
			_removedClients.Remove(a_ffclient);
			_reconnectedClients.Remove(a_ffclient);
            _lostClients.Remove(a_ffclient);

            FFLog.LogError(EDbgCat.Networking, "Added Client : " + a_ffclient.Remote.ToString());
		}

        private void AddClientOnMt(FFTcpClient a_client)
        {
            _clients.Add(a_client.Remote, a_client);
            RegisterClientCallback(a_client);
            if (onClientAdded != null)
                onClientAdded(a_client);
        }
        #endregion


        #region Reconnection Client
        protected List<FFTcpClient> _reconnectedClients;
		internal TcpClientCallback onClientReconnection = null;
		protected void ReconnectClient(FFTcpClient a_ffclient)
		{
			a_ffclient.onConnectionLost += OnClientConnectionLost;
			
			_newClients.Remove(a_ffclient);
			_removedClients.Remove(a_ffclient);
			_reconnectedClients.Add(a_ffclient);
            _lostClients.Remove(a_ffclient);

            FFLog.LogError(EDbgCat.Networking, "Reconnected Client : " + a_ffclient.Remote.ToString());
		}

        private void ReconnectClientOnMt(FFTcpClient a_client)
        {
            _clients[a_client.Remote] = a_client;
            RegisterClientCallback(a_client);
            if (onClientReconnection != null)
                onClientReconnection(a_client);
        }
        #endregion

        #region Replacing Client
        protected List<IPEndPoint> _replacedClients;
        //internal IPEndPointCallback onPlayerReplaced = null;
        internal void OnIdReceived(FFTcpClient a_client)
        {
            if (_idMapping.ContainsKey(a_client.NetworkID))
            {
                IPEndPoint ep = _idMapping[a_client.NetworkID];
                _replacedClients.Add(ep);
                _idMapping[a_client.NetworkID] = a_client.Remote;
                /*if(onPlayerReplaced != null)
                    onPlayerReplaced(ep);*/
            }
            else
            {
                _idMapping.Add(a_client.NetworkID, a_client.Remote);
            }
        }
        #endregion
    }
}