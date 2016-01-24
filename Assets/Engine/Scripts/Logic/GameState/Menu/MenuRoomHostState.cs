using UnityEngine;
using System.Collections;
using Zeroconf;
using System.Collections.Generic;

using System.Net;

using FF.UI;
using FF.Network;
using FF.Network.Message;
using FF.Multiplayer;

namespace FF
{
    internal class MenuRoomHostState : ANavigationMenuState
    {
        #region Inspector Properties
        protected FFMenuRoomPanel _roomPanel = null;
        #endregion

        #region Properties
        protected int _slotOptionPopupId = -1;
        protected int _swapPopupId;
        #endregion

        #region Properties Receiver
        protected Network.Receiver.RequestJoinRoom _joinRoomReceiver;
        protected Network.Receiver.RequestMoveToSlot _moveToSlotReceiver;
        protected Network.Receiver.RequestSlotSwap _slotSwapReceiver;
        protected Network.Receiver.RequestConfirmSwap _confirmSwapReceiver;
        #endregion

        #region States Methods
        internal override int ID {
            get {
                return (int)EMenuStateID.GameRoomHost;
            }
        }

        internal override void Enter()
        {
            base.Enter();
            InitNetwork();
            Engine.Network.StartBroadcastingGame(Engine.Game.Player.username + "'s game");

            FFLog.Log(EDbgCat.Logic, "Game Room Host state enter.");
            _slotOptionPopupId = -1;

            _roomPanel = Engine.UI.GetPanel("MenuRoomPanel") as FFMenuRoomPanel;

            _navigationPanel.SetTitle("Game Lobby");

            OnRoomUpdate(Engine.Game.CurrentRoom);
            Engine.Inputs.EnableServerMode();

            //Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        private void InitNetwork()
        {
            if (_joinRoomReceiver == null)
                _joinRoomReceiver = new Network.Receiver.RequestJoinRoom();
            if (_moveToSlotReceiver == null)
                _moveToSlotReceiver = new Network.Receiver.RequestMoveToSlot();
            if (_slotSwapReceiver == null)
                _slotSwapReceiver = new Network.Receiver.RequestSlotSwap();
            if (_confirmSwapReceiver == null)
                _confirmSwapReceiver = new Network.Receiver.RequestConfirmSwap();
        }

        internal override void Exit()
        {
            base.Exit();
            if (_slotOptionPopupId != -1)
                Engine.UI.DismissPopup(_slotOptionPopupId);
            if (_swapPopupId != -1)
                Engine.UI.DismissPopup(_swapPopupId);

            Engine.Network.StopBroadcastingGame();

            if(IsGoingBack)
                Engine.Network.StopServer();
        }

        internal override void GoBack()
        {
            new Handler.Farewell(OnShutdownComplete);
        }

        internal void OnShutdownComplete()
        {
            base.GoBack();
            Engine.Inputs.DisableServerMode();
        }
        #endregion

        #region Events
        protected override void RegisterForEvent()
        {
            base.RegisterForEvent();
            Engine.Events.RegisterForEvent(FFEventType.Next, OnNext);
            Engine.NetworkStatus.onLanStatusChanged += OnLanStatusChanged;

            Engine.Game.CurrentRoom.onRoomUpdated += OnRoomUpdate;
            Engine.Network.Server.onClientAdded += OnClientAdded;
            Engine.Network.Server.onClientReconnection += OnClientReconnected;

            RegisterReceiver();
        }

        private void RegisterReceiver()
        {
            Engine.Receiver.RegisterReceiver(EMessageType.JoinRoomRequest, _joinRoomReceiver);
            Engine.Receiver.RegisterReceiver(EMessageType.MoveToSlotRequest, _moveToSlotReceiver);
            Engine.Receiver.RegisterReceiver(EMessageType.SlotSwapRequest, _slotSwapReceiver);
            Engine.Receiver.RegisterReceiver(EMessageType.ConfirmSwapRequest, _confirmSwapReceiver);
            Engine.Events.RegisterForEvent("SlotSelected", OnSlotSelected);
        }

        private void RegisterForbiddenReceiver()
        {
            Engine.Receiver.RegisterReceiver(EMessageType.JoinRoomRequest, Engine.Receiver.RESPONSE_ALWAYS_FAIL);
            Engine.Receiver.RegisterReceiver(EMessageType.MoveToSlotRequest, Engine.Receiver.RESPONSE_ALWAYS_FAIL);
            Engine.Receiver.RegisterReceiver(EMessageType.SlotSwapRequest, Engine.Receiver.RESPONSE_ALWAYS_FAIL);
            Engine.Receiver.RegisterReceiver(EMessageType.ConfirmSwapRequest, Engine.Receiver.RESPONSE_ALWAYS_FAIL);
        }

        protected override void UnregisterForEvent()
        {
            base.UnregisterForEvent();
            Engine.Events.UnregisterForEvent(FFEventType.Next, OnNext);
            Engine.Events.UnregisterForEvent("SlotSelected", OnSlotSelected);
            Engine.NetworkStatus.onLanStatusChanged -= OnLanStatusChanged;

            if (Engine.Network.Server != null)
            {
                Engine.Game.CurrentRoom.onRoomUpdated -= OnRoomUpdate;
                Engine.Network.Server.onClientAdded -= OnClientAdded;
                Engine.Network.Server.onClientReconnection -= OnClientReconnected;
            }

            UnregisterReceiver();
        }

        private void UnregisterReceiver()
        {
            Engine.Receiver.UnregisterReceiver(EMessageType.JoinRoomRequest, _joinRoomReceiver);
            Engine.Receiver.UnregisterReceiver(EMessageType.MoveToSlotRequest, _moveToSlotReceiver);
            Engine.Receiver.UnregisterReceiver(EMessageType.SlotSwapRequest, _slotSwapReceiver);
            Engine.Receiver.UnregisterReceiver(EMessageType.ConfirmSwapRequest, _confirmSwapReceiver);
            Engine.Events.UnregisterForEvent("SlotSelected", OnSlotSelected);
        }

        private void UnregisterForbiddenReceiver()
        {
            Engine.Receiver.UnregisterReceiver(EMessageType.JoinRoomRequest, Engine.Receiver.RESPONSE_ALWAYS_FAIL);
            Engine.Receiver.UnregisterReceiver(EMessageType.MoveToSlotRequest, Engine.Receiver.RESPONSE_ALWAYS_FAIL);
            Engine.Receiver.UnregisterReceiver(EMessageType.SlotSwapRequest, Engine.Receiver.RESPONSE_ALWAYS_FAIL);
            Engine.Receiver.UnregisterReceiver(EMessageType.ConfirmSwapRequest, Engine.Receiver.RESPONSE_ALWAYS_FAIL);
        }


        #endregion

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

            MessageRoomInfos roomInfo = new MessageRoomInfos(Engine.Game.CurrentRoom);
            Engine.Network.Server.BroadcastMessage(roomInfo);
        }

