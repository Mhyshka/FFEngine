using UnityEngine;
using System.Collections;
using Zeroconf;
using System.Collections.Generic;

using FF.UI;

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
				FFEngine.Network.StartBroadcastingGame ("Partie de " + _networkGameMode.playerName);
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
		}
		#endregion
	}
}