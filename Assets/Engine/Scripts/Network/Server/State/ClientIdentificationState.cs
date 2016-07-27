using UnityEngine;
using System.Collections;

using FF.Network.Message;

namespace FF.Network
{
    internal class ClientIdentificationState : IFsmState<EClientConnectionState>
    {
        #region Properties
        FFNetworkClient _client;
        protected IntCallback _onSuccess;
        protected IntCallback _onfail;

        protected bool _didSucceed = false;
        protected bool _didFailed = false;
        protected int _serverId = 0;

        #region Version Compatibility Properties
        SentRequest _networkIdRequest = null;
        #endregion
        #endregion

        internal ClientIdentificationState(FFNetworkClient a_client,
                                            IntCallback a_onSuccess,
                                            IntCallback a_onFail)
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
                return EClientConnectionState.Identification;
            }
        }

        public void Enter(EClientConnectionState a_previousStateId)
        {
            _didSucceed = false;
            _didFailed = false;
            _serverId = -1;

            _networkIdRequest = new SentRequest(new MessageIntegerData(_client.NetworkID),
                                                EMessageChannel.NetworkId.ToString(),
                                                Engine.Network.NextRequestId,
                                                -1f);
            _networkIdRequest.onSucces += OnNetworkIdSuccess;
            _networkIdRequest.onFail += OnNetworkIdfailed;
            _client.QueueRequest(_networkIdRequest);
        }

        public EClientConnectionState DoUpdate()
        {
            if (_didSucceed)
            {
                _onSuccess(_serverId);
                return EClientConnectionState.Connected;
            }
            else if (_didFailed)
            {
                _onfail(_serverId);
                return EClientConnectionState.Disconnected;
            }
            return ID;
        }

        public void Exit(EClientConnectionState a_targetStateId)
        {
            if (!_networkIdRequest.IsComplete)
            {
                _networkIdRequest.Cancel();
            }
        }
        #endregion

        #region NetworkId callback
        protected void OnNetworkIdSuccess(ReadResponse a_response)
        {
            _didSucceed = true;
            MessageIntegerData data = a_response.Data as MessageIntegerData;
            _serverId = data.Data;
        }

        protected void OnNetworkIdfailed(ERequestErrorCode a_errCode, ReadResponse a_response)
        {
            _didFailed = true;
            if (a_errCode == ERequestErrorCode.Failed)
            {
                MessageIntegerData data = a_response.Data as MessageIntegerData;
                _serverId = data.Data;
            }
            else
            {
                _serverId = -1;
            }
        }
        #endregion
    }
}