using UnityEngine;
using System.Collections;

using FF.UI;
using FF.Networking;

namespace FF
{
	internal class MenuRoomClientState : ANavigationMenuState
	{
		#region Inspector Properties
		protected FFMenuRoomPanel _roomPanel = null;
        #endregion

        #region Properties
        protected int _slotOptionPopupId = -1;
        protected int _connectionLostPopupId = -1;
        protected int _swapPopupId;
        #endregion

        #region States Methods
        internal override int ID
		{
			get {
				return (int)EMenuStateID.GameRoomClient;
			}
		}
		
		internal override void Enter ()
		{
			base.Enter ();
			FFLog.Log (EDbgCat.Logic, "Game Room Host state enter.");

            _slotOptionPopupId = -1;
            _connectionLostPopupId = -1;
            _swapPopupId = -1;

            _roomPanel = FFEngine.UI.GetPanel("MenuRoomPanel") as FFMenuRoomPanel;
			_roomPanel.UpdateWithRoom(FFEngine.Network.CurrentRoom);
			_navigationPanel.SetTitle ("Game Lobby");

            OnLanStatusChanged(FFEngine.NetworkStatus.IsConnectedToLan);
        }
		
		internal override void Exit ()
		{
			base.Exit ();
            if (_connectionLostPopupId != -1)
                FFEngine.UI.DismissPopup(_connectionLostPopupId);
            if(_swapPopupId != -1)
                FFEngine.UI.DismissPopup(_swapPopupId);
            if(_slotOptionPopupId != -1)
                FFEngine.UI.DismissPopup(_slotOptionPopupId);
        }
		#endregion
		
		#region Events
		protected override void RegisterForEvent ()
		{
			base.RegisterForEvent ();
			FFEngine.Events.RegisterForEvent("SlotSelected", OnSlotSelected);

            FFEngine.NetworkStatus.onLanStatusChanged += OnLanStatusChanged;

            FFEngine.Network.CurrentRoom.onRoomUpdated += OnRoomUpdate;
            FFEngine.Network.MainClient.onConnectionSuccess += OnReconnection;
            FFEngine.Network.MainClient.onConnectionLost += OnConnectionLost;
            FFEngine.Network.MainClient.onConnectionEnded += OnConnectionEnded;

            FFMessageRemovedFromRoom.onKickReceived += OnKickReceived;
            FFMessageRemovedFromRoom.onBanReceived += OnBanReceived;

            FFEngine.Inputs.PushOnBackCallback(QuitRoom);
        }
		
		protected override void UnregisterForEvent ()
		{
			base.UnregisterForEvent ();
            FFEngine.Events.UnregisterForEvent("SlotSelected", OnSlotSelected);

            FFEngine.NetworkStatus.onLanStatusChanged -= OnLanStatusChanged;

            if (FFEngine.Network.MainClient != null)
            {
                FFEngine.Network.CurrentRoom.onRoomUpdated -= OnRoomUpdate;
                FFEngine.Network.MainClient.onConnectionSuccess -= OnReconnection;
                FFEngine.Network.MainClient.onConnectionLost -= OnConnectionLost;
                FFEngine.Network.MainClient.onConnectionEnded -= OnConnectionEnded;
            }

            FFMessageRemovedFromRoom.onKickReceived -= OnKickReceived;
            FFMessageRemovedFromRoom.onBanReceived -= OnBanReceived;

            FFEngine.Inputs.PopOnBackCallback();
        }
        #endregion

        #region ConnectionLost
        protected void OnLanStatusChanged(bool a_state)
        {
            if (a_state)
            {
                _navigationPanel.HideWifiWarning();
            }
            else
            {
                _navigationPanel.ShowWifiWarning();
            }
        }

        protected void OnRoomUpdate(FFRoom a_room)
        {
            _roomPanel.UpdateWithRoom(a_room);
        }
        #endregion

        #region Client Callbacks
        protected void OnConnectionLost(FFTcpClient a_client)
        {
            _connectionLostPopupId = FFConnectionLostPopup.RequestDisplay(OnConnectionLostPopupCancel);
        }

        protected void OnReconnection(FFTcpClient a_client)
        {
            FFEngine.UI.DismissPopup(_connectionLostPopupId);
            _connectionLostPopupId = -1;
        }

        protected void OnConnectionEnded(FFTcpClient a_client, string a_reason)
        {
            FFEngine.Network.SetNoMainClient();
            FFMessagePopup.RequestDisplay("Connection ended : " + a_reason, "Ok", null);
            GoBack();
        }

