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
		
		#endregion
		

		#region States Methods
		internal override int ID {
			get {
				return (int)EMenuStateID.GameRoomHost;
			}
		}

		internal override void Enter ()
		{
			base.Enter ();
			FFLog.Log (EDbgCat.Logic, "Game Room Host state enter.");
			
			_roomPanel = FFEngine.UI.GetPanel("MenuRoomPanel") as FFMenuRoomPanel;
			
			if(FFEngine.NetworkStatus.IsConnectedToLan)
			{
				_navigationPanel.setTitle ("Waiting for clients...");

				FFEngine.Network.StartBroadcastingGame ("Partie de " + FFEngine.Game.player.username);
				
				FFEngine.Network.CurrentRoom.onRoomUpdated += OnRoomUpdate;
				
				OnRoomUpdate(FFEngine.Network.CurrentRoom);
				
				FFEngine.Network.Server.onClientAdded += OnClientAdded;
				FFEngine.Network.Server.onClientRemoved += OnClientRemoved;
			}
			else
			{
				_navigationPanel.setTitle ("No network");
			}
		}
		
		internal override void Exit ()
		{
			base.Exit ();
			FFEngine.Network.StopBroadcastingGame();
			FFEngine.Network.StopServer();
		}
		#endregion
		
		#region Events
		protected override void RegisterForEvent ()
		{
			base.RegisterForEvent ();
			FFEngine.Events.RegisterForEvent("SlotSelected", OnSlotSelected);
		}
		
		protected override void UnregisterForEvent ()
		{
			base.UnregisterForEvent ();
			FFEngine.Events.UnregisterForEvent("SlotSelected", OnSlotSelected);

			if(FFEngine.Network.CurrentRoom != null)
			{
				FFEngine.Network.CurrentRoom.onRoomUpdated -= OnRoomUpdate;
			}

			if(FFEngine.Network.Server != null)
			{
				FFEngine.Network.Server.onClientAdded -= OnClientAdded;
				FFEngine.Network.Server.onClientRemoved -= OnClientRemoved;
			}
		}
		#endregion
		
		#region Callback
		internal void OnSlotSelected(FFEventParameter a_args)
		{
            FFSlotRef selectedSlot = (FFSlotRef)a_args.data;

            FFNetworkPlayer player = FFEngine.Network.CurrentRoom.GetPlayerForSlot(selectedSlot);
            if (player != null)
            {
                FFSlotOptionPopup.RequestDisplay(player, OnPlayerOptionKick, OnPlayerOptionBan, OnPlayerOptionSwap, null);
            }
            else
            {
                FFSlotRef from = new FFSlotRef();
                from.slotIndex = FFEngine.Network.Player.slot.slotIndex;
                from.teamIndex = FFEngine.Network.Player.slot.team.teamIndex;

                FFEngine.Network.CurrentRoom.MovePlayer(from, selectedSlot);
            }
		}
		
		protected void OnRoomUpdate(FFRoom a_room)
        {
			_roomPanel.UpdateWithRoom(a_room);
			
			FFMessageRoomInfo roomInfo = new FFMessageRoomInfo(FFEngine.Network.CurrentRoom);
            FFEngine.Network.Server.BroadcastMessage(roomInfo);
        }
		
		protected void OnClientAdded(FFTcpClient a_newClient)
		{
			FFMessageRoomInfo roomInfo = new FFMessageRoomInfo(FFEngine.Network.CurrentRoom);
			a_newClient.QueueMessage(roomInfo);
		}
		
		protected void OnClientRemoved(FFTcpClient a_newClient)
		{
			/*FFMessageRoomInfo roomInfo = new FFMessageRoomInfo();
			roomInfo.currentPlayerCount = 2;
			roomInfo.maxPlayerCount = 3;
			roomInfo.gameName = "My Game!";
			
			a_newClient.QueueMessage(roomInfo);*/
		}
        #endregion

        internal override void GoBack(int a_id)
        {
            FFEngine.Network.Server.BroadcastMessage(new FFMessageFarewell("Shuting down."));
            base.GoBack(a_id);
        }


        #region Slot Options
        internal void OnPlayerOptionKick(FFNetworkPlayer a_player)
        {
            FFLog.Log("On Player Kick");
            if (!a_player.isDCed)
            {
                FFEngine.Network.Server.SendMessageToClient(a_player.ipEndPoint, new FFMessageKick());
            }
            else
            {
                FFEngine.Network.CurrentRoom.RemovePlayer(a_player.SlotRef);
            }

            FFEngine.UI.DismissCurrentPopup();
        }

        internal void OnPlayerOptionBan(FFNetworkPlayer a_player)
        {
            FFEngine.UI.DismissCurrentPopup();
        }

        internal void OnPlayerOptionSwap(FFNetworkPlayer a_player)
        {
            FFEngine.UI.DismissCurrentPopup();
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