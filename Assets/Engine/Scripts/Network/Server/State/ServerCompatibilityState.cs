using UnityEngine;
using System.Collections;

using FF.Network.Message;

namespace FF.Network
{
    internal class ServerCompatibilityState : IFsmState<EClientConnectionState>
    {
        #region Properties
        FFNetworkClient _client;

        protected bool _didSucceed = false;
        protected bool _didFailed = false;

        #region Version Compatibility Properties
        Receiver.GenericMessageReceiver _versionCompatibilityReceiver = null;
        #endregion
        #endregion

        internal ServerCompatibilityState(FFNetworkClient a_client)
        {
            _client = a_client;
            _versionCompatibilityReceiver = new Receiver.GenericMessageReceiver(null,
                                                                                OnVersionCompatibilityReceived);
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
            FFLog.Log(EDbgCat.ClientIdentification, "Enter compatibility check");
            _didSucceed = false;
            _didFailed = false;
            Engine.Receiver.RegisterReceiver(EMessageChannel.VersionCompatibility.ToString(),
                                            _versionCompatibilityReceiver);
        }

        public EClientConnectionState DoUpdate()
        {
            if (_didSucceed)
            {
                return EClientConnectionState.Identification;
            }
            else if (_didFailed)
            {
                return EClientConnectionState.Disconnected;
            }
            return ID;
        }

        public void Exit(EClientConnectionState a_targetStateId)
        {
            FFLog.Log(EDbgCat.ClientIdentification, "Exit compatibility check");
            Engine.Receiver.UnregisterReceiver(EMessageChannel.VersionCompatibility.ToString(),
                                            _versionCompatibilityReceiver);
        }
        #endregion

        protected void OnVersionCompatibilityReceived(ReadRequest a_request)
        {
            if (a_request.Client == _client)
            {
                FFLog.Log(EDbgCat.ClientIdentification, "Compatibility check received for client : " + _client.NetworkID);
                MessageVersionData data = a_request.Data as MessageVersionData;
                if (Engine.Network.GameServer.IsReconnectionOnlyMode && Engine.Network.CurrentRoom.DcedPlayers.Count == 0)
                {
                    a_request.FailWithResponse(ERequestErrorCode.Canceled, new MessageEmptyData());
                    _didFailed = true;
                }
                else if (data.Data.Equals(Engine.Network.NetworkVersion))
                {
                    a_request.Success(data);
                    _didSucceed = true;
                    if(_client.onVersionCompatibilityVerificationSuccess != null)
                        _client.onVersionCompatibilityVerificationSuccess(_client);
                }
                else
                {
                    a_request.FailWithResponse(ERequestErrorCode.Failed, data);
                    _didFailed = false;
                }
            }
        }
    }
}