        protected void OnConnectionLostPopupCancel()
        {
            KillClient();
            FFEngine.UI.DismissPopup(_connectionLostPopupId);
            _connectionLostPopupId = -1;
            GoBack();
        }
        #endregion

        #region UI Events
        internal void OnSlotSelected(FFEventParameter a_args)
		{
			FFSlotRef selectedSlot = (FFSlotRef)a_args.data;
            FFNetworkPlayer player = FFEngine.Network.CurrentRoom.GetPlayerForSlot(selectedSlot);
            if (player != FFEngine.Network.Player)
            {
                if (player != null)
                {
                    _slotOptionPopupId = FFClientSlotOptionPopup.RequestDisplay(player, OnPlayerOptionSwap, null);
                }
                else
                {
                    new FFMoveToSlotHandler(FFEngine.Network.MainClient, selectedSlot, OnMoveToSlotSuccess, OnMoveToSlotFailed);
                }
            }
        }
        #endregion

        #region Move To Slot Callbacks
        protected void OnMoveToSlotSuccess()
        {
            FFLog.Log(EDbgCat.Logic, "Move to slot success");
            //FFMessageToast.RequestDisplay("Move to slot success");
        }

        protected void OnMoveToSlotFailed(int a_errorCode)
        {
            string message = FFMoveToSlotRequest.MessageForCode(a_errorCode);
            FFMessageToast.RequestDisplay("Move to slot failed : " + message);
            //FFLog.Log(EDbgCat.Logic, "Move to slot failed : " + message);
        }
        #endregion

        #region Slot Options Callbacks
        internal void OnPlayerOptionKick(FFNetworkPlayer a_player)
        {
        }

        internal void OnPlayerOptionBan(FFNetworkPlayer a_player)
        {
        }

        internal void OnKickReceived()
        {
            KillClient();
            FFMessagePopup.RequestDisplay("Kicked by server.", "Sorry", null);
            GoBack();
        }

        internal void OnBanReceived()
        {
            KillClient();
            FFMessagePopup.RequestDisplay("Banned by server.", "Sorry", null);
            GoBack();
        }
        #endregion

        #region Swap
        FFSlotSwapHandler _swapHandler;
        internal void OnPlayerOptionSwap(FFNetworkPlayer a_player)
        {
            FFEngine.UI.DismissPopup(_slotOptionPopupId);
            _slotOptionPopupId = -1;
            _swapPopupId = FFLoadingPopup.RequestDisplay("Waiting for " + a_player.player.username + " to respond.", "Cancel", OnSwapCanceled);
            _swapHandler = new FFSlotSwapHandler(FFEngine.Network.MainClient, a_player.SlotRef, OnSwapSuccess, OnSwapFailed);
        }

        protected void OnSwapSuccess()
        {
            FFEngine.UI.DismissPopup(_swapPopupId);
            _swapPopupId = -1;
            FFMessageToast.RequestDisplay("Swap success");
        }

        protected void OnSwapFailed(int a_errorCode)
        {
            string message = FFSlotSwapRequest.MessageForCode(a_errorCode);
            FFEngine.UI.DismissPopup(_swapPopupId);
            _swapPopupId = -1;
            FFMessageToast.RequestDisplay("Swap failed : " + message);
        }

        protected void OnSwapCanceled()
        {
            FFEngine.UI.DismissPopup(_swapPopupId);
            _swapPopupId = -1;
            _swapHandler.Cancel();
        }
        #endregion

        #region Quit
        FFMessageLeavingRoom _leavingMessage = null;
        protected void QuitRoom()
        {
            _leavingMessage = new FFMessageLeavingRoom();
            _leavingMessage.onMessageSent += OnLeavingRoomSent;
            FFEngine.Network.MainClient.QueueFinalMessage(_leavingMessage);
        }

        protected void OnLeavingRoomSent()
        {
            KillClient();
            _leavingMessage.onMessageSent -= OnLeavingRoomSent;
            GoBack();
        }
        #endregion

        #region Stop
        protected void KillClient()
        {
            FFEngine.Network.CloseMainClient();
        }
        #endregion

        #region focus
        internal override void OnGetFocus()
        {
            base.OnGetFocus();
            if (FFEngine.Inputs.ShouldUseNavigation)
            {
                _roomPanel.TrySelectWidget();
            }
        }
        #endregion
    }
}