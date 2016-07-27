using UnityEngine;
using System.Collections;
using Zeroconf;
using System.Collections.Generic;

using System.Net;

using FF.UI;
using FF.Handler;
using FF.Network;
using FF.Network.Message;
using FF.Network.Receiver;
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
        protected JoinRoomReceiver _joinRoomReceiver;
        protected MoveToSlotReceiver _moveToSlotReceiver;
        protected MultiInstanceReceiver<InstanceSlotSwapReceiver> _slotSwapReceiver;
        protected MultiInstanceReceiver<InstanceConfirmSwapReceiver> _confirmSwapReceiver;
        #endregion

        #region States Methods
        internal override int ID
        {
            get
            {
                return (int)EMenuStateID.GameRoomHost;
            }
        }

        internal override void Enter()
        {
            base.Enter();
            InitNetwork();
            Engine.Network.StartServer(Engine.Game.Player.username + "'s game");
            Engine.Network.StartBroadcastingGame(Engine.Game.Player.username + "'s game");

            FFLog.Log(EDbgCat.Logic, "Game Room Host state enter.");
            _slotOptionPopupId = -1;

            _roomPanel = Engine.UI.GetPanel("MenuRoomPanel") as FFMenuRoomPanel;
            _roomPanel.SetRoomEndpoint(Engine.Network.TcpServer.LocalEndpoint);
            _navigationPanel.SetTitle("Game Lobby");

            OnRoomUpdate(Engine.Network.CurrentRoom);
        }

        private void InitNetwork()
        {
            if (_joinRoomReceiver == null)
                _joinRoomReceiver = new JoinRoomReceiver();
            if (_moveToSlotReceiver == null)
                _moveToSlotReceiver = new MoveToSlotReceiver();
            if (_slotSwapReceiver == null)
                _slotSwapReceiver = new MultiInstanceReceiver<InstanceSlotSwapReceiver>();
            if (_confirmSwapReceiver == null)
                _confirmSwapReceiver = new MultiInstanceReceiver<InstanceConfirmSwapReceiver>();
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

        SentBroadcastMessage _broadcastFarewell;
        internal override void GoBack()
        {
            if (_broadcastFarewell == null)
            {
                _broadcastFarewell = new SentBroadcastMessage(Engine.Network.GameServer.GetClientsIds(),
                                                                                    new MessageIntegerData((int)EFarewellCode.Shuttingdown),
                                                                                    EMessageChannel.Farewell.ToString(),
                                                                                    false,
                                                                                    false,
                                                                                    2f);
                _broadcastFarewell.onEveryMessageSent += OnShutdownComplete;
                _broadcastFarewell.onTimeout += delegate { OnShutdownComplete(); };
                _broadcastFarewell.Broadcast();
            }
        }

        internal void OnShutdownComplete()
        {
            base.GoBack();
            _broadcastFarewell = null;
        }
        #endregion

        #region Events
        protected override void RegisterForEvent()
        {
            base.RegisterForEvent();
            Engine.Events.RegisterForEvent(FFEventType.Next, OnNext);
            Engine.NetworkStatus.onLanStatusChanged += OnLanStatusChanged;

            Engine.Network.CurrentRoom.onRoomUpdated += OnRoomUpdate;

            RegisterReceiver();
        }

        private void RegisterReceiver()
        {
            Engine.Receiver.RegisterReceiver(EMessageChannel.JoinRoom.ToString(), _joinRoomReceiver);
            Engine.Receiver.RegisterReceiver(EMessageChannel.MoveToSlot.ToString(), _moveToSlotReceiver);
            Engine.Receiver.RegisterReceiver(EMessageChannel.SwapSlot.ToString(), _slotSwapReceiver);
            Engine.Receiver.RegisterReceiver(EMessageChannel.SwapConfirm.ToString(), _confirmSwapReceiver);
            Engine.Events.RegisterForEvent("SlotSelected", OnSlotSelected);
        }

        private void RegisterForbiddenReceiver()
        {
            Engine.Receiver.RegisterReceiver(EMessageChannel.JoinRoom.ToString(), Engine.Receiver.RESPONSE_ALWAYS_FAIL);
            Engine.Receiver.RegisterReceiver(EMessageChannel.MoveToSlot.ToString(), Engine.Receiver.RESPONSE_ALWAYS_FAIL);
            Engine.Receiver.RegisterReceiver(EMessageChannel.SwapSlot.ToString(), Engine.Receiver.RESPONSE_ALWAYS_FAIL);
            Engine.Receiver.RegisterReceiver(EMessageChannel.SwapConfirm.ToString(), Engine.Receiver.RESPONSE_ALWAYS_FAIL);
        }

        protected override void UnregisterForEvent()
        {
            base.UnregisterForEvent();
            Engine.Events.UnregisterForEvent(FFEventType.Next, OnNext);
            Engine.Events.UnregisterForEvent("SlotSelected", OnSlotSelected);
            Engine.NetworkStatus.onLanStatusChanged -= OnLanStatusChanged;

            if (Engine.Network.TcpServer != null)
            {
                Engine.Network.CurrentRoom.onRoomUpdated -= OnRoomUpdate;
            }

            UnregisterReceiver();
        }

        private void UnregisterReceiver()
        {
            Engine.Receiver.UnregisterReceiver(EMessageChannel.JoinRoom.ToString(), _joinRoomReceiver);
            Engine.Receiver.UnregisterReceiver(EMessageChannel.MoveToSlot.ToString(), _moveToSlotReceiver);
            Engine.Receiver.UnregisterReceiver(EMessageChannel.SwapSlot.ToString(), _slotSwapReceiver);
            Engine.Receiver.UnregisterReceiver(EMessageChannel.SwapConfirm.ToString(), _confirmSwapReceiver);
            Engine.Events.UnregisterForEvent("SlotSelected", OnSlotSelected);
        }

        private void UnregisterForbiddenReceiver()
        {
            Engine.Receiver.UnregisterReceiver(EMessageChannel.JoinRoom.ToString(), Engine.Receiver.RESPONSE_ALWAYS_FAIL);
            Engine.Receiver.UnregisterReceiver(EMessageChannel.MoveToSlot.ToString(), Engine.Receiver.RESPONSE_ALWAYS_FAIL);
            Engine.Receiver.UnregisterReceiver(EMessageChannel.SwapSlot.ToString(), Engine.Receiver.RESPONSE_ALWAYS_FAIL);
            Engine.Receiver.UnregisterReceiver(EMessageChannel.SwapConfirm.ToString(), Engine.Receiver.RESPONSE_ALWAYS_FAIL);
        }
        #endregion

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

        #region UI Callback
        internal void OnSlotSelected(FFEventParameter a_args)
        {
            SlotRef selectedSlot = (SlotRef)a_args.data;

            FFNetworkPlayer player = Engine.Network.CurrentRoom.GetPlayerForSlot(selectedSlot);
            
            if (player != Engine.Network.NetPlayer)
            {
                if (player != null)
                {
                    _slotOptionPopupId = FFHostSlotOptionPopup.RequestDisplay(player, OnPlayerOptionKick, OnPlayerOptionBan, OnPlayerOptionSwap, null);
                }
                else
                {
                    SlotRef from = new SlotRef();
                    from.slotIndex = Engine.Network.NetPlayer.slot.slotIndex;
                    from.teamIndex = Engine.Network.NetPlayer.slot.team.teamIndex;

                    Engine.Network.CurrentRoom.MovePlayer(from, selectedSlot);
                }
            }
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
                FFNetworkClient client = Engine.Network.GameServer.ClientForId(_targetPlayer.ID);
                MessageBoolData data = new MessageBoolData(false);
                SentMessage message = new SentMessage(data,
                                                        EMessageChannel.RemovedFromRoom.ToString());
                message.onMessageSent += OnRemoveSent;
                client.QueueMessage(message);
            }
            else
            {
                Engine.Network.CurrentRoom.RemovePlayer(_targetPlayer.SlotRef);
            }

            Engine.UI.DismissPopup(_slotOptionPopupId);
            _slotOptionPopupId = -1;
        }

        internal void OnPlayerOptionBan(FFNetworkPlayer a_player)
        {
            _targetPlayer = a_player;
            FFLog.Log(EDbgCat.Logic, "On Player Ban");

            FFNetworkClient client = Engine.Network.GameServer.ClientForId(_targetPlayer.ID);
            if (!_targetPlayer.isDced)
            {
                Engine.Network.CurrentRoom.BanIP(client.Remote.Address);

                MessageBoolData data = new MessageBoolData(true);
                SentMessage message = new SentMessage(data,
                                                        EMessageChannel.RemovedFromRoom.ToString());
                message.onMessageSent += OnRemoveSent;
                client.QueueMessage(message);
            }
            else
            {
                Engine.Network.CurrentRoom.BanIP(client.Remote.Address);
                Engine.Network.CurrentRoom.RemovePlayer(_targetPlayer.SlotRef);
            }

            Engine.UI.DismissPopup(_slotOptionPopupId);
            _slotOptionPopupId = -1;
        }

        protected void OnRemoveSent()
        {
            Engine.Network.CurrentRoom.RemovePlayer(_targetPlayer.ID);
        }
        #endregion

        #region Swap
        SentRequest _swapRequest;
        internal void OnPlayerOptionSwap(FFNetworkPlayer a_player)
        {
            FFNetworkClient client = Engine.Network.GameServer.ClientForId(a_player.ID);
            if (client != null)
            {
                Engine.UI.DismissPopup(_slotOptionPopupId);
                _slotOptionPopupId = -1;
                _swapPopupId = FFLoadingPopup.RequestDisplay("Waiting for " + a_player.player.username + " to respond.", "Cancel", OnSwapCanceled);
                MessageSlotRefData data = new MessageSlotRefData(a_player.SlotRef);
                _swapRequest = new SentRequest(data,
                                                EMessageChannel.SwapSlot.ToString(),
                                                Engine.Network.NextRequestId,
                                                float.MaxValue,
                                                true,
                                                true);
                _swapRequest.onSucces += OnSwapSuccess;
                _swapRequest.onFail += OnSwapFailed;

                Engine.Network.TcpServer.LoopbackClient.Mirror.QueueRequest(_swapRequest);
            }
            else
                FFLog.Log("Couldn't swap, client not found.");
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

                    EErrorCodeSwapSlot errorCode = (EErrorCodeSwapSlot)data.Data;
                    //TODO
                    message = errorCode.ToString();
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

        #region focus
        #endregion

        #region Start & Network Check
        internal void OnNext(FFEventParameter a_args)
        {
            Engine.Network.TcpServer.PauseAcceptingConnections();
            _roomPanel.StartReadyCheck();

            UnregisterReceiver();
            RegisterForbiddenReceiver();

            SentBroadcastRequest broadcastIsIdle = new SentBroadcastRequest(Engine.Network.CurrentRoom.GetPlayersIds(),
                                                                            new MessageEmptyData(),
                                                                            EMessageChannel.IsIdle.ToString(),
                                                                            Engine.Network.NextRequestId,
                                                                            true,
                                                                            true,
                                                                            2f);
            broadcastIsIdle.onFailureForClient += OnClientCheckFailed;
            broadcastIsIdle.onSuccessForClient += OnClientCheckSucceed;
            broadcastIsIdle.onResult += OnCheckResult;
            broadcastIsIdle.Broadcast();
        }

        internal void OnCheckResult(Dictionary<FFNetworkClient, ReadResponse> succeed, Dictionary<FFNetworkClient, ReadResponse> failed)
        {
            if (failed.Count > 0)
            {
                //failure
                Engine.Network.TcpServer.ResumeAcceptingConnections();

                RegisterReceiver();
                UnregisterForbiddenReceiver();

                FFMessageToast.RequestDisplay("Some player aren't ready.");
                _roomPanel.StopReadyCheck();
            }
            else
            {
                //Success
                _roomPanel.StopReadyCheck();

                RegisterReceiver();
                UnregisterForbiddenReceiver();

                SentBroadcastMessage message = new SentBroadcastMessage(Engine.Network.CurrentRoom.GetPlayersIds(),
                                                                        new MessageEmptyData(),
                                                                        EMessageChannel.StartGame.ToString());
                message.Broadcast();

                Engine.Network.GameServer.KeepPlayersOnly(Engine.Network.CurrentRoom.GetPlayersIds());
                Engine.Network.CurrentRoom.EnableHandleDeconnectionOnly();

                RequestMultiGameMode("PongServerGameMode");
            }
        }

        internal void OnClientCheckSucceed(FFNetworkClient a_client, ReadResponse a_response)
        {
            PlayerSlotWidget slotWidget = _roomPanel.SlotForId(a_client.NetworkID);
            if (slotWidget != null)
            {
                slotWidget.readyCheck.SetSuccess();
            }
        }

        internal void OnClientCheckFailed(FFNetworkClient a_client, ReadResponse a_response)
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