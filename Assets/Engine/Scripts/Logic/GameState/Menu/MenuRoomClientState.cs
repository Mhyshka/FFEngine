﻿using UnityEngine;
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
			
			_roomPanel = FFEngine.UI.GetPanel("MenuRoomPanel") as FFMenuRoomPanel;
			_roomPanel.UpdateWithRoom(FFEngine.Network.CurrentRoom);
			_navigationPanel.setTitle ("Game Lobby");

            OnLanStatusChanged(FFEngine.NetworkStatus.IsConnectedToLan);
		}
		
		internal override void Exit ()
		{
			base.Exit ();
            LeaveRoom();
        }
		#endregion
		
		#region Events
		protected override void RegisterForEvent ()
		{
			base.RegisterForEvent ();
			FFEngine.Events.RegisterForEvent("SlotSelected", OnSlotSelected);
			FFEngine.Network.CurrentRoom.onRoomUpdated += OnRoomUpdate;
            FFEngine.Network.MainClient.onConnectionEnded += OnConnectionEnded;

            FFEngine.NetworkStatus.onLanStatusChanged += OnLanStatusChanged;
            FFEngine.Network.MainClient.onConnectionSuccess += OnReconnection;
            FFEngine.Network.MainClient.onConnectionLost += OnConnectionLost;
            FFEngine.Events.RegisterForEvent("CancelReconnection", OnCancelReconnection);
        }
		
		protected override void UnregisterForEvent ()
		{
			base.UnregisterForEvent ();
            FFEngine.Events.UnregisterForEvent("SlotSelected", OnSlotSelected);

            FFEngine.NetworkStatus.onLanStatusChanged -= OnLanStatusChanged;
            FFEngine.Events.UnregisterForEvent("CancelReconnection", OnCancelReconnection);
        }
        #endregion

        #region ConnectionLost
        protected void OnLanStatusChanged(bool a_state)
        {
            /*if (a_state)
            {
                FFEngine.UI.HideSpecificPanel("MenuWifiCheckPanel");
            }
            else
            {
                FFEngine.UI.RequestDisplay("MenuWifiCheckPanel");
            }*/
        }

        protected void OnConnectionLost(FFTcpClient a_client)
        {
            FFEngine.UI.RequestDisplay("ConnectionLostPanel");
        }

        protected void OnReconnection(FFTcpClient a_client)
        {
            FFEngine.UI.HideSpecificPanel("ConnectionLostPanel");
            _roomPanel.TrySelectWidget();
        }

        protected void OnCancelReconnection(FFEventParameter a_args)
        {
            FFEngine.UI.HideSpecificPanel("ConnectionLostPanel");
            FFEngine.Events.FireEvent(EEventType.Back);
        }
        #endregion

        #region Inputs Events
        internal void OnSlotSelected(FFEventParameter a_args)
		{
			FFSlotRef selectedSlot = (FFSlotRef)a_args.data;
			FFEngine.Network.MainClient.QueueMessage(new FFMoveToSlotRequest(selectedSlot));
		}
		
		protected void OnRoomUpdate(FFRoom a_room)
		{
			_roomPanel.UpdateWithRoom(a_room);
		}

        internal override void GoBack(int a_id)
        {
            if (FFEngine.Network.MainClient.IsConnected)
            {
                FFMessageLeavingRoom leaveMess = new FFMessageLeavingRoom();
                FFEngine.Network.MainClient.QueueMessage(leaveMess);
            }
            FFEngine.Network.MainClient.onConnectionSuccess -= OnReconnection;
            FFEngine.Network.MainClient.onConnectionLost -= OnConnectionLost;
            LeaveRoom();
            base.GoBack(a_id);
        }

        protected void OnConnectionEnded(FFTcpClient a_client, string a_reason)
        {
            FFEngine.Events.FireEvent(EEventType.Back);
        }

        protected void LeaveRoom()
        {
            if (FFEngine.Network.MainClient != null)
            {
                FFEngine.Network.CurrentRoom.onRoomUpdated -= OnRoomUpdate;
                FFEngine.Network.MainClient.onConnectionEnded -= OnConnectionEnded;
                FFEngine.Network.LeaveCurrentRoom();
            }
        }
		#endregion
	}
}