using UnityEngine;
using System.Collections;

using FF.Network.Message;

namespace FF.Network
{
    internal class ClientCompatibilityState : IFsmState<EClientConnectionState>
    {
        #region Properties
        FFNetworkClient _client;
        protected FFVersionCalback _onSuccess;
        protected FFVersionCalback _onfail;

        protected bool _didSucceed = false;
        protected bool _didFailed = false;

        protected FFVersion _serverVersion = null;


        protected int _timeoutCount = 0;
        protected static int MAX_RETRY_COUNT = 10;

        #region Version Compatibility Properties
        SentRequest _versionCompatibilityRequest = null;
        #endregion
        #endregion

        internal ClientCompatibilityState(FFNetworkClient a_client, FFVersionCalback a_onSuccess, FFVersionCalback a_onFail)
        {
            _client = a_client;
            _onSuccess = a_onSuccess;
            _onfail = a_onFail;
        }

        internal void TearDown()
        {
            _onSuccess = null;
            _onfail = null;
        }

        #region Interface
        public EClientConnectionState ID
        {
            get
            {
                return EClientConnectionState.VersionCompatibility;
            }
        }

        public void Enter(EClientConnectionState a_previousStateId)
        {
            FFLog.Log(EDbgCat.ClientIdentification, "Enter Compatibility check");
            _didSucceed = false;
            _didFailed = false;
            _serverVersion = null;

            _timeoutCount = 0;
            SendCompatRequest();
        }

        protected void SendCompatRequest()
        {
            _versionCompatibilityRequest = new SentRequest(new MessageVersionData(Engine.Network.NetworkVersion),
                                                EMessageChannel.VersionCompatibility.ToString(),
                                                Engine.Network.NextRequestId,
                                                2f);
            _versionCompatibilityRequest.onSucces += OnVersionCheckSuccess;
            _versionCompatibilityRequest.onFail += OnVersionCheckFailed;
            _client.QueueRequest(_versionCompatibilityRequest);
        }

        public EClientConnectionState DoUpdate()
        {
            if (_didSucceed)
            {
                _onSuccess(_serverVersion);
                return EClientConnectionState.Identification;
            }
            else if (_didFailed)
            {
                _onfail(_serverVersion);
                return EClientConnectionState.Disconnected;
            }
            return ID;
        }

        public void Exit(EClientConnectionState a_targetStateId)
        {
            FFLog.Log(EDbgCat.ClientIdentification, "Exit Compatibility check");
            if (_versionCompatibilityRequest.IsComplete)
            {
                _versionCompatibilityRequest.Cancel();
            }
        }
        #endregion

        #region Version Check callback
        protected void OnVersionCheckSuccess(ReadResponse a_response)
        {
            _didSucceed = true;
            FFLog.Log(EDbgCat.ClientIdentification, "Version compatibility success.");
            if (a_response.Data.Type == EDataType.IntegerArray)
            {

                MessageVersionData data = a_response.Data as MessageVersionData;
                _serverVersion = data.Data;
                FFLog.Log(EDbgCat.ClientIdentification, "Local version : " + Engine.Network.NetworkVersion.ToString() +
                                                        "Server version : " + _serverVersion.ToString());
            }
        }
        
        protected void OnVersionCheckFailed(ERequestErrorCode a_errCode, ReadResponse a_response)
        {
            FFLog.LogError(EDbgCat.ClientIdentification, "Version compatibility check failed : " + a_errCode.ToString());
            if (a_errCode == ERequestErrorCode.Failed)
            {
                MessageVersionData data = a_response.Data as MessageVersionData;
                _serverVersion = data.Data;
                FFLog.LogError(EDbgCat.ClientIdentification, "Local version : " + Engine.Network.NetworkVersion.ToString() +
                                                                "Server version : " + _serverVersion.ToString());
                _didFailed = true;
            }
            else if(a_errCode == ERequestErrorCode.Timeout && _timeoutCount < MAX_RETRY_COUNT)
            {
                _timeoutCount++;
                SendCompatRequest();
            }
            else
            {
                _serverVersion = null;
                _didFailed = true;
            }
        }
        #endregion
    }
}