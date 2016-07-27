using UnityEngine;
using System.Collections;

using FF.Network.Message;

namespace FF.Network
{
    internal class ServerIdentificationState : IFsmState<EClientConnectionState>
    {
        #region Properties
        FFNetworkClient _client;

        protected bool _didSucceed = false;
        protected bool _didFailed = false;

        #region Version Compatibility Properties
        Receiver.GenericMessageReceiver _networkIdReceiver = null;
        #endregion
        #endregion

        internal ServerIdentificationState(FFNetworkClient a_client)
        {
            _client = a_client;
            _networkIdReceiver = new Receiver.GenericMessageReceiver(null,
                                                                    OnNetworkIdReceived);
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
            Engine.Receiver.RegisterReceiver(EMessageChannel.NetworkId.ToString(),
                                            _networkIdReceiver);
            FFLog.Log(EDbgCat.ClientIdentification, "Enter identification state");
        }

        public EClientConnectionState DoUpdate()
        {
            if (_didSucceed)
            {
                return EClientConnectionState.Connected;
            }
            else if (_didFailed)
            {
                return EClientConnectionState.Disconnected;
            }
            return ID;
        }

        public void Exit(EClientConnectionState a_targetStateId)
        {
            Engine.Receiver.UnregisterReceiver(EMessageChannel.NetworkId.ToString(),
                                            _networkIdReceiver);

            FFLog.Log(EDbgCat.ClientIdentification, "Exit identification state");
        }
        #endregion

        protected void OnNetworkIdReceived(ReadRequest a_request)
        {
            FFLog.Log(EDbgCat.ClientIdentification, "Network ID Received");

            if (a_request.Client == _client)
            {
                if (Engine.Network.GameServer.IsReconnectionOnlyMode && Engine.Network.CurrentRoom.DcedPlayers.Count == 0)
                {
                    a_request.FailWithResponse(ERequestErrorCode.Canceled, new MessageEmptyData());
                    _didFailed = true;
                }
                else
                {
                    MessageIntegerData data = a_request.Data as MessageIntegerData;

                    _client.onIdCheckCompleted(_client, _client.NetworkID, data.Data);

                    MessageIntegerData result = new MessageIntegerData(_client.NetworkID);

                    _didSucceed = true;

                    a_request.Success(result);
                }
            }
        }
    }
}