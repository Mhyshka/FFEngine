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

            new Handler.JoinRoom(Engine.Network.MainClient, player, OnSuccess, OnFail);

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
        protected void OnFail(int a_errorCode)
        {
            string message = MessagePlayerData.MessageForCode(a_errorCode);
            FFMessageToast.RequestDisplay("Couldn't join room : " + message);
            Engine.Network.SetNoMainClient();
            GoBack();
        }

        protected void OnSuccess()
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