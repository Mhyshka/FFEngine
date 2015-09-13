using UnityEngine;
using System.Collections;

using FF.UI;

namespace FF
{
	// Custom editor
	public class ExitState : AGameState
	{
		#region Inspector Properties
		public string gameModeToLoad = "MenuGameMode";
		#endregion
	
		#region Properties
		private bool _finalized;
		#endregion
	
		#region States Methods
		internal override int ID
		{
			get
			{
				return (int)EStateID.Exit;
			}
		}
	
		internal override void Enter ()
		{
			base.Enter ();
			_finalized = false;
			if(FFEngine.UI.HasLoadingScreen)
				FFEngine.UI.DisplayLoading();
		}
	
		internal override int Manage ()
		{
			if(FFEngine.UI.LoadingScreenState == FFPanel.EState.Shown && !_finalized)
			{
				FinalizeGameMode();
			}
			return base.Manage ();
		}
	
		internal override void Exit ()
		{
			base.Exit ();
		}
		
		internal void FinalizeGameMode()
		{
			_finalized = true;
			FFEngine.Game.RequestGameMode(gameModeToLoad);
		}
		#endregion
	
		#region Event Management
		/*protected override void RegisterForEvent ()
		{
			base.RegisterForEvent ();
		}
	
		protected override void UnregisterForEvent ()
		{
			base.UnregisterForEvent ();
		}
	
		internal void OnEvent(FFEventParameter a_args)
		{
		}
		*/
		#endregion
	}
}