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
        private bool _isLoadingShowing;
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
            if (Engine.UI.HasLoadingScreen)
            {
                Engine.UI.DisplayLoading();
                Engine.UI.LoadingScreen.SetLoadingDescription("Loading");
                Engine.UI.LoadingScreen.SetProgress(0f);
                Engine.UI.LoadingScreen.onShown += OnLoadingShown;
                _isLoadingShowing = true;
            }
            else
            {
                _isLoadingShowing = false;
            }
		}
	
		internal override int Manage ()
		{
			if(!_isLoadingShowing && !_finalized)
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
			Engine.Game.RequestGameMode(gameModeToLoad);
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

        internal void OnLoadingShown(FFPanel a_panel)
        {
            _isLoadingShowing = false;
        }
		#endregion
	}
}