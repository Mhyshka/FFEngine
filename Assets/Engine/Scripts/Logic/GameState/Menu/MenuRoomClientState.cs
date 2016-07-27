using UnityEngine;
using System.Collections;

using FF.UI;
using FF.Network;
using FF.Network.Message;
using FF.Network.Receiver;
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
        protected RemovedFromRoomReceiver _removedFromRoomReceiver;
        protected InstanceConfirmSwapReceiver _confirmSwapReceiver;
        protected GenericMessageReceiver _startGameReceiver;
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
			_roomPanel.UpdateWithRoom(Engine.Network.CurrentRoom);
            _roomPanel.SetRoomEndpoint(Engine.Network.CurrentRoom.serverEndPoint);
            _navigationPanel.SetTitle ("Game Lobby");

            OnLanStatusChanged(Engine.NetworkStatus.IsConnectedToLan);

            //Engine.Inputs.EnableClientMode();
            //Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        private void InitReceivers()
        {
            if (_removedFromRoomReceiver == null)
                _removedFromRoomReceiver = new RemovedFromRoomReceiver(OnKickReceived, OnBanReceived);
            if (_confirmSwapReceiver == null)
                _confirmSwapReceiver = new InstanceConfirmSwapReceiver();
            if (_startGameReceiver == null)
                _startGameReceiver = new GenericMessageReceiver(OnRequestGameModeReceived);
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
        }
        #endregion

        #region Events
        protected override void RegisterForEvent ()
		{
			base.RegisterForEvent ();
			Engine.Events.RegisterForEvent("SlotSelected", OnSlotSelected);

            Engine.NetworkStatus.onLanStatusChanged += OnLanStatusChanged;

            Engine.Network.CurrentRoom.onRoomUpdated += OnRoomUpdate;
            Engine.Network.MainClient.onReconnection += OnReconnection;
            Engine.Network.MainClient.onConnectionLost += OnConnectionLost;
            Engine.Network.MainClient.onConnectionEnded += OnConnectionEnded;
            Engine.Network.MainClient.Clock.onLatencyUpdate += OnLatencyUpdate;

            Engine.Receiver.RegisterReceiver(EMessageChannel.RemovedFromRoom.ToString(), _removedFromRoomReceiver);
            Engine.Receiver.RegisterReceiver(EMessageChannel.SwapConfirm.ToString(), _confirmSwapReceiver);
            Engine.Receiver.RegisterReceiver(EMessageChannel.StartGame.ToString(), _startGameReceiver);

            Engine.Inputs.PushOnBackCallback(QuitRoom);
        }

        protected void OnLatencyUpdate(FFNetworkClient a_client, float a_latency)
        {
            EntryPoint.s_latency = a_latency;
        }

		protected override void UnregisterForEvent ()
		{
			base.UnregisterForEvent ();
            Engine.Events.UnregisterForEvent("SlotSelected", OnSlotSelected);

            Engine.NetworkStatus.onLanStatusChanged -= OnLanStatusChanged;

            if (Engine.Network.CurrentRoom != null)
            {
                Engine.Network.CurrentRoom.onRoomUpdated -= OnRoomUpdate;
            }

            if (Engine.Network.MainClient != null)
            {
                Engine.Network.MainClient.onReconnection -= OnReconnection;
                Engine.Network.MainClient.onConnectionLost -= OnConnectionLost;
                Engine.Network.MainClient.onConnectionEnded -= OnConnectionEnded;
            }
        
            Engine.Receiver.UnregisterReceiver(EMessageChannel.RemovedFromRoom.ToString(), _removedFromRoomReceiver);
            Engine.Receiver.UnregisterReceiver(EMessageChannel.SwapConfirm.ToString(), _confirmSwapReceiver);
            Engine.Receiver.UnregisterReceiver(EMessageChannel.StartGame.ToString(), _startGameReceiver);

            Engine.Inputs.PopOnBackCallback();
        }
        #endregion

        #region ConnectionLost
        protected void OnLanStatusChanged(bool a_state)
        {
            if (a_state)
            {
                Engine.UI.HideSpecificPanel("WifiWarningPanel");
            }
            else
            {
                Engine.UI.RequestDisplay("WifiWarningPanel");
            }
        }

        protected void OnRoomUpdate(Room a_room)
        {
            _roomPanel.UpdateWithRoom(a_room);
        }
        #endregion

        #region Client Callbacks
        protected void OnConnectionLost(FFNetworkClient a_client)
        {
            if (_connectionLostPopupId == -1)
            {
                _connectionLostPopupId = FFConnectionLostPopup.RequestDisplay(OnConnectionLostPopupCancel, UIManager.POPUP_PRIO_HIGH);
            }
        }

        protected void OnReconnection()
        {
            Engine.UI.DismissPopup(_connectionLostPopupId);
            _connectionLostPopupId = -1;
            Engine.UI.HideSpecificPanel("WifiWarningPanel");
        }

        protected void OnConnectionEnded(FFNetworkClient a_client)
        {
            Engine.Network.LeaveCurrentRoom();
            FFMessagePopup.RequestDisplay("Connection ended.", "Ok", null);
            GoBack();
        }

        protected void OnConnectionLostPopupCancel()
        {
            Engine.UI.DismissPopup(_connectionLostPopupId);
            Engine.Network.LeaveCurrentRoom();
            _connectionLostPopupId = -1;
            GoBack();
        }
        #endregion

        #region UI Events
        internal void OnSlotSelected(FFEventParameter a_args)
		{
			SlotRef selectedSlot = (SlotRef)a_args.data;
            FFNetworkPlayer player = Engine.Network.CurrentRoom.GetPlayerForSlot(selectedSlot);
            if (player != null)
            {
                if(player.ID != Engine.Network.NetworkId)
                    _slotOptionPopupId = FFClientSlotOptionPopup.RequestDisplay(player, OnPlayerOptionSwap, null);
            }
            else
            {
                MessageSlotRefData data = new MessageSlotRefData(selectedSlot);
                
                SentRequest request = new SentRequest(data,
                                                        EMessageChannel.MoveToSlot.ToString(),
                                                        Engine.Network.NextRequestId);
                request.onSucces += OnMoveToSlotSuccess;
                request.onFail += OnMoveToSlotFailed;
                Engine.Network.MainClient.QueueRequest(request);
            }
        }
        #endregion

        #region Move To Slot Callbacks
        protected void OnMoveToSlotSuccess(ReadResponse a_message)
        {
            FFLog.Log(EDbgCat.Logic, "Move to slot success");
            //FFMessageToast.RequestDisplay("Move to slot success");
        }

        protected void OnMoveToSlotFailed(ERequestErrorCode a_errorCode, ReadResponse a_response)
        {
            string message = "";

            if (a_errorCode == ERequestErrorCode.Failed)
            {
                if (a_response.Data.Type == EDataType.Integer)
                {
                    MessageIntegerData data = a_response.Data as MessageIntegerData;

                    EErrorCodeMoveToSlot detailErrorCode = (EErrorCodeMoveToSlot)data.Data;

                    //TODO display errorMessage
                    message = detailErrorCode.ToString();
                }
            }
            else
            {
                message = a_errorCode.ToString();
            }
           
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
            Engine.Network.LeaveCurrentRoom();
            FFMessagePopup.RequestDisplay("Kicked by server.", "Sorry", null);
            GoBack();
        }

        internal void OnBanReceived()
        {
            Engine.Network.LeaveCurrentRoom();
            FFMessagePopup.RequestDisplay("Banned by server.", "Sorry", null);
            GoBack();
        }
        #endregion

        #region Swap
        SentRequest _swapRequest;
        internal void OnPlayerOptionSwap(FFNetworkPlayer a_player)
        {
            Engine.UI.DismissPopup(_slotOptionPopupId);
            _slotOptionPopupId = -1;
            _swapPopupId = FFLoadingPopup.RequestDisplay("Waiting for " + a_player.player.username + " to respond.", "Cancel", OnSwapCanceled);
            MessageSlotRefData data = new MessageSlotRefData(a_player.SlotRef);
            _swapRequest = new SentRequest(data,
                                            EMessageChannel.SwapSlot.ToString(),
                                            Engine.Network.NextRequestId,
                                            float.MaxValue);
            _swapRequest.onSucces += OnSwapSuccess;
            _swapRequest.onFail += OnSwapFailed;
            Engine.Network.MainClient.QueueRequest(_swapRequest);
        }

        protected void OnSwapSuccess(ReadResponse a_response)
        {
            Engine.UI.DismissPopup(_swapPopupId);
            _swapPopupId = -1;
            FFMessageToast.RequestDisplay("Swap success");
        }

        protected void OnSwapFailed(ERequestErrorCode a_errorCode, ReadResponse a_response)
        {
            string message = "";

            if (a_errorCode == ERequestErrorCode.Failed)
            {
                if (a_response.Data.Type == EDataType.Integer)
                {
                    MessageIntegerData data = a_response.Data as MessageIntegerData;

                    EErrorCodeSwapSlot detailErrorCode = (EErrorCodeSwapSlot)data.Data;

                    //TODO display errorMessage
                    message = detailErrorCode.ToString();
                }
            }
            else
            {
                message = a_errorCode.ToString();
            }
        
            Engine.UI.DismissPopup(_swapPopupId);
            _swapPopupId = -1;
            FFMessageToast.RequestDisplay("Swap failed : " + message);
        }

        protected void OnSwapCanceled()
        {
            Engine.UI.DismissPopup(_swapPopupId);
            _swapPopupId = -1;
            _swapRequest.Cancel(true);
        }
        #endregion

        #region Quit
        SentMessage _leavingMessage = null;
        protected void QuitRoom()
        {
            if (Engine.Network.MainClient != null)
            {
                _leavingMessage = new SentMessage(new MessageEmptyData(),
                                                EMessageChannel.LeavingRoom.ToString());
                _leavingMessage.onMessageSent += OnLeavingRoomSent;

                Engine.Network.MainClient.QueueFinalMessage(_leavingMessage);
            }
        }

        protected void OnLeavingRoomSent()
        {
            Engine.Network.LeaveCurrentRoom();
            _leavingMessage.onMessageSent -= OnLeavingRoomSent;
            GoBack();
        }
        #endregion

        #region LoadGame
        internal void OnRequestGameModeReceived(ReadMessage a_message)
        {
            RequestMultiGameMode("PongClientGameMode");
        }
        #endregion

        #region focus
        #endregion
    }
}