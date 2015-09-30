using UnityEngine;
using System.Collections;

using FF.UI;
using FF.Networking;

namespace FF
{
	internal class MenuConnectionState : ANavigationMenuState
	{
        #region Properties
        protected FFJoinRoomRequest _request;
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
            _request = new FFJoinRoomRequest(player);
            FFEngine.Network.MainClient.QueueMessage(_request);
            if (_request != null)
            {
                _request.onTimeout += OnTimeout;
                _request.onDeny += OnDeny;
                _request.onSuccess += OnSuccess;
            }

            FFLoadingPopup.RequestDisplay("Joining " + FFEngine.Network.CurrentRoom.roomName ,"Cancel", OnCancel);
        }

        internal override void Exit()
        {
            base.Exit();
            if (_request != null)
            {
                _request.onTimeout -= OnTimeout;
                _request.onDeny -= OnDeny;
                _request.onSuccess -= OnSuccess;
            }
        }
        #endregion

        #region Events
        protected override void RegisterForEvent ()
		{
			base.RegisterForEvent ();
			//FFEngine.NetworkStatus.onLanStatusChanged += OnLanStatusChanged;
        }
		
		protected override void UnregisterForEvent ()
		{
			base.UnregisterForEvent ();
			//FFEngine.NetworkStatus.onLanStatusChanged -= OnLanStatusChanged;
        }

        /*protected void OnLanStatusChanged(bool a_state)
        {
            if (a_state)
            {
                _navigationPanel.HideWifiWarning();
            }
            else
            {
                _navigationPanel.ShowWifiWarning();
            }
        }*/

        protected void OnTimeout()
        {
            FFEngine.UI.DismissCurrentPopup();
            FFMessagePopup.RequestDisplay("Couldn't join room : Timedout", "Close", null);
            FFEngine.Network.SetNoMainClient();
            FFEngine.Events.FireEvent(EEventType.Back);
        }

        protected void OnDeny(string a_message)
        {
            FFEngine.UI.DismissCurrentPopup();
            FFMessagePopup.RequestDisplay("Couldn't join room : " + a_message, "Close", null);
            FFEngine.Network.SetNoMainClient();
            FFEngine.Events.FireEvent(EEventType.Back);
        }

        protected void OnSuccess()
        {
            FFEngine.UI.DismissCurrentPopup();
            RequestState(outState.ID);
        }

        //User Canceled
        protected void OnCancel()
        {
            FFEngine.UI.DismissCurrentPopup();
            _request.Cancel();
            FFEngine.Network.SetNoMainClient();
            FFEngine.Events.FireEvent(EEventType.Back);
        }
        #endregion
    }
}