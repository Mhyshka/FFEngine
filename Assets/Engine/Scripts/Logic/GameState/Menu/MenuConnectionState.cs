using UnityEngine;
using System.Collections;

using FF.UI;
using FF.Network;
using FF.Network.Message;
using FF.Multiplayer;

namespace FF
{
	internal class MenuConnectionState : ANavigationMenuState
	{
        #region Properties
        protected int _popupId;
        #endregion

        #region State Methods
        internal override int ID
        {
            get
            {
                return (int)EMenuStateID.Connecting;
            }
        }

        internal override void Enter()
        {
            base.Enter();
            FFLog.Log(EDbgCat.Logic, "Waiting State enter.");

            FFNetworkPlayer player = Engine.Game.NetPlayer;

            MessagePlayerData data = new MessagePlayerData(Engine.Game.NetPlayer);
            SentRequest request = new SentRequest(data,
                                                  EMessageChannel.JoinRoom.ToString(),
                                                  Engine.Network.NextRequestId,
                                                  3f);
            request.onSucces += OnSuccess;
            request.onFail += OnFail;

            Engine.Network.MainClient.QueueRequest(request);

            _popupId = FFLoadingPopup.RequestDisplay("Joining " + Engine.Game.CurrentRoom.roomName, "Cancel", null, false);
        }

        internal override void Exit()
        {
            Engine.UI.DismissPopup(_popupId);
            base.Exit();
        }
        #endregion

        #region Register 
        protected override void RegisterForEvent()
        {
            base.RegisterForEvent();
        }

        protected override void UnregisterForEvent()
        {
            base.UnregisterForEvent();
        }
        #endregion

        #region Events
        protected void OnFail(ERequestErrorCode a_errorCode, ReadResponse a_response)
        {
            string message = "";

            if (a_response.Data.Type == EDataType.Integer)
            {
                MessageIntegerData data = a_response.Data as MessageIntegerData;

                EErrorCodeJoinRoom errorCode = (EErrorCodeJoinRoom)data.Data;
                //TODO
                message = errorCode.ToString();
            }

            FFMessageToast.RequestDisplay("Couldn't join room : " + message);
            Engine.Network.SetNoMainClient();
            GoBack();
        }

        protected void OnSuccess(ReadResponse a_response)
        {
            Engine.Network.StopLookingForGames();
            RequestState(outState.ID);
        }

        //User Canceled
        protected void OnCancel()
        {
            /*
            _request.Cancel();
            FFEngine.Network.CloseMainClient();
            GoBack();*/
        }
        #endregion
    }
}