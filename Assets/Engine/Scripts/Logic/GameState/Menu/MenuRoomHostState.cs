using UnityEngine;
using System.Collections;
using Zeroconf;
using System.Collections.Generic;

using System.Net;

using FF.UI;
using FF.Networking;

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


        #region States Methods
        internal override int ID {
            get {
                return (int)EMenuStateID.GameRoomHost;
            }
        }

        internal override void Enter()
        {
            //Needs to stay before enter.
            FFEngine.Network.StartBroadcastingGame("Partie de " + FFEngine.Game.player.username);
            base.Enter();
            FFLog.Log(EDbgCat.Logic, "Game Room Host state enter.");
            _slotOptionPopupId = -1;

            _roomPanel = FFEngine.UI.GetPanel("MenuRoomPanel") as FFMenuRoomPanel;

            _navigationPanel.SetTitle("Waiting for clients...");

            OnRoomUpdate(FFEngine.Network.CurrentRoom);
        }

        internal override void Exit()
        {
            base.Exit();
            if (_slotOptionPopupId != -1)
                FFEngine.UI.DismissPopup(_slotOptionPopupId);
            if (_swapPopupId != -1)
                FFEngine.UI.DismissPopup(_swapPopupId);

            FFEngine.Network.StopBroadcastingGame();
            FFEngine.Network.StopServer();
        }

        internal override void GoBack()
        {
            new FFFarewellHandler(OnShutdownComplete);
        }

        internal void OnShutdownComplete()
        {
            base.GoBack();
        }
        #endregion

        #region Events
        protected override void RegisterForEvent()
        {
            base.RegisterForEvent();
            FFEngine.Events.RegisterForEvent("SlotSelected", OnSlotSelected);
            FFEngine.NetworkStatus.onLanStatusChanged += OnLanStatusChanged;

            FFEngine.Network.CurrentRoom.onRoomUpdated += OnRoomUpdate;
            FFEngine.Network.Server.onClientAdded += OnClientAdded;
            FFEngine.Network.Server.onClientReconnection += OnClientReconnected;
        }

        protected override void UnregisterForEvent()
        {
            base.UnregisterForEvent();
            FFEngine.Events.UnregisterForEvent("SlotSelected", OnSlotSelected);
            FFEngine.NetworkStatus.onLanStatusChanged -= OnLanStatusChanged;

            if (FFEngine.Network.Server != null)
            {
                FFEngine.Network.CurrentRoom.onRoomUpdated -= OnRoomUpdate;
                FFEngine.Network.Server.onClientAdded -= OnClientAdded;
                FFEngine.Network.Server.onClientReconnection -= OnClientReconnected;
            }
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

        protected void OnRoomUpdate(FFRoom a_room)
        {
            _roomPanel.UpdateWithRoom(a_room);

            FFMessageRoomInfo roomInfo = new FFMessageRoomInfo(FFEngine.Network.CurrentRoom);
            FFEngine.Network.Server.BroadcastMessage(roomInfo);
        }

        #region UI Callback
        internal void OnSlotSelected(FFEventParameter a_args)
        {
            FFSlotRef selectedSlot = (FFSlotRef)a_args.data;

            FFNetworkPlayer player = FFEngine.Network.CurrentRoom.GetPlayerForSlot(selectedSlot);
            if (player != FFEngine.Network.Player)
            {
                if (player != null)
                {
                    _slotOptionPopupId = FFHostSlotOptionPopup.RequestDisplay(player, OnPlayerOptionKick, OnPlayerOptionBan, OnPlayerOptionSwap, null);
                }
                else
                {
                    FFSlotRef from = new FFSlotRef();
                    from.slotIndex = FFEngine.Network.Player.slot.slotIndex;
                    from.teamIndex = FFEngine.Network.Player.slot.team.teamIndex;

                    FFEngine.Network.CurrentRoom.MovePlayer(from, selectedSlot);
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
            if (!_targetPlayer.isDCed)
            {
                FFTcpClient client = FFEngine.Network.Server.ClientForEP(_targetPlayer.IpEndPoint);
                new FFRemovedFromRoomHandler(client, false, OnRemoveSent);
            }
            else
            {
                FFEngine.Network.CurrentRoom.RemovePlayer(_targetPlayer.SlotRef);
            }

            FFEngine.UI.DismissPopup(_slotOptionPopupId);
            _slotOptionPopupId = -1;
        }

        internal void OnPlayerOptionBan(FFNetworkPlayer a_player)
        {
            _targetPlayer = a_player;
            FFLog.Log(EDbgCat.Logic, "On Player Kick");
            if (!_targetPlayer.isDCed)
            {
                FFEngine.Network.CurrentRoom.BanId(_targetPlayer.ID);
                FFTcpClient client = FFEngine.Network.Server.ClientForEP(_targetPlayer.IpEndPoint);
                new FFRemovedFromRoomHandler(client, true, OnRemoveSent);
            }
            else
            {
                FFEngine.Network.CurrentRoom.BanId(_targetPlayer.ID);
                FFEngine.Network.CurrentRoom.RemovePlayer(_targetPlayer.SlotRef);
            }

            FFEngine.UI.DismissPopup(_slotOptionPopupId);
            _slotOptionPopupId = -1;
        }

        protected void OnRemoveSent()
        {
            FFEngine.Network.CurrentRoom.RemovePlayer(_targetPlayer.ID);
            FFLog.LogError("Player count after remove : " + FFEngine.Network.CurrentRoom.TotalPlayers);
        }
        #endregion

        #region Swap
        FFSlotSwapHandler _swapHandler;
        internal void OnPlayerOptionSwap(FFNetworkPlayer a_player)
        {
            
            FFTcpClient client = FFEngine.Network.Server.ClientForEP(a_player.IpEndPoint);
            if (client != null)
            {
                FFEngine.UI.DismissPopup(_slotOptionPopupId);
                _slotOptionPopupId = -1;
                _swapPopupId = FFLoadingPopup.RequestDisplay("Waiting for " + a_player.player.username + " to respond.", "Cancel", OnSwapCanceled);
                _swapHandler = new FFSlotSwapHandler(FFEngine.Network.Server.LoopbackClient.Mirror,
                                                            a_player.SlotRef,
                                                            OnSwapSuccess,
                                                            OnSwapFailed);
            }
            else
                FFLog.Log("Couldn't swap, client not found.");
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