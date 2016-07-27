using UnityEngine;
using System.Collections;

using FF;
using System;

namespace FF.Network
{
    internal class ClientConnectionState : IFsmState<EClientConnectionState>
    {
        #region Properties
        FFNetworkClient _client;
        protected SimpleCallback _onSuccess;
        protected IntCallback _onfail;

        #region Connection Properties
        protected FFTcpConnectionTask _connectionTask;
        protected bool _isConnecting = false;

        protected bool _didSucceed = false;
        protected bool _didFailed = false;

        protected long _lastConnectionAttemptTicks = 0;

        protected int _connectionAttemptCount = 0;
        #endregion
        #endregion

        internal ClientConnectionState(FFNetworkClient a_client, SimpleCallback a_onSuccess, IntCallback a_onFail)
        {
            _client = a_client;
            _onSuccess = a_onSuccess;
            _onfail = a_onFail;
            _connectionTask = new FFTcpConnectionTask(_client, OnConnectionSuccess, OnConnectionFailed);
        }

        internal void TearDown()
        {
            _onSuccess = null;
            _onfail = null;
            _connectionTask.TearDown();
        }

        #region Interface
        public EClientConnectionState ID
        {
            get
            {
                return EClientConnectionState.Connection;
            }
        }

        public void Enter(EClientConnectionState a_previousStateId)
        {
            FFLog.Log(EDbgCat.ClientConnection, "Enter connection");
            _didSucceed = false;
            _didFailed = false;
            _connectionAttemptCount = 0;
            
            TryConnect();
        }

        public EClientConnectionState DoUpdate()
        {
            if (_didSucceed)
            {
                _onSuccess();
                return EClientConnectionState.VersionCompatibility;
            }
            else if (_didFailed)
            {
                _onfail(_connectionAttemptCount);
                Reset();
                return ID;
            }
            else if (!_isConnecting)
            {
                TryConnect();
            }
            return ID;
        }

        public void Exit(EClientConnectionState a_targetStateId)
        {
            FFLog.Log(EDbgCat.ClientConnection, "Exit connection");
            _connectionTask.Stop();
            _isConnecting = false;
        }
        #endregion

        #region Connection Task callback
        protected void OnConnectionSuccess()
        {
            _isConnecting = false;
            _didSucceed = true;
            FFLog.Log(EDbgCat.ClientConnection, "Connection success");
        }

        protected void OnConnectionFailed()
        {
            _isConnecting = false;
            _didFailed = true;
            _connectionAttemptCount++;
            FFLog.Log(EDbgCat.ClientConnection, "Connection failed");
        }
        #endregion

        protected void Reset()
        {
            _didSucceed = false;
            _didFailed = false;
        }

        protected void TryConnect()
        {
            _connectionTask.Start();
            _isConnecting = true;
        }
    }
}