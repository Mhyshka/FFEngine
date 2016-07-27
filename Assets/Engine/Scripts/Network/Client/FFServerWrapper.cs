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
    internal class FFServerWrapper : FFNetworkClient
    {
        #region States
        protected ServerCompatibilityState _compatibilityState;
        protected ServerIdentificationState _identificationState;
        protected EmptyNetworkState _connectedState;
        protected DisconnectedState _disconnectedState;
        #endregion

        

        #region Constructors
        /// <summary>
        /// Called by the server when accepting a new connection
        /// </summary>
        internal FFServerWrapper(TcpClient a_client, int a_id)
        {
            _writtenMessages = new Queue<SentMessage>();
            _pendingSentRequest = new Dictionary<long, SentRequest>();
            _pendingReadRequest = new Dictionary<long, ReadRequest>();
            _readMessages = new Queue<ReadMessage>();
            _readRequestToCancel = new Queue<long>();
            _sentRequestToRemove = new Queue<long>();
            _sentRequestToAdd = new Queue<SentRequest>();

            _clock = new ClientClock(this);

            _compatibilityState = new ServerCompatibilityState(this);
            _identificationState = new ServerIdentificationState(this);
            _connectedState = new EmptyNetworkState();
            _disconnectedState = new DisconnectedState(this);

            _states = new Dictionary<EClientConnectionState, IFsmState<EClientConnectionState>>();
            _states.Add(EClientConnectionState.VersionCompatibility, _compatibilityState);
            _states.Add(EClientConnectionState.Identification, _identificationState);
            _states.Add(EClientConnectionState.Connected, _connectedState);
            _states.Add(EClientConnectionState.Disconnected, _disconnectedState);

            _currentState = _compatibilityState;
            _targetState = _compatibilityState;
            _compatibilityState.Enter(EClientConnectionState.None);

            _tcpClient = a_client;
            _tcpClient.NoDelay = true;
            _tcpClient.SendTimeout = 1000;
            _tcpClient.ReceiveTimeout = 3000;

            _local = _tcpClient.Client.LocalEndPoint as IPEndPoint;
            _remote = _tcpClient.Client.RemoteEndPoint as IPEndPoint;

            _reader = new FFTcpReader(this, OnConnectionLost);
            _writer = new FFTcpWriter(this, OnConnectionLost);

            _clock.SetConnectionTime(DateTime.Now);

            StartWorkers();

            _networkID = a_id;
        }
        #endregion


    }
}
