using UnityEngine;
using System.Collections;

using FF.UI;
using FF.Network;
using FF.Network.Message;
using FF.Multiplayer;

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

        #region Properties Receiver
        protected Network.Receiver.MessageRemovedFromRoom _removedFromRoomReceiver;
        protected Network.Receiver.RequestSlotSwap _slotSwapReceiver;
        protected Network.Receiver.RequestConfirmSwap _confirmSwapReceiver;
        protected Network.Receiver.MessageRequestGameMode _loadGameReceiver;
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
            InitReceivers();
            FFLog.Log (EDbgCat.Logic, "Game Room Host state enter.");
            _slotOptionPopupId = -1;
            _connectionLostPopupId = -1;
            _swapPopupId = -1;

            _roomPanel = Engine.UI.GetPanel("MenuRoomPanel") as FFMenuRoomPanel;
            _roomPanel.TrySelectWidget();
			_roomPanel.UpdateWithRoom(Engine.Game.CurrentRoom);
			_navigationPanel.SetTitle ("Game Lobby");

            OnLanStatusChanged(Engine.NetworkStatus.IsConnectedToLan);

            Engine.Inputs.EnableClientMode();
            //Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        private void InitReceivers()
        {
            if (_removedFromRoomReceiver == null)
                _removedFromRoomReceiver = new Network.Receiver.MessageRemovedFromRoom(OnKickReceived, OnBanReceived);
            if (_slotSwapReceiver == null)
                _slotSwapReceiver = new Network.Receiver.RequestSlotSwap();
            if (_confirmSwapReceiver == null)
                _confirmSwapReceiver = new Network.Receiver.RequestConfirmSwap();
            if (_loadGameReceiver == null)
                _loadGameReceiver = new Network.Receiver.MessageRequestGameMode(OnRequestGameModeReceived);
        }

        internal override void Exit ()
		{
			base.Exit ();
            if (_connectionLostPopupId != -1)
                Engine.UI.DismissPopup(_connectionLostPopupId);
            if(_swapPopupId != -1)
                Engine.UI.DismissPopup(_swapPopupId);
            if(_slotOptionPopupId != -1)
                Engine.UI.DismissPopup(_slotOptionPopupId);
        }

        internal override void GoBack()
        {
            base.GoBack();
            Engine.Inputs.DisableClientMode();
        }
        #endregion

        #region Events
        protected override void RegisterForEvent ()
		{
			base.RegisterForEvent ();
			Engine.Events.RegisterForEvent("SlotSelected", OnSlotSelected);

            Engine.NetworkStatus.onLanStatusChanged += OnLanStatusChanged;

            Engine.Game.CurrentRoom.onRoomUpdated += OnRoomUpdate;
            Engine.Network.MainClient.onConnectionSuccess += OnReconnection;
            Engine.Network.MainClient.onConnectionLost += OnConnectionLost;
            Engine.Network.MainClient.onConnectionEnded += OnConnectionEnded;

            Engine.Receiver.RegisterReceiver(EMessageType.RemovedFromRoom, _removedFromRoomReceiver);
            Engine.Receiver.RegisterReceiver(EMessageType.SlotSwapRequest, _slotSwapReceiver);
            Engine.Receiver.RegisterReceiver(EMessageType.ConfirmSwapRequest, _confirmSwapReceiver);
            Engine.Receiver.RegisterReceiver(EMessageType.RequestGameMode, _loadGameReceiver);

            Engine.Inputs.PushOnBackCallback(QuitRoom);
        }
		
		protected override void UnregisterForEvent ()
		{
			base.UnregisterForEvent ();
            Engine.Events.UnregisterForEvent("SlotSelected", OnSlotSelected);

            Engine.NetworkStatus.onLanStatusChanged -= OnLanStatusChanged;

            if (Engine.Network.MainClient != null)
            {
                Engine.Game.CurrentRoom.onRoomUpdated -= OnRoomUpdate;
                Engine.Network.MainClient.onConnectionSuccess -= OnReconnection;
                Engine.Network.MainClient.onConnectionLost -= OnConnectionLost;
                Engine.Network.MainClient.onConnectionEnded -= OnConnectionEnded;
            }
        
            Engine.Receiver.UnregisterReceiver(EMessageType.RemovedFromRoom, _removedFromRoomReceiver);
            Engine.Receiver.UnregisterReceiver(EMessageType.SlotSwapRequest, _slotSwapReceiver);
            Engine.Receiver.UnregisterReceiver(EMessageType.ConfirmSwapRequest, _confirmSwapReceiver);
            Engine.Receiver.UnregisterReceiver(EMessageType.RequestGameMode, _loadGameReceiver);

            Engine.Inputs.PopOnBackCallback();
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

        protected void OnRoomUpdate(Room a_room)
        {
            _roomPanel.UpdateWithRoom(a_room);
        }
        #endregion

        #region Client Callbacks
        protected void OnConnectionLost(FFTcpClient a_client)
        {
            _connectionLostPopupId = FFConnectionLostPopup.RequestDisplay(OnConnectionLostPopupCancel, UIManager.POPUP_PRIO_HIGH);
        }

        protected void OnReconnection(FFTcpClient a_client)
        {
            Engine.UI.DismissPopup(_connectionLostPopupId);
            _connectionLostPopupId = -1;
        }

        protected void OnConnectionEnded(FFTcpClient a_client, string a_reason)
        {
            Engine.Network.SetNoMainClient();
            FFMessagePopup.RequestDisplay("Connection ended : " + a_reason, "Ok", null);
            GoBack();
        }

        protected void OnConnectionLostPopupCancel()
        {
            KillClient();
            Engine.UI.DismissPopup(_connectionLostPopupId);
            _connectionLostPopupId = -1;
            GoBack();
        }
        #endregion

        #region UI Events
        internal void OnSlotSelected(FFEventParameter a_args)
		{
			SlotRef selectedSlot = (SlotRef)a_args.data;
            FFNetworkPlayer player = Engine.Game.CurrentRoom.GetPlayerForSlot(selectedSlot);
            if (player != null)
            {
                if(player.ID != Engine.Game.NetPlayer.ID)
                    _slotOptionPopupId = FFClientSlotOptionPopup.RequestDisplay(player, OnPlayerOptionSwap, null);
            }
            else
            {
                new Handler.MoveToSlot(Engine.Network.MainClient, selectedSlot, OnMoveToSlotSuccess, OnMoveToSlotFailed);
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
            string message = RequestMoveToSlot.MessageForCode(a_errorCode);
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
        Handler.SlotSwap _swapHandler;
        internal void OnPlayerOptionSwap(FFNetworkPlayer a_player)
        {
            Engine.UI.DismissPopup(_slotOptionPopupId);
            _slotOptionPopupId = -1;
            _swapPopupId = FFLoadingPopup.RequestDisplay("Waiting for " + a_player.player.username + " to respond.", "Cancel", OnSwapCanceled);
            _swapHandler = new Handler.SlotSwap(Engine.Network.MainClient, a_player.SlotRef, OnSwapSuccess, OnSwapFailed);
        }

        protected void OnSwapSuccess()
        {
            Engine.UI.DismissPopup(_swapPopupId);
            _swapPopupId = -1;
            FFMessageToast.RequestDisplay("Swap success");
        }

        protected void OnSwapFailed(int a_errorCode)
        {
            string message = RequestSlotSwap.MessageForCode(a_errorCode);
            Engine.UI.DismissPopup(_swapPopupId);
            _swapPopupId = -1;
            FFMessageToast.RequestDisplay("Swap failed : " + message);
        }

        protected void OnSwapCanceled()
        {
            Engine.UI.DismissPopup(_swapPopupId);
            _swapPopupId = -1;
            _swapHandler.Cancel();
        }
        #endregion

        #region Quit
        MessageLeavingRoom _leavingMessage = null;
        protected void QuitRoom()
        {
            _leavingMessage = new MessageLeavingRoom();
            _leavingMessage.onMessageSent += OnLeavingRoomSent;
            Engine.Network.MainClient.QueueFinalMessage(_leavingMessage);
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
            Engine.Network.CloseMainClient();
        }
        #endregion

        #region LoadGame
        internal void OnRequestGameModeReceived()
        {
            RequestMultiGameMode("PongClientGameMode");
        }
        #endregion

        #region focus
        #endregion
    }
}