using UnityEngine;
using System.Collections;

namespace FF
{
	//Custom Inspector
	public abstract class AGameState : MonoBehaviour
	{
		#region Inspector Properties
		public AGameState outState = null;
		
		[HideInInspector]
		public int panelsToShow = 0;
		
		[HideInInspector]
		public string[] panelNamesToShow = null;
		#endregion
		
		#region Properties
		protected AGameMode _gameMode;
		protected float _timeElapsedSinceEnter = 0f;
		
		private int _requestedStateId = 0;

        protected GameObject _previousFocus;
		#endregion
		
		#region States Methods
		internal abstract int ID
		{
			get;
		}
	
		internal virtual void Enter ()
		{
            _previousFocus = null;
            _timeElapsedSinceEnter = 0f;
			_requestedStateId = -1;
			
			//TODO : debug editor script. Temporary
			foreach(string each in panelNamesToShow)
			{
				FFLog.Log(EDbgCat.UI, "Requesting panel " + each + " from " + this.GetType().ToString());
			}
            //debug editor script. Temporary

            if (panelsToShow != 0)
            {
                Engine.UI.SwitchToPanels(panelNamesToShow, !_gameMode.IsGoingBack);
            }
            else
            {
                Engine.UI.HideAllPanels();
            }
			                                                  	  
			
		}

        internal void PostEnter()
        {
            RegisterForEvent();
        }
	
		internal virtual int Manage ()
		{
			_timeElapsedSinceEnter += Time.deltaTime;
			if(_requestedStateId != -1)
				return _requestedStateId;
				
			return ID;
		}
	
		internal virtual void Exit ()
		{
            _previousFocus = null;
            UnregisterForEvent();
		}
		#endregion
		
		#region Event Management
		protected virtual void RegisterForEvent ()
		{
		}
	
		protected virtual void UnregisterForEvent ()
		{
		}
	
		/*
		internal void OnEvent(FFEventParameter a_args)
		{
		}
		*/
		#endregion
		
		protected void RequestState(int a_id)
		{
			_requestedStateId = a_id;
		}
		
		internal void SetGameMode(AGameMode a_gameMode)
		{
			_gameMode = a_gameMode;
		}
		
		internal void RequestGameMode(string a_gameMode)
		{
			_gameMode.exit.gameModeToLoad = a_gameMode;
			_gameMode.ForceQuit();
		}

        internal void RequestMultiGameMode(string a_gameMode)
        {
            Engine.Game.Loading.loadMultiplayerGameMode = true;
            RequestGameMode(a_gameMode);
        }
		
		#region Pause
		internal virtual void OnPause()
		{
		
		}

		internal virtual void OnResume()
		{
		
		}
        #endregion

        #region Focus Popup
        internal virtual void OnLostFocus()
        {
            _previousFocus = UICamera.selectedObject;
        }

        internal virtual void OnGetFocus()
        {
            if (_previousFocus != null)
                UICamera.selectedObject = _previousFocus;
        }
        #endregion
    }
}