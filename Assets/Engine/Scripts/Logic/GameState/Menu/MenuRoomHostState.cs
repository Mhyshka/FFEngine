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
			FFSlotRef from = new FFSlotRef();
			from.slotIndex = FFEngine.Network.Player.slot.slotIndex;
			from.teamIndex = FFEngine.Network.Player.slot.team.teamIndex;
			
			FFSlotRef to = (FFSlotRef)a_args.data;
			
			FFEngine.Network.CurrentRoom.MovePlayer(from, to);
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
    }
}