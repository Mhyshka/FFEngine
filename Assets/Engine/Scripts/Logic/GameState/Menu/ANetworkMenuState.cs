using UnityEngine;
using System.Collections;


namespace FF
{
	internal abstract class ANetworkMenuState : AMenuGameState
	{
		#region Properties
		protected NetworkMenuGameMode _networkGameMode;
		#endregion
		
		internal override void Enter ()
		{
			base.Enter ();
			_networkGameMode = _gameMode as NetworkMenuGameMode;
		}
	}
}