        #region UI Callback
        internal void OnSlotSelected(FFEventParameter a_args)
        {
            SlotRef selectedSlot = (SlotRef)a_args.data;

            FFNetworkPlayer player = Engine.Game.CurrentRoom.GetPlayerForSlot(selectedSlot);
            if (player != Engine.Game.NetPlayer)
            {
                if (player != null)
                {
                    _slotOptionPopupId = FFHostSlotOptionPopup.RequestDisplay(player, OnPlayerOptionKick, OnPlayerOptionBan, OnPlayerOptionSwap, null);
                }
                else
                {
                    SlotRef from = new SlotRef();
                    from.slotIndex = Engine.Game.NetPlayer.slot.slotIndex;
                    from.teamIndex = Engine.Game.NetPlayer.slot.team.teamIndex;

                    Engine.Game.CurrentRoom.MovePlayer(from, selectedSlot);
                }
            }
        }
        #endregion

        #region Client Callbacks
        protected void OnClientAdded(FFTcpClient a_newClient)
        {
            /*FFMessageRoomInfo roomInfo = new FFMessageRoomInfo(FFEngine.Network.CurrentRoom);
            a_newClient.QueueMessage(roomInfo);*/
        }

        protected void OnClientReconnected(FFTcpClient a_recoClient)
        {
            /*FFMessageRoomInfo roomInfo = new FFMessageRoomInfo(FFEngine.Network.CurrentRoom); // Already done from OnRoomUpdate
            a_recoClient.QueueMessage(roomInfo);*/
        }
        #endregion

