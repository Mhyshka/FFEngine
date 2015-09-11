﻿using UnityEngine;
using System.Collections;
using Zeroconf;
using System.Collections.Generic;

using FF.UI;

namespace FF
{
	internal class GameRoomHostState : AMenuGameState
	{
		#region Inspector Properties
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

			FFNavigationBarPanel lNavigationBarPanel = FFEngine.UI.GetPanel ("NavigationBarPanel") as FFNavigationBarPanel;
			lNavigationBarPanel.setTitle ("Youpoupidou");

			NetworkMenuGameMode lGameMode = _gameMode as NetworkMenuGameMode;
			FFEngine.Network.StartBroadcastingGame ("Partie de " + lGameMode.playerName);
		}
		#endregion
	}
}