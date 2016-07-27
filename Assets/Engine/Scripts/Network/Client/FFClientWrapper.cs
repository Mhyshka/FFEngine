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
    internal class FFClientWrapper : FFNetworkClient
    {
        #region States
        protected ClientConnectionState _connectingState;
        protected ClientCompatibilityState _compatibilityState;
        protected ClientIdentificationState _identificationState;
        protected ClientConnectionLostState _connectionLostState;
        protected EmptyNetworkState _connectedState;
        protected DisconnectedState _disconnectedState;
        #endregion

        #region Callback
        internal SimpleCallback onReconnection = null;
        #endregion
        #region Constructors
        /// <summary>
        /// Called by the client
        /// </summary>
        internal FFClientWrapper(IPEndPoint a_local, IPEndPoint a_remote)
        {
            _writtenMessages = new Queue<SentMessage>();
            _pendingSentRequest = new Dictionary<long, SentRequest>();
            _pendingReadRequest = new Dictionary<long, ReadRequest>();
            _readMessages = new Queue<ReadMessage>();
            _readRequestToCancel = new Queue<long>();
            _sentRequestToRemove = new Queue<long>();
            _sentRequestToAdd = new Queue<SentRequest>();

            _clock = new ClientClock(this);

            _connectingState = new ClientConnectionState(this,
                                                            OnConnectionSuccess,
                                                            OnConnectionFailed);
            _compatibilityState = new ClientCompatibilityState(this,
                                                                OnVersionCompatibiltySuccess,
                                                                OnVersionCompatibiltyFailed);
            _identificationState = new ClientIdentificationState(this,
                                                                OnNetworkIdVerificationSuccess,
                                                                OnNetworkIdVerificationFailed);
            _connectedState = new EmptyNetworkState();
            _connectionLostState = new ClientConnectionLostState(this);
            _disconnectedState = new DisconnectedState(this);

            _states = new Dictionary<EClientConnectionState, IFsmState<EClientConnectionState>>();
            _states.Add(EClientConnectionState.Connection, _connectingState);
            _states.Add(EClientConnectionState.VersionCompatibility, _compatibilityState);
            _states.Add(EClientConnectionState.Identification, _identificationState);
            _states.Add(EClientConnectionState.Connected, _connectedState);
            _states.Add(EClientConnectionState.ConnectionLost, _connectionLostState);
            _states.Add(EClientConnectionState.Disconnected, _disconnectedState);

            _currentState = null;
            _targetState = null;

            _local = a_local;
            _remote = a_remote;

            _reader = new FFTcpReader(this, OnConnectionLost);
            _writer = new FFTcpWriter(this, OnConnectionLost);
        }

        internal override void Close()
        {
            base.Close();

            onConnectionSuccess = null;
            onReconnection = null;
            onConnectionFailed = null;
            onIdCheckFailed = null;
        }
        #endregion

        #region Connection
        internal FFClientCallback onConnectionSuccess = null;
        internal FFIntClientCallback onConnectionFailed = null;
        internal virtual void Connect()
        {
            _targetState = _states[EClientConnectionState.Connection];
        }

        /// <summary>
        /// Called from the Connecting State when the TCPClient successfully connect with the server.
        /// </summary>
        protected virtual void OnConnectionSuccess()
        {
            FFLog.Log(EDbgCat.ClientConnection, "Client Connection Success.");
            _clock.SetConnectionTime(DateTime.Now);

            StartWorkers();

            if (onConnectionSuccess != null)
                onConnectionSuccess(this);

            _local = _tcpClient.Client.LocalEndPoint as IPEndPoint;
        }

        /// <summary>
        /// Called from the Connecting State when the TCPClient failed to connect with the server.
        /// The ConnectionState will automatically retry.
        /// </summary>
        protected virtual void OnConnectionFailed(int a_attemptCount)
        {
            FFLog.Log(EDbgCat.ClientConnection, "Client Connection Failed.");
            if (onConnectionFailed != null)
                onConnectionFailed(this, a_attemptCount);
        }

        protected override void OnConnectionLost()
        {
            if (_isWorkersRunning)
            {
                StopWorkers();
                _tcpClient.Close();
                _tcpClient = null;
                _local.Port = 0;

                FFLog.LogWarning(EDbgCat.ClientConnection, "Connection lost : Thread crash");
                _targetState = _connectionLostState;
            }
        }

        internal void OnConnectionLostOnMt()
        {
            if (onConnectionLost != null)
                onConnectionLost(this);
        }
        #endregion

        #region Version Compatibility
        internal FFVersionClientCallback onVersionCheckFail = null;
        protected virtual void OnVersionCompatibiltySuccess(FFVersion a_version)
        {
            FFLog.Log(EDbgCat.ClientIdentification, "Version is compatible : " + Engine.Network.NetworkVersion.ToString());

            if (onVersionCompatibilityVerificationSuccess != null)
                onVersionCompatibilityVerificationSuccess(this);
        }

        protected virtual void OnVersionCompatibiltyFailed(FFVersion a_version)
        {
            FFLog.Log(EDbgCat.ClientIdentification, "Version is NOT compatible : " + Engine.Network.NetworkVersion.ToString());

            if (onVersionCheckFail != null)
                onVersionCheckFail(this, a_version, Engine.Network.NetworkVersion);

            Disconnect();
        }
        #endregion

        #region Network Id
        internal FFIdCheckClientCallback onIdCheckFailed = null;
        protected virtual void OnNetworkIdVerificationSuccess(int a_serverId)
        {
            FFLog.Log(EDbgCat.ClientIdentification, "Network id verified : New Client");

            if (onReconnection != null && _networkID != -1)
                onReconnection();

            if (onIdCheckCompleted != null)
                onIdCheckCompleted(this, a_serverId, NetworkID);

            _networkID = a_serverId;
        }

        protected virtual void OnNetworkIdVerificationFailed(int a_serverId)
        {
            FFLog.Log(EDbgCat.ClientIdentification, "Network id verification failed.");

            if (onIdCheckFailed != null)
                onIdCheckFailed(this, a_serverId, NetworkID);

            Disconnect();
        }
        #endregion

    }
}
