using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using System.Net.Sockets;
using System.Net;

namespace FF.Networking
{
	internal delegate void FFTcpClientCallback(FFTcpClient a_client);
    internal delegate void FFTcpDisconnectedCallback(FFTcpClient a_client, string a_reason);

    internal class FFTcpClient
	{
		#region Properties
		protected int _requestIndex;
		protected Dictionary<int, FFRequestMessage> _pendingRequest;
		protected TcpClient _tcpClient;
		internal TcpClient TcpClient
		{
			get
			{
				return _tcpClient;
			}
		}
		
		protected IPEndPoint _remote;
		internal IPEndPoint Remote
		{
			get
			{
				return _remote;
			}
		}
		
		protected IPEndPoint _local;
		internal IPEndPoint Local
		{
			get
			{
				
				return _remote;
			}
		}
		
		protected FFTcpReader _reader;
		protected FFTcpWriter _writer;
		
		protected Queue<FFMessage> _readMessages = new Queue<FFMessage>();
		
		internal void QueueReadMessage(FFMessage a_message)
		{
			FFLog.Log(EDbgCat.Networking, "Queueing read message");
			lock(_readMessages)
			{
				_readMessages.Enqueue(a_message);
			}
		}

        protected FFTcpConnectionTask _connectionTask;
        protected bool _isConnecting = false;

		protected bool _wasConnected;

        protected const int MAX_CONNECTION_TRY = 10;
        protected int _connectionTryCount = 0;
        #endregion

        #region Constructors
        internal bool autoRetryConnection;

		/// <summary>
		/// Called by the client
		/// </summary>
		internal FFTcpClient(IPEndPoint a_local, IPEndPoint a_remote)
		{
			_requestIndex = 0;
			_pendingRequest = new Dictionary<int, FFRequestMessage>();
			
			_local = a_local;
			_remote = a_remote;
			_wasConnected = false;
			autoRetryConnection = false;

            _connectionTask = new FFTcpConnectionTask(this);
            _reader = new FFTcpReader(this);
			_writer = new FFTcpWriter(this);
		}
		
		/// <summary>
		/// Called by the server when accepting the connection
		/// </summary>
		internal FFTcpClient(TcpClient a_client)
		{
			FFLog.LogError("Connected? : " + a_client.Connected.ToString());
			_wasConnected = true;
			autoRetryConnection = false;
			
			_tcpClient = a_client;
            _tcpClient.SendTimeout = 0;
            _tcpClient.ReceiveTimeout = 0;

            _local = _tcpClient.Client.LocalEndPoint as IPEndPoint;
			_remote = _tcpClient.Client.RemoteEndPoint as IPEndPoint;

            _reader = new FFTcpReader(this);
			_writer = new FFTcpWriter(this);
		}
		#endregion
		
		#region Interface	
		internal void Stop()
		{
			FFLog.Log(EDbgCat.Networking, "Stoping Client");
            _wasConnected = false;

            if(_connectionTask != null)
                _connectionTask.Stop();

            _reader.Stop();
            _writer.Stop();
            if (_tcpClient != null)
			{
                if(_tcpClient.Connected)
				    _tcpClient.GetStream().Close();
				_tcpClient.Close();
            }
			_tcpClient = null;
			
		}
		
		internal void Close()
		{
            FFLog.Log(EDbgCat.Networking, "Closing Client");
            Stop ();
			autoRetryConnection = false;
		}
		
		internal void StartWorkers()
		{
			FFLog.Log(EDbgCat.Networking, "Starting workers");
			_reader.Start();
			_writer.Start();
		}
		
		internal void QueueMessage(FFMessage a_message)
		{
			if(a_message is FFRequestMessage)
			{
				FFRequestMessage req = a_message as FFRequestMessage;
				req.requestId = _requestIndex;
				_pendingRequest.Add(req.requestId, req);
				_requestIndex++;
			}
			_writer.QueueMessage(a_message);
		}

