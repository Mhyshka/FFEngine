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
			_navigationPanel.SetTitle ("Game Lobby");

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

            FFMessageKick.onKickReceived += OnKickReceived;
        }
		
		protected override void UnregisterForEvent ()
		{
			base.UnregisterForEvent ();
            FFEngine.Events.UnregisterForEvent("SlotSelected", OnSlotSelected);

            FFEngine.NetworkStatus.onLanStatusChanged -= OnLanStatusChanged;

            FFMessageKick.onKickReceived -= OnKickReceived;
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

        protected void OnConnectionLost(FFTcpClient a_client)
        {
            FFConnectionLostPopup.RequestDisplay(OnErroPopupCancel);
        }

        protected void OnReconnection(FFTcpClient a_client)
        {
            FFEngine.UI.DismissCurrentPopup();
            _roomPanel.TrySelectWidget();
        }

        protected void OnErroPopupCancel()
        {
            FFEngine.UI.DismissCurrentPopup();
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
            FFMessagePopup.RequestDisplay("Server's down : " + a_reason, "Ok", null);
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

        #region Slot Options 
        internal void OnKickReceived()
        {
            FFEngine.Events.FireEvent(EEventType.Back);
            FFMessagePopup.RequestDisplay("Kicked by server.", "Sorry", null);
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