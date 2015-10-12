using UnityEngine;
using System.Collections;

using FF.UI;
using FF.Networking;

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

            FFNetworkPlayer player = FFEngine.Network.Player;

            new FFJoinRoomHandler(FFEngine.Network.MainClient, player, OnSuccess, OnFail);

            _popupId = FFLoadingPopup.RequestDisplay("Joining " + FFEngine.Network.CurrentRoom.roomName, "Cancel", null, false);
        }

        internal override void Exit()
        {
            FFEngine.UI.DismissPopup(_popupId);
            base.Exit();
        }
        #endregion

        #region Events
        protected void OnFail(int a_errorCode)
        {
            string message = FFJoinRoomRequest.MessageForCode(a_errorCode);
            FFMessageToast.RequestDisplay("Couldn't join room : " + message);
            FFEngine.Network.SetNoMainClient();
            GoBack();
        }

        protected void OnSuccess()
        {
            FFEngine.Network.StopLookingForGames();
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