        internal void QueueFinalMessage(FFMessage a_message)
        {
            _writer.QueueFinalMessage(a_message);
        }
        #endregion
        internal void DoUpdate()
		{
            
			if(!_wasConnected && !_isConnecting && autoRetryConnection)
			{
                if (!Connect())
                    ConnectionFailed();
			}
			
			if(_wasConnected)//Was Connected
			{
                if (!IsConnected)
				{
					ConnectionLost();
				}
			}
			
			while(_readMessages.Count > 0 )
			{
				FFMessage messageRead = null;
				lock(_readMessages)
				{
					messageRead =_readMessages.Dequeue();
				}
				FFLog.Log(EDbgCat.Networking,"Reading new message : " + messageRead.ToString());
				
				bool isRead = false;
			
				if(messageRead is FFResponseMessage)
				{
					FFResponseMessage response = messageRead as FFResponseMessage;
					FFRequestMessage req = null;
					if(_pendingRequest.TryGetValue(response.requestId, out req))
					{
						isRead = true;
						response.Read(this,req);
					}
				}
				
				if(!isRead)
				{
					messageRead.Read(this);
				}
			}
		}

        #region Connection
        internal bool IsConnected
        {
            get
            {
                return _tcpClient != null && _tcpClient.Connected;
            }
        }

        internal bool Connect()
		{
            FFLog.Log(EDbgCat.Networking, "Trying to connect to : " + _remote.ToString() + " from : " + _local.ToString());
            if (FFEngine.NetworkStatus.IsConnectedToLan)
            {
                try
                {
                    _tcpClient = new TcpClient(_local);
                    _tcpClient.SendTimeout = 0;
                    _tcpClient.ReceiveTimeout = 0;

                    _isConnecting = true;
                    _connectionTask.Start();

                    _wasConnected = false;
                    return true;
                }
                catch (SocketException e)
                {
                    FFLog.LogError("Couldn't connect to server." + e.StackTrace);
                }
            }
            else
            {
                FFLog.LogWarning("Couldn't connect to server : Not lan network.");
            }
            return false;
        }
		
		/// <summary>
		/// Called when the TCPClient successfully connect with the server.
		/// </summary>
		internal FFTcpClientCallback onConnectionSuccess = null;
		internal void ConnectionSuccess()
		{
            FFLog.Log(EDbgCat.Networking, "Connection Success.");
            _isConnecting = false;
            _connectionTryCount = 0;
            _wasConnected = true;
            _local = _tcpClient.Client.LocalEndPoint as IPEndPoint;
            StartWorkers();

            if (onConnectionSuccess != null)
                onConnectionSuccess(this);
            autoRetryConnection = true;
		}

        internal void ConnectionFailed()
        {
            FFLog.Log(EDbgCat.Networking, "Connection Failed.");
            _isConnecting = false;
            _connectionTryCount++;
            FFTcpClient main = FFEngine.Network.MainClient;
            if ((main == null || FFEngine.Network.MainClient != this) && _connectionTryCount >= MAX_CONNECTION_TRY) // TIME OUT
            {
                EndConnection("Server unreachable.");
            }
            else
            {
                autoRetryConnection = true;
            }
        }
		
		/// <summary>
		/// Called when the TCPClient encounter an error & disconnects.
		/// </summary>
		internal FFTcpClientCallback onConnectionLost = null;
		internal void ConnectionLost()
		{
			FFLog.Log(EDbgCat.Networking,"Connection Lost.");
			Stop();
			
			if(onConnectionLost != null)
				onConnectionLost(this);
		}

        /// <summary>
        /// Called when the TCPClient properly disconnect from user/server request.
        /// </summary>
        internal FFTcpDisconnectedCallback onConnectionEnded = null;
        internal void EndConnection(string a_reason)
        {
            FFLog.Log(EDbgCat.Networking, "Connection Ended.");
            Close();

            if (onConnectionEnded != null)
                onConnectionEnded(this, a_reason);
        }
		#endregion
	}
}