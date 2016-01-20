using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using System.Net.Sockets;
using System.Net;

using FF.Network.Receiver;
using FF.Network.Message;

namespace FF.Network
{
    internal class FFTcpClient
    {
        #region Request Properties
        protected int _requestIndex;
        protected Dictionary<int, ARequest> _pendingSentRequest;
        internal ARequest SentRequestForId(int a_id)
        {
            ARequest toReturn = null;
            _pendingSentRequest.TryGetValue(a_id, out toReturn);
            return toReturn;
        }

        protected Dictionary<int, ARequest> _pendingReadRequest;
        internal ARequest ReadRequestForId(int a_id)
        {
            ARequest toReturn = null;
            _pendingReadRequest.TryGetValue(a_id, out toReturn);
            return toReturn;
        }
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
        protected Queue<AMessage> _writtenMessages;
        protected Queue<AMessage> _readMessages;
        protected Queue<int> _readRequestToCancel;
        protected Queue<int> _sentRequestToCancel;
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
            _requestIndex = 0;
            _writtenMessages = new Queue<AMessage>();
            _pendingSentRequest = new Dictionary<int, ARequest>();
            _pendingReadRequest = new Dictionary<int, ARequest>();
            _readMessages = new Queue<AMessage>();
            _readRequestToCancel = new Queue<int>();
            _sentRequestToCancel = new Queue<int>();
        }

        /// <summary>
        /// Called by the client
        /// </summary>
        internal FFTcpClient(int a_netId, IPEndPoint a_local, IPEndPoint a_remote)
        {
            _networkID = a_netId;

            _requestIndex = 0;
            _writtenMessages = new Queue<AMessage>();
            _pendingSentRequest = new Dictionary<int, ARequest>();
            _pendingReadRequest = new Dictionary<int, ARequest>();
            _readMessages = new Queue<AMessage>();
            _readRequestToCancel = new Queue<int>();
            _sentRequestToCancel = new Queue<int>();

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
            _writtenMessages = new Queue<AMessage>();
            _pendingSentRequest = new Dictionary<int, ARequest>();
            _pendingReadRequest = new Dictionary<int, ARequest>();
            _readMessages = new Queue<AMessage>();
            _readRequestToCancel = new Queue<int>();
            _sentRequestToCancel = new Queue<int>();

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

        #region Start & Stop	
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

            foreach (ARequest each in _pendingReadRequest.Values)
            {
                each.Cancel(false);
            }
            _pendingReadRequest.Clear();
            _readRequestToCancel.Clear();

            foreach (ARequest each in _pendingSentRequest.Values)
            {
                each.ForceFail();
            }
            _pendingSentRequest.Clear();
            _sentRequestToCancel.Clear();

            _writtenMessages.Clear();

            TryReadMessages();

            _readMessages.Clear();
        }

        internal virtual void StartWorkers()
        {
            FFLog.Log(EDbgCat.Networking, "Starting workers");
            _reader.Start();
            _writer.Start();
        }
        #endregion

        #region Messages
        internal virtual void QueueWrittenMessage(AMessage a_message)
        {
            lock(_writtenMessages)
            {
                _writtenMessages.Enqueue(a_message);
            }
        }

        internal virtual void QueueMessage(AMessage a_message)
        {
            a_message.Client = this;
            if (a_message is AResponse)
            {
                AResponse res = a_message as AResponse;
                _pendingReadRequest.Remove(res.requestId);
            }
            else if (a_message is ARequest)
            {
                ARequest req = a_message as ARequest;
                req.requestId = _requestIndex;
                _pendingSentRequest.Add(req.requestId, req);
                _requestIndex++;
            }
            _writer.QueueMessage(a_message);
        }

        internal virtual void QueueFinalMessage(AMessage a_message)
        {
            a_message.Client = this;
            if (a_message is AResponse)
            {
                AResponse res = a_message as AResponse;
                _pendingReadRequest.Remove(res.requestId);
            }
            else if (a_message is ARequest)
            {
                ARequest req = a_message as ARequest;
                req.requestId = _requestIndex;
                _pendingSentRequest.Add(req.requestId, req);
                _requestIndex++;
            }
            _writer.QueueFinalMessage(a_message);
        }

