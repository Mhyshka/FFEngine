using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using System.Net.Sockets;
using System.Net;

using FF.Network.Receiver;
using FF.Network.Message;

internal enum EClientConnectionState
{
    None = -1,
    Connection = 0,
    VersionCompatibility,
    Identification,
    Connected,
    ConnectionLost,
    Disconnected
}

namespace FF.Network
{
    internal abstract class FFNetworkClient
    {
        #region Properties Manager
        protected ClientClock _clock = null;
        internal ClientClock Clock
        {
            get
            {
                return _clock;
            }
        }
        #endregion

        #region States
        protected Dictionary<EClientConnectionState, IFsmState<EClientConnectionState>> _states;
        protected IFsmState<EClientConnectionState> _currentState;
        protected IFsmState<EClientConnectionState> _targetState;
        #endregion

        #region Status
        /// <summary>
        /// Returns if this TcpSocket is alive and can message the remote.
        /// </summary>
        internal virtual bool IsConnected
        {
            get
            {
                if (_currentState == null)
                    return false;

                return _currentState.ID == EClientConnectionState.Connected ||
                        _currentState.ID == EClientConnectionState.VersionCompatibility ||
                        _currentState.ID == EClientConnectionState.Identification;
            }
        }

        /// <summary>
        /// Returns if this client is ready to be used by the application
        /// </summary>
        internal virtual bool IsReady
        {
            get
            {
                if (_currentState == null)
                    return false;

                return _currentState.ID == EClientConnectionState.Connected;
            }
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
            set
            {
                _tcpClient = value;
                _tcpClient.Client.NoDelay = true;
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

                return _local;
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
        #endregion

        #region Close
        /// <summary>
        /// Close the client. Can NOT be restarted.
        /// </summary>
        internal virtual void Close()
        {
            FFLog.Log(EDbgCat.Client, "Closing client : " + _networkID);
            //Stoping threads
            StopWorkers();

            if (_tcpClient != null)
            {
                _tcpClient.Close();
            }
            _tcpClient = null;

            if (_currentState != null)
            {
                _currentState.Exit(_currentState.ID);
            }
            _currentState = null;
            _targetState = null;

            //Clearing callbacks
            onVersionCompatibilityVerificationSuccess = null;

            onIdCheckCompleted = null;

            onConnectionLost = null;

            if (onConnectionEnded != null)
            {
                onConnectionEnded(this);
                onConnectionEnded = null;
            }

            //Clearing collections
            foreach (ReadRequest each in _pendingReadRequest.Values)
            {
                each.FailWithoutResponse(ERequestErrorCode.Canceled);
            }
            _pendingReadRequest.Clear();
            _readRequestToCancel.Clear();

            foreach (SentRequest each in _pendingSentRequest.Values)
            {
                each.OnFail(ERequestErrorCode.Canceled, null);
            }
            _pendingSentRequest.Clear();
            _sentRequestToRemove.Clear();

            _writtenMessages.Clear();
            _readMessages.Clear();
        }
        #endregion

        #region Workers
        protected bool _isWorkersRunning = false;
        internal virtual void StartWorkers()
        {
            FFLog.Log(EDbgCat.Client, "Starting workers");
            _reader.Start();
            _writer.Start();
            _isWorkersRunning = true;
        }

        internal virtual void StopWorkers()
        {
            FFLog.Log(EDbgCat.Client, "Stoping workers");
            _isWorkersRunning = false;
            _reader.Stop();
            _writer.Stop();
        }
        #endregion

        #region Messages

        #region Sync Properties
        protected Queue<SentMessage> _writtenMessages;
        protected Queue<ReadMessage> _readMessages;
        protected Queue<long> _readRequestToCancel;
        protected Queue<long> _sentRequestToRemove;
        protected Queue<SentRequest> _sentRequestToAdd;
        #endregion

        #region Properties
        protected Dictionary<long, SentRequest> _pendingSentRequest;
        internal SentRequest SentRequestForId(long a_id)
        {
            SentRequest toReturn = null;
            _pendingSentRequest.TryGetValue(a_id, out toReturn);
            return toReturn;
        }

        protected Dictionary<long, ReadRequest> _pendingReadRequest;
        internal ReadRequest ReadRequestForId(long a_id)
        {
            ReadRequest toReturn = null;
            _pendingReadRequest.TryGetValue(a_id, out toReturn);
            return toReturn;
        }
        #endregion

        #region Write
        internal virtual void QueueMessage(SentMessage a_message)
        {
            QueueMessageOnWriterThread(a_message);
        }

        internal virtual void QueueRequest(SentRequest a_request)
        {
            _sentRequestToAdd.Enqueue(a_request);

            QueueMessageOnWriterThread(a_request);
        }

        internal virtual void QueueResponse(SentResponse a_response)
        {
            _readRequestToCancel.Enqueue(a_response.RequestId);

            QueueMessageOnWriterThread(a_response);
        }

        /// <summary>
        /// Queue the message on the writer thread.
        /// </summary>
        /// <param name="a_message"></param>
        protected virtual void QueueMessageOnWriterThread(SentMessage a_message)
        {
            a_message.Client = this;
            if(_isWorkersRunning)
                _writer.QueueMessage(a_message);
        }

        /// <summary>
        /// Used to kill the Client right after the given message. Will clear the message Queue before being add.
        /// </summary>
        internal virtual void QueueFinalMessage(SentMessage a_message)
        {
            _writer.QueueFinalMessage(a_message);
        }

        /// <summary>
        /// Used to call On Message Sent on main thread
        /// </summary>
        internal virtual void QueueWrittenMessage(SentMessage a_message)
        {
            lock (_writtenMessages)
            {
                _writtenMessages.Enqueue(a_message);
            }
        }
        #endregion

        #region Read
        /// <summary>
        /// Used to call to Read message on the Main Thread
        /// </summary>
        internal virtual void QueueReadMessage(ReadMessage a_message)
        {
            FFLog.Log(EDbgCat.Message, "Queueing read message");
            lock (_readMessages)
            {
                _readMessages.Enqueue(a_message);
            }
        }

        internal virtual void RemoveReadRequest(long a_requestId)
        {
            _readRequestToCancel.Enqueue(a_requestId);
        }

        protected void RemoveReadRequestOnMt(long a_requestId)
        {
            _pendingReadRequest.Remove(a_requestId);
        }

        internal virtual void RemoveSentRequest(long a_requestId)
        {
            _sentRequestToRemove.Enqueue(a_requestId);
        }

        protected void AddSentRequestOnMt(SentRequest a_request)
        {
            _pendingSentRequest.Add(a_request.RequestId, a_request);
        }

        protected void RemoveSentRequestOnMt(long a_requestId)
        {
            _pendingSentRequest.Remove(a_requestId);
        }
        #endregion
        #endregion

        #region Update
        internal virtual void DoUpdate()
        {
            EClientConnectionState targetId = EClientConnectionState.None;
            bool didUpdate = false;
            while (_currentState != _targetState)
            {
                if (_currentState != null)
                    _currentState.Exit(_targetState != null ? _targetState.ID : EClientConnectionState.None);

                EClientConnectionState previousId = _currentState != null ? _currentState.ID : EClientConnectionState.None;

                _currentState = _targetState;

                if (_currentState != null)
                    _currentState.Enter(previousId);

                didUpdate = true;
                if (_currentState != null)
                {
                    targetId = _currentState.DoUpdate();
                    _targetState = _states[targetId];
                }
            }
            if (!didUpdate)
            {
                if (_currentState != null)
                {
                    targetId = _currentState.DoUpdate();
                    _targetState = _states[targetId];
                }
            }
            
            CheckForWrittenMessage();
            CheckForSentRequestTimeout();
            CheckForReadRequestTimeout();//Unused for now
            TryReadMessages();
        }



        protected virtual void TryReadMessages()
        {
            while (_readMessages.Count > 0)
            {
                ReadMessage messageRead = null;
                lock (_readMessages)
                {
                    messageRead = _readMessages.Dequeue();
                }
                FFLog.Log(EDbgCat.Message, "Reading new message." +
                                           "\nHeader : " + messageRead.HeaderType.ToString() +
                                           "\nData : " + messageRead.Data.Type.ToString() +
                                           "\nChannel : " + messageRead.Channel.ToString());

                messageRead.Client = this;
                if (messageRead is ReadRequest)// Request
                {
                    ReadRequest request = messageRead as ReadRequest;
                    _pendingReadRequest.Add(request.RequestId, request);
                }

                List<BaseReceiver> receivers = Engine.Receiver.ReceiversForChannel(messageRead.Channel);
                if (receivers != null)
                {
                    receivers = new List<BaseReceiver>();
                    foreach (BaseReceiver each in Engine.Receiver.ReceiversForChannel(messageRead.Channel))
                    {
                        receivers.Add(each);
                    }

                    if(receivers.Count == 0)
                        FFLog.LogWarning(EDbgCat.Client, "No receivers found for channel : " + messageRead.Channel);

                    foreach (BaseReceiver each in receivers)
                    {
                        each.Read(messageRead);
                    }
                }
                else
                {
                    FFLog.LogWarning(EDbgCat.Client, "No receivers found for channel : " + messageRead.Channel);
                }
            }
        }

        protected void CheckForWrittenMessage()
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
            while (_sentRequestToAdd.Count > 0)
            {
                AddSentRequestOnMt(_sentRequestToAdd.Dequeue());
            }

            lock (_pendingSentRequest)
            {
                foreach (int id in _pendingSentRequest.Keys)
                {
                    if (_pendingSentRequest[id].CheckForTimeout(Time.fixedDeltaTime))
                    {
                        _pendingSentRequest[id].Timeout();
                    }
                }
            }

            while (_sentRequestToRemove.Count > 0)
            {
                RemoveSentRequestOnMt(_sentRequestToRemove.Dequeue());
            }
        }

        protected void CheckForReadRequestTimeout()
        {
            /*
            lock(_pendingReadRequest)
            {
                foreach (int id in _pendingReadRequest.Keys)
                {
                    if (_pendingReadRequest[id].CheckForTimeout(Time.fixedDeltaTime))
                    {
                        _pendingSentRequest[id].Timeout();
                    }
                }
            }*/
            while (_readRequestToCancel.Count > 0)
            {
                RemoveReadRequestOnMt(_readRequestToCancel.Dequeue());
            }
        }
        #endregion

        #region Identification
        #region Version Compatibility
        internal FFClientCallback onVersionCompatibilityVerificationSuccess = null;
        #endregion

        #region Network Id
        internal FFIdCheckClientCallback onIdCheckCompleted = null;
        #endregion
        #endregion

        #region Connection Lost
        internal FFClientCallback onConnectionLost = null;

        /// <summary>
        /// Called by the Writer or the Reader thread, whichever crash first.
        /// Doesn't run on Main Thread!
        /// </summary>
        protected virtual void OnConnectionLost()
        {
            if (_isWorkersRunning)
            {
                StopWorkers();
                _tcpClient.Close();
                FFLog.LogWarning(EDbgCat.ClientConnection, "Connection lost : Thread crash" );
                Disconnect();
            }
        }
        #endregion

        #region Disconnection
        /// <summary>
        /// Called when the TCPClient properly disconnect from user/server request.
        /// </summary>
        internal FFClientCallback onConnectionEnded = null;
        
        internal virtual void Disconnect()
        {
            FFLog.Log(EDbgCat.ClientConnection, "Disconnecting");
            _targetState = _states[EClientConnectionState.Disconnected];
        }
        #endregion
    }
}