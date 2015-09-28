using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FF
{
	//Custom Inspector Link ( From AGameState )
	public abstract class AGameMode : MonoBehaviour
	{
		#region Inspector properties
		#endregion
		
		#region Properties
		internal LoadingState loading = null;
		internal ExitState exit = null;
		
		protected int _currentID = -1;
		protected Dictionary<int,AGameState> _states = new Dictionary<int,AGameState>();
        internal AGameState StateForId(int a_id)
        {
            AGameState state = null;
            _states.TryGetValue(a_id, out state);
            return state;
        }
		
		// Loading
		protected int _asyncLoadingCount = 0;
		protected bool _isGameModeLoaded = false;
		protected bool _isUILoaded = false;
		internal virtual bool IsLoadingComplete{get{return _isGameModeLoaded && _isUILoaded;}}
		
		protected bool _isGoingBack = false;
		internal bool IsGoingBack
		{
			get
			{
				return _isGoingBack;
			}
		}
		
		internal AGameState CurrentState
		{
			get
			{
				AGameState state = null;
				if(_currentID != -1)
					state = _states[_currentID];
				return state;
			}
		}
		#endregion
		
		#region Awake & Destroy
		protected /*virtual*/ void Awake()
		{
			foreach(AGameState each in GetComponents<AGameState>())
			{
				if(_states.ContainsKey(each.ID))
				{
					Debug.LogError("States ID found twice : " + each.ID.ToString());
				}
				else
				{
					_states.Add(each.ID, each);
				}
				
				if(each is ExitState)
				{
					exit = each as ExitState;
				}
				
				if(each is LoadingState)
				{
					loading = each as LoadingState;
				}
				
				each.SetGameMode(this);
			}
			
			Enter();
		}
		
		protected /*virtual*/ void OnDestroy()
		{
			Exit();
		}
		#endregion
		
		#region Main
		internal abstract int ID
		{
			get;
		}
		
		protected virtual void Enter()
		{
			RegisterForEvent();
			_currentID = -1;
			FFEngine.Game.RegisterGameMode(this);
			_isGameModeLoaded = loading.additionalRequiredScenes.Length == 0;
			CurrentStateID = loading.ID;
		}
		
		protected virtual void Update ()
		{
			CurrentStateID = _states[CurrentStateID].Manage();
		}
		
		protected virtual void Exit()
		{
			UnregisterForEvent();
			FFEngine.UI.ClearPanels();
			FFEngine.Game.ReleaseGameMode();
		}
		#endregion
		
		#region State Management
		protected virtual int CurrentStateID
		{
			get
			{
				return _currentID;
			}
			set
			{
				if(value < 0)
				{
					Debug.LogError("Invalid state ID : " + value.ToString());
				}
				
				if(value != _currentID)
				{
					if(_currentID != -1)
						_states[_currentID].Exit();
					_currentID = value;
					_states[_currentID].Enter();
					_isGoingBack = false;
				}
			}
		}
		
		internal void ForceQuit()
		{
			CurrentStateID = exit.ID;
		}
		#endregion
		
		#region Event Management
		protected virtual void RegisterForEvent ()
		{
			FFEngine.Events.RegisterForEvent(EEventType.AsyncLoadingComplete, OnAsyncLoadingComplete);
			FFEngine.Events.RegisterForEvent(EEventType.UILoadingComplete, OnUILoadingComplete);
		}
		
		protected virtual void UnregisterForEvent ()
		{
			FFEngine.Events.UnregisterForEvent(EEventType.AsyncLoadingComplete, OnAsyncLoadingComplete);
			FFEngine.Events.UnregisterForEvent(EEventType.UILoadingComplete, OnUILoadingComplete);
		}
		
		private void OnAsyncLoadingComplete(FFEventParameter a_args)
		{
			_asyncLoadingCount--;
			if(_asyncLoadingCount == 0)
			{
				_isGameModeLoaded = true;
			}
			else if(_asyncLoadingCount < 0)
			{
				Debug.LogError("Async loading error, loading count invalid : " + _asyncLoadingCount.ToString());
			}
		}
		
		private void OnUILoadingComplete(FFEventParameter a_args)
		{
			_isUILoaded = true;
		}
		
		internal void LoadAsyncScene(string a_sceneName)
		{
			Application.LoadLevelAdditiveAsync(a_sceneName);
			_asyncLoadingCount++;
		}
		#endregion
		
		#region Pause
		protected void OnApplicationPause(bool a_isPaused)
		{
			if(a_isPaused)
			{
				OnPause();
			}
			else
			{
				OnResume();
			}
		}
		
		protected virtual void OnPause()
		{
			CurrentState.OnPause();
		}
		
		protected virtual void OnResume()
		{
			CurrentState.OnResume();
		}
        #endregion

        #region Focus Popup
        internal virtual void OnLostFocus()
        {
            CurrentState.OnLostFocus();
        }

        internal virtual void OnGetFocus()
        {
            CurrentState.OnGetFocus();
        }
        #endregion
    }
}