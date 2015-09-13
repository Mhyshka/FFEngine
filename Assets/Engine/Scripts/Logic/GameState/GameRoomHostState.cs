using UnityEngine;
using System.Collections;
using Zeroconf;
using System.Collections.Generic;

using FF.UI;
using FF.Networking;

namespace FF
{
	internal class GameRoomHostState : ANavigationMenuState
	{
		#region Inspector Properties
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
			
			if(FFEngine.Network.IsConnectedToLan())
			{
				_navigationPanel.setTitle ("Waiting for clients...");
				
				FFEngine.Game.PrepareRoom();
				
				FFEngine.Network.StartBroadcastingGame ("Partie de " + _networkGameMode.playerName);
				
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
		}
		
		protected override void UnregisterForEvent ()
		{
			base.UnregisterForEvent ();
			FFEngine.Network.Server.onClientAdded -= OnClientAdded;
			FFEngine.Network.Server.onClientRemoved -= OnClientRemoved;
		}
		#endregion
		
		#region Callback
		protected void OnClientAdded(FFTcpClient a_newClient)
		{
			FFMessageRoomInfo roomInfo = new FFMessageRoomInfo(FFEngine.Game.CurrentRoom);
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
	}
}