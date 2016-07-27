using UnityEngine;
using System.Collections;

using FF.UI;
using FF.Network;
using FF.Network.Message;
using FF.Multiplayer;
using FF.Network.Receiver;

using System.Net;

namespace FF
{
	internal class MenuDirectConnectState : ANavigationMenuState
	{
        #region Inspector Properties
        public float maxConnectTime = 15f;
        #endregion

        #region Properties
        internal IPEndPoint targetEndpoint = null;
        protected int _popupId;

        protected FFClientWrapper _client;

        protected GenericMessageReceiver _receiver = null;
        #endregion

        #region State Methods
        internal override int ID
        {
            get
            {
                return (int)EMenuStateID.DirectConnect;
            }
        }

        internal override void Enter()
        {
            base.Enter();
            FFLog.Log(EDbgCat.Logic, "Direct Connect State enter.");

            _client = Engine.Network.DirectConnect(targetEndpoint);
            
            _popupId = FFLoadingPopup.RequestDisplay("Connecting to server : " + targetEndpoint.ToString(), "Cancel", OnCancelPressed, true);

            _client.onConnectionSuccess += OnConnectionSuccess;
            _client.onConnectionFailed += OnConnectionFailed;
            _client.onVersionCheckFail += OnVersionCheckFailed;
            _client.onIdCheckFailed += OnNetworkIdFailed;
            _client.onConnectionEnded += OnConnectionEnded;

            _receiver = new GenericMessageReceiver(OnRoomInfoReceived);
            Engine.Receiver.RegisterReceiver(EMessageChannel.RoomInfos.ToString(), _receiver);
        }

        internal override void Exit()
        {
            Engine.UI.DismissPopup(_popupId);
            base.Exit();

            Engine.Receiver.UnregisterReceiver(EMessageChannel.RoomInfos.ToString(), _receiver);
        }
        #endregion

        protected void OnCancelPressed()
        {
            Engine.UI.DismissPopup(_popupId);
            Engine.Network.LeaveCurrentRoom();
            GoBack();
        }

        protected void OnRoomInfoReceived(ReadMessage a_message)
        {
            if (a_message.Data.Type == EDataType.Room)
            {
                MessageRoomData data = a_message.Data as MessageRoomData;
                data.Room.serverEndPoint = a_message.Client.Remote;

                Engine.Network.DirectConnectSetRoom(data.Room);
                Engine.Receiver.UnregisterReceiver(EMessageChannel.RoomInfos.ToString(), _receiver);
            }
        }

        #region Register 
        protected override void RegisterForEvent()
        {
            base.RegisterForEvent();
            if(Engine.Network.MainClient != null)
                Engine.Network.MainClient.onConnectionEnded += OnConnectionEnded;
        }

        protected override void UnregisterForEvent()
        {
            base.UnregisterForEvent();
            if (Engine.Network.MainClient != null)
                Engine.Network.MainClient.onConnectionEnded -= OnConnectionEnded;
        }
        #endregion

        #region Connection
        protected void OnConnectionSuccess(FFNetworkClient a_client)
        {
            FFLog.Log(EDbgCat.Logic, "Connection success.");
            _client.onConnectionSuccess -= OnConnectionSuccess;

            Engine.UI.DismissPopup(_popupId);
            _popupId = FFLoadingPopup.RequestDisplay("Checking Version", "Cancel", OnCancelPressed, true);

            _client.onVersionCompatibilityVerificationSuccess += OnVersionCheckSuccess;
        }

        protected void OnConnectionFailed(FFNetworkClient a_client, int a_attemptCount)
        {
            if (_timeElapsedSinceEnter > maxConnectTime)
            {
                FFMessageToast.RequestDisplay("Couldn't connect to server.");
                Engine.Network.LeaveCurrentRoom();
                GoBack();
            }
        }
        #endregion

        #region Identification
        #region Version Check
        protected void OnVersionCheckSuccess(FFNetworkClient a_client)
        {
            FFLog.Log(EDbgCat.Logic, "Version check success.");
            _client.onVersionCompatibilityVerificationSuccess -= OnVersionCheckSuccess;

            Engine.UI.DismissPopup(_popupId);
            _popupId = FFLoadingPopup.RequestDisplay("Identifying", "Cancel", OnCancelPressed, true);

            _client.onIdCheckCompleted += OnNetworkIdCheckSuccess;
        }

        protected void OnVersionCheckFailed(FFNetworkClient a_client, FFVersion a_serverVersion, FFVersion a_playerVersion)
        {
            if (a_serverVersion != null)
            {
                FFMessageToast.RequestDisplay("Version check Server : " + a_serverVersion.ToString() + " / Player : " + a_playerVersion.ToString());
            }
            else
            {
                FFMessageToast.RequestDisplay("Server declined.");
            }
            Engine.Network.LeaveCurrentRoom();
            GoBack();
        }
        #endregion

        #region NetworkId
        protected void OnNetworkIdCheckSuccess(FFNetworkClient a_client, int a_serverId, int a_playerId)
        {
            FFLog.Log(EDbgCat.Logic, "Network Id check success.");
            _client.onIdCheckCompleted -= OnNetworkIdCheckSuccess;

            Engine.UI.DismissPopup(_popupId);
            _popupId = FFLoadingPopup.RequestDisplay("Joining Room", "Cancel", OnCancelPressed, true);

            FFNetworkPlayer player = new FFNetworkPlayer(-1, Engine.Game.Player);
            MessagePlayerData data = new MessagePlayerData(player);
            SentRequest request = new SentRequest(data,
                                                  EMessageChannel.JoinRoom.ToString(),
                                                  Engine.Network.NextRequestId,
                                                  3f);
            request.onSucces += OnJoinSuccess;
            request.onFail += OnJoinFail;
            Engine.Network.MainClient.QueueRequest(request);
        }

        protected void OnNetworkIdFailed(FFNetworkClient a_client, int a_serverId, int a_playerId)
        {
            if (a_serverId != -1)
            {
                FFMessageToast.RequestDisplay("Network Id Server : " + a_serverId.ToString() + " / Player : " + a_playerId.ToString());
            }
            else
            {
                FFMessageToast.RequestDisplay("Server declined.");
            }
            Engine.Network.LeaveCurrentRoom();
            GoBack();
        }
        #endregion
        #endregion

        #region Joining
        protected void OnJoinFail(ERequestErrorCode a_errorCode, ReadResponse a_response)
        {
            string message = "";

            if (a_errorCode == ERequestErrorCode.Failed)
            {
                if (a_response.Data.Type == EDataType.Integer)
                {
                    MessageIntegerData data = a_response.Data as MessageIntegerData;

                    EErrorCodeJoinRoom errorCode = (EErrorCodeJoinRoom)data.Data;
                    //TODO
                    message = errorCode.ToString();
                }
            }
            else
            {
                message = a_errorCode.ToString();
            }

            FFMessageToast.RequestDisplay("Couldn't join room : " + message);
            Engine.Network.LeaveCurrentRoom();
            GoBack();
        }

        protected void OnJoinSuccess(ReadResponse a_response)
        {
            FFLog.Log(EDbgCat.Logic, "Join Room Success");
            RequestState(outState.ID);
        }

        #endregion

        protected void OnConnectionEnded(FFNetworkClient a_client)
        {
            Engine.Network.LeaveCurrentRoom();
            GoBack();
        }
    }
}