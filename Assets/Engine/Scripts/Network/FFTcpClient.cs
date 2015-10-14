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
        #region Request Properties
        protected int _requestIndex;
        protected Dictionary<int, FFRequestMessage> _pendingSentRequest;
        protected Dictionary<int, FFRequestMessage> _pendingReadRequest;
        protected List<int> _requestToRemove;
        #endregion

        #region TCP Properties
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

        protected int _networkID = -1;
        internal int NetworkID
        {
            get
            {
                return _networkID;
            }
            set
            {
                _networkID = value;
            }
        }

        protected bool _didReconnect = false;
        protected bool _didEndConnection = false;
        protected string _endReason = null;
        protected bool _didLostConnection = false;
        #endregion

        #region Message Read properties
        protected Queue<FFMessage> _readMessages = new Queue<FFMessage>();

        internal virtual void QueueReadMessage(FFMessage a_message)
        {
            FFLog.Log(EDbgCat.Networking, "Queueing read message");
           /* if (a_message is FFResponseMessage)
                FFLog.LogError(EDbgCat.Networking, "Queueing read response : " + a_message.ToString());*/
            lock (_readMessages)
            {
                _readMessages.Enqueue(a_message);
            }
        }
        #endregion

        #region Connection Properties
        protected FFTcpConnectionTask _connectionTask;
        protected bool _isConnecting = false;

        protected bool _wasConnected;
        internal bool WasConnected
        {
            get
            {
                return _wasConnected;
            }
        }

        protected const int MAX_CONNECTION_TRY = 10;
        protected int _connectionTryCount = 0;

        protected bool _autoRetryConnection;
        #endregion

        #region Constructors
        protected FFTcpClient()
        {
        }

        /// <summary>
        /// Called by the client
        /// </summary>
        internal FFTcpClient(int a_netId, IPEndPoint a_local, IPEndPoint a_remote)
        {
            _networkID = a_netId;

            _requestIndex = 0;
            _pendingSentRequest = new Dictionary<int, FFRequestMessage>();
            _pendingReadRequest = new Dictionary<int, FFRequestMessage>();
            _requestToRemove = new List<int>();

            _local = a_local;
            _remote = a_remote;
            _wasConnected = false;
            _autoRetryConnection = false;

            _connectionTask = new FFTcpConnectionTask(this);
            _reader = new FFTcpReader(this);
            _writer = new FFTcpWriter(this);
        }

        /// <summary>
        /// Called by the server when accepting the connection
        /// </summary>
        internal FFTcpClient(TcpClient a_client)
        {
            _requestIndex = 0;
            _pendingSentRequest = new Dictionary<int, FFRequestMessage>();
            _pendingReadRequest = new Dictionary<int, FFRequestMessage>();
            _requestToRemove = new List<int>();

            _wasConnected = true;
            _autoRetryConnection = false;

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
        internal virtual void Stop()
        {
            FFLog.Log(EDbgCat.Networking, "Stoping Client");
            _isConnecting = false;
            _wasConnected = false;
            _endReason = null;
            _didEndConnection = false;
            _didReconnect = false;
            _didLostConnection = false;

            if (_connectionTask != null)
                _connectionTask.Stop();

            _reader.Stop();
            _writer.Stop();
            if (_tcpClient != null)
            {
                if (_tcpClient.Connected)
                    _tcpClient.GetStream().Close();
                _tcpClient.Close();
            }
            _tcpClient = null;

        }

        internal virtual void Close()
        {
            FFLog.Log(EDbgCat.Networking, "Closing Client");
            Stop();
            onConnectionEnded = null;
            onConnectionLost = null;
            onConnectionSuccess = null;
            _autoRetryConnection = false;

            foreach (FFRequestMessage each in _pendingReadRequest.Values)
            {
                each.Cancel(false);
            }
            _pendingReadRequest.Clear();

            foreach (FFRequestMessage each in _pendingSentRequest.Values)
            {
                each.ForceFail();
            }
            _pendingSentRequest.Clear();

            TryReadMessages();

            _readMessages.Clear();
        }

        internal virtual void StartWorkers()
        {
            FFLog.Log(EDbgCat.Networking, "Starting workers");
            _reader.Start();
            _writer.Start();
        }

        internal virtual void QueueMessage(FFMessage a_message)
        {
            a_message.Client = this;
            if (a_message is FFResponseMessage)
            {
                FFResponseMessage res = a_message as FFResponseMessage;
                _pendingReadRequest.Remove(res.requestId);
            }
            else if (a_message is FFRequestMessage)
            {
                FFRequestMessage req = a_message as FFRequestMessage;
                req.requestId = _requestIndex;
                _pendingSentRequest.Add(req.requestId, req);
                _requestIndex++;
            }
            _writer.QueueMessage(a_message);
        }

        internal virtual void QueueFinalMessage(FFMessage a_message)
        {
            a_message.Client = this;
            if (a_message is FFResponseMessage)
            {
                FFResponseMessage res = a_message as FFResponseMessage;
                _pendingReadRequest.Remove(res.requestId);
            }
            else if (a_message is FFRequestMessage)
            {
                FFRequestMessage req = a_message as FFRequestMessage;
                req.requestId = _requestIndex;
                _pendingSentRequest.Add(req.requestId, req);
                _requestIndex++;
            }
            _writer.QueueFinalMessage(a_message);
        }

        internal virtual bool CancelReadRequest(int a_requestId)
        {
            return _pendingReadRequest.Remove(a_requestId);
        }

        internal virtual bool CancelSentRequest(int a_requestId)
        {
            return _pendingSentRequest.Remove(a_requestId);
        }
        #endregion

        #region Update
        internal virtual void DoUpdate()
        {
            if (_didEndConnection)
            {
                EndConnectionOnMt();
            }

            if (_didReconnect)
            {
                ReconnectOnMt();
            }

            if (_didLostConnection)
            {
                LostConnectionOnMt();
            }

            if (!_wasConnected && !_isConnecting && _autoRetryConnection)
            {
                if (!Connect())
                    ConnectionFailed();
            }

            if (_wasConnected)//Was Connected
            {
                if (!IsConnected)
                {
                    ConnectionLost();
                }
            }

            CheckForSentRequestTimeout();
            CheckForReadRequestTimeout();
            TryReadMessages();
        }

       

        protected virtual void TryReadMessages()
        {
            while (_readMessages.Count > 0)
            {
                FFMessage messageRead = null;
                lock (_readMessages)
                {
                    messageRead = _readMessages.Dequeue();
                }
                FFLog.Log(EDbgCat.Networking, "Reading new message : " + messageRead.ToString());

                messageRead.Client = this;
                if (messageRead is FFRequestMessage)// Request
                {
                    FFRequestMessage request = messageRead as FFRequestMessage;
                    _pendingReadRequest.Add(request.requestId, request);
                    request.Read();
                }
                else if (messageRead is FFRequestCancel)// Cancel Request
                {
                    FFRequestCancel cancel = messageRead as FFRequestCancel;
                    FFRequestMessage request = null;
                    if (_pendingReadRequest.TryGetValue(cancel.requestId, out request))
                    {
                        cancel.Read(request);
                    }
                }
                else if (messageRead is FFResponseMessage)// Response
                {
                    FFResponseMessage response = messageRead as FFResponseMessage;
                    FFRequestMessage req = null;
                    if (_pendingSentRequest.TryGetValue(response.requestId, out req))
                    {
                        response.Read(req);
                        _pendingSentRequest.Remove(response.requestId);
                        /*  FFLog.LogError("Sent request count : " + _pendingSentRequest.Count);
                          FFLog.LogError("Read request count : " + _pendingReadRequest.Count);*/
                    }
                }
                else //Standard message
                {
                    messageRead.Read();
                }
            }
        }

        protected void CheckForSentRequestTimeout()
        {
            foreach (int id in _pendingSentRequest.Keys)
            {
                if (_pendingSentRequest[id].CheckForTimeout())
                    _requestToRemove.Add(id);
            }
            foreach (int id in _requestToRemove)
            {
                _pendingSentRequest.Remove(id);
            }
            _requestToRemove.Clear();
        }

        protected void CheckForReadRequestTimeout()
        {
            foreach (int id in _pendingReadRequest.Keys)
            {
                if (_pendingReadRequest[id].CheckForTimeout())
                    _requestToRemove.Add(id);
            }
            foreach (int id in _requestToRemove)
            {
                _pendingReadRequest.Remove(id);
            }
            _requestToRemove.Clear();
        }
        #endregion

        #region Connection
        internal virtual bool IsConnected
        {
            get
            {
                return _tcpClient != null && _tcpClient.Connected;
            }
        }

        internal virtual bool Connect()
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
                    FFLog.LogWarning("Couldn't connect to server." + e.Message);
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
        protected bool _didSendId = false;
        internal virtual void ConnectionSuccess()
        {
            FFLog.Log(EDbgCat.Networking, "Connection Success.");
            _isConnecting = false;
            _connectionTryCount = 0;
            _wasConnected = true;
            _local = _tcpClient.Client.LocalEndPoint as IPEndPoint;
            StartWorkers();

            if (!_didSendId)
            {
                _didSendId = true;
                QueueMessage(new FFMessageNetworkID(NetworkID));
            }

            _didReconnect = true;
            _autoRetryConnection = true;
        }

        internal virtual void ConnectionFailed()
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
                _autoRetryConnection = true;
            }
        }

        /// <summary>
        /// Called when the TCPClient encounter an error & disconnects.
        /// </summary>
        internal FFTcpClientCallback onConnectionLost = null;
        internal virtual void ConnectionLost()
        {
            FFLog.Log(EDbgCat.Networking, "Connection Lost.");
            _didLostConnection = true;
        }

        /// <summary>
        /// Called when the TCPClient properly disconnect from user/server request.
        /// </summary>
        internal FFTcpDisconnectedCallback onConnectionEnded = null;
        internal virtual void EndConnection(string a_reason)
        {
            FFLog.Log(EDbgCat.Networking, "Connection Ended.");
            _endReason = a_reason;
            _didEndConnection = true;
        }

        private void LostConnectionOnMt()
        {
            FFLog.Log(EDbgCat.Networking, "Connection lost on MT");
            _didLostConnection = false;
            if (onConnectionLost != null)
                onConnectionLost(this);
            Stop();
        }

        private void ReconnectOnMt()
        {
            _didReconnect = false;
            if (onConnectionSuccess != null)
                onConnectionSuccess(this);
        }

        internal void EndConnectionOnMt()
        {
            _didEndConnection = false;
            if (onConnectionLost != null)
                onConnectionLost(this);
            if (onConnectionEnded != null)
                onConnectionEnded(this, _endReason);
            Close();
        }
        #endregion
    }
}