        #region Kick & Ban
        protected FFNetworkPlayer _targetPlayer = null;
        internal void OnPlayerOptionKick(FFNetworkPlayer a_player)
        {
            _targetPlayer = a_player;
            FFLog.Log(EDbgCat.Logic, "On Player Kick");
            if (!_targetPlayer.isDced)
            {
                FFTcpClient client = Engine.Network.Server.ClientForEP(_targetPlayer.IpEndPoint);
                new Handler.RemovedFromRoom(client, false, OnRemoveSent);
            }
            else
            {
                Engine.Game.CurrentRoom.RemovePlayer(_targetPlayer.SlotRef);
            }

            Engine.UI.DismissPopup(_slotOptionPopupId);
            _slotOptionPopupId = -1;
        }

        internal void OnPlayerOptionBan(FFNetworkPlayer a_player)
        {
            _targetPlayer = a_player;
            FFLog.Log(EDbgCat.Logic, "On Player Ban");
            if (!_targetPlayer.isDced)
            {
                Engine.Game.CurrentRoom.BanId(_targetPlayer.ID);
                FFTcpClient client = Engine.Network.Server.ClientForEP(_targetPlayer.IpEndPoint);
                new Handler.RemovedFromRoom(client, true, OnRemoveSent);
            }
            else
            {
                Engine.Game.CurrentRoom.BanId(_targetPlayer.ID);
                Engine.Game.CurrentRoom.RemovePlayer(_targetPlayer.SlotRef);
            }

            Engine.UI.DismissPopup(_slotOptionPopupId);
            _slotOptionPopupId = -1;
        }

        protected void OnRemoveSent()
        {
            Engine.Game.CurrentRoom.RemovePlayer(_targetPlayer.ID);
        }
        #endregion

        #region Swap
        Handler.SlotSwap _swapHandler;
        internal void OnPlayerOptionSwap(FFNetworkPlayer a_player)
        {
            
            FFTcpClient client = Engine.Network.Server.ClientForEP(a_player.IpEndPoint);
            if (client != null)
            {
                Engine.UI.DismissPopup(_slotOptionPopupId);
                _slotOptionPopupId = -1;
                _swapPopupId = FFLoadingPopup.RequestDisplay("Waiting for " + a_player.player.username + " to respond.", "Cancel", OnSwapCanceled);
                _swapHandler = new Handler.SlotSwap(Engine.Network.Server.LoopbackClient.Mirror,
                                                            a_player.SlotRef,
                                                            OnSwapSuccess,
                                                            OnSwapFailed);
            }
            else
                FFLog.Log("Couldn't swap, client not found.");
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

        #region focus
        #endregion

        #region Start & Network Check
        internal void OnNext(FFEventParameter a_args)
        {
            Engine.Network.Server.PauseAcceptingConnections();
            _roomPanel.StartReadyCheck();

            UnregisterReceiver();
            RegisterForbiddenReceiver();

            new Handler.IsIdleCheck(OnCheckSuccess,OnCheckFailed, OnClientCheckSucceed, OnClientCheckFailed);
        }

        internal void OnCheckSuccess(List<FFTcpClient> succeed, List<FFTcpClient> failed)
        {
            _roomPanel.StopReadyCheck();

            RegisterReceiver();
            UnregisterForbiddenReceiver();

            MessageRequestGameMode loadGameMessage = new MessageRequestGameMode();
            Engine.Network.Server.BroadcastMessage(loadGameMessage);

            RequestMultiGameMode("PongServerGameMode");
        }

        internal void OnCheckFailed(List<FFTcpClient> succeed, List<FFTcpClient> failed)
        {
            RegisterReceiver();
            UnregisterForbiddenReceiver();

            Engine.Network.Server.ResumeAcceptingConnections();
            FFMessageToast.RequestDisplay("Some player aren't ready.");
            _roomPanel.StopReadyCheck();
        }

        internal void OnClientCheckSucceed(FFTcpClient a_client)
        {
            PlayerSlotWidget slotWidget = _roomPanel.SlotForId(a_client.NetworkID);
            if (slotWidget != null)
            {
                slotWidget.readyCheck.SetSuccess();
            }
        }

        internal void OnClientCheckFailed(FFTcpClient a_client)
        {
            PlayerSlotWidget slotWidget = _roomPanel.SlotForId(a_client.NetworkID);
            if (slotWidget != null)
            {
                slotWidget.readyCheck.SetFail();
            }
        }
        #endregion
    }
}