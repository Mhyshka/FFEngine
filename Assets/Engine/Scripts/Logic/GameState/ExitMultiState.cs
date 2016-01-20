using UnityEngine;
using System.Collections;

using FF.UI;

namespace FF
{
	// Custom editor
	public class ExitMultiState : ExitState
	{
		#region Inspector Properties
		#endregion
	
		#region Properties
		#endregion
	
		#region States Methods
		internal override void Enter ()
		{
			base.Enter ();

            if (Engine.Game.Loading.loadMultiplayerGameMode)
            {
                Engine.Game.Loading.loadMultiplayerGameMode = false;
                if (Engine.Network.IsServer)
                {
                    Engine.Game.Loading.RegisterLoadingComplete();
                }
                else
                {
                    Engine.Game.Loading.RegisterLoadingStarted();
                }
            }
		}
		#endregion
	
		#region Event Management
		#endregion
	}
}