        internal virtual void QueueReadMessage(AMessage a_message)
        {
            FFLog.Log(EDbgCat.Networking, "Queueing read message");
            /* if (a_message is FFResponseMessage)
                 FFLog.LogError(EDbgCat.Networking, "Queueing read response : " + a_message.ToString());*/
            lock (_readMessages)
            {
                _readMessages.Enqueue(a_message);
            }
        }

        internal virtual void RemoveReadRequest(int a_requestId)
        {
            _readRequestToCancel.Enqueue(a_requestId);
        }

        protected void RemoveReadRequestOnMt(int a_requestId)
        {
            _pendingReadRequest.Remove(a_requestId);
        }

        internal virtual void RemoveSentRequest(int a_requestId)
        {
            _sentRequestToCancel.Enqueue(a_requestId);
        }

        protected void RemoveSentRequestOnMt(int a_requestId)
        {
            _pendingSentRequest.Remove(a_requestId);
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

            CheckForWrittenRequest();
            CheckForSentRequestTimeout();
            CheckForReadRequestTimeout();
            TryReadMessages();
        }

       

        protected virtual void TryReadMessages()
        {
            while (_readMessages.Count > 0)
            {
                AMessage messageRead = null;
                lock (_readMessages)
                {
                    messageRead = _readMessages.Dequeue();
                }
                FFLog.Log(EDbgCat.Networking, "Reading new message : " + messageRead.ToString());

                messageRead.Client = this;
                if (messageRead is ARequest)// Request
                {
                    ARequest request = messageRead as ARequest;
                    _pendingReadRequest.Add(request.requestId, request);
                }

                List<BaseReceiver> receivers = Engine.Receiver.ReceiversForType(messageRead.Type);
                if (receivers != null)
                {
                    receivers = new List<BaseReceiver>();
                    foreach (BaseReceiver each in Engine.Receiver.ReceiversForType(messageRead.Type))
                    {
                        receivers.Add(each);
                    }

                    foreach (BaseReceiver each in receivers)
                    {
                        each.Read(messageRead);
                    }
                }
            }
        }

        protected void CheckForWrittenRequest()
        {
            if (_writtenMessages.Count > 0)
            {
                lock (_writtenMessages)
                {
                    _writtenMessages.Dequeue().PostWrite();
                }
            }
        }

        protected void CheckForSentRequestTimeout()
        {
            foreach (int id in _pendingSentRequest.Keys)
            {
                if (_pendingSentRequest[id].CheckForTimeout())
                    _sentRequestToCancel.Enqueue(id);
            }
            while (_sentRequestToCancel.Count > 0)
            {
                RemoveSentRequestOnMt(_sentRequestToCancel.Dequeue());
            }
        }

        protected void CheckForReadRequestTimeout()
        {
            foreach (int id in _pendingReadRequest.Keys)
            {
                if (_pendingReadRequest[id].CheckForTimeout())
                    _readRequestToCancel.Enqueue(id);
            }
            while (_readRequestToCancel.Count > 0)
            {
                RemoveSentRequestOnMt(_readRequestToCancel.Dequeue());
            }
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
            if (Engine.NetworkStatus.IsConnectedToLan)
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
        internal FFClientCallback onConnectionSuccess = null;
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
                QueueMessage(new MessageNetworkID(NetworkID));
            }

            _didReconnect = true;
            _autoRetryConnection = true;
        }

        internal virtual void ConnectionFailed()
        {
            FFLog.Log(EDbgCat.Networking, "Connection Failed.");
            _isConnecting = false;
            _connectionTryCount++;
            FFTcpClient main = Engine.Network.MainClient;
            if ((main == null || Engine.Network.MainClient != this) && _connectionTryCount >= MAX_CONNECTION_TRY) // TIME OUT
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
        internal FFClientCallback onConnectionLost = null;
        internal virtual void ConnectionLost()
        {
            FFLog.Log(EDbgCat.Networking, "Connection Lost.");
            _didLostConnection = true;
        }

        /// <summary>
        /// Called when the TCPClient properly disconnect from user/server request.
        /// </summary>
        internal FFDisconnectedCallback onConnectionEnded = null;
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