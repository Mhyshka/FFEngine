using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Custom Inspector Link ( From AGameState )
public abstract class AGameMode : MonoBehaviour
{
	#region Inspector properties
	#endregion
	
	#region Properties
	internal LoadingState loading = null;
	internal ExitState exit = null;
	
	protected int _currentID = -1;
	protected Dictionary<int,AGameState> states = new Dictionary<int,AGameState>();
	
	// Loading
	protected int _asyncLoadingCount = 0;
	protected bool _isGameModeLoaded = false;
	protected bool _isUILoaded = false;
	internal virtual bool IsLoadingComplete{get{return _isGameModeLoaded && _isUILoaded;}}
	
	internal AGameState CurrentState
	{
		get
		{
			AGameState state = null;
			if(_currentID != -1)
				state = states[_currentID];
			return state;
		}
	}
	#endregion
	
	#region Awake & Destroy
	protected /*virtual*/ void Awake()
	{
		foreach(AGameState each in GetComponents<AGameState>())
		{
			if(states.ContainsKey(each.ID))
			{
				Debug.LogError("States ID found twice : " + each.ID.ToString());
			}
			else
			{
				states.Add(each.ID, each);
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
		CurrentStateID = states[CurrentStateID].Manage();
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
					states[_currentID].Exit();
				_currentID = value;
				states[_currentID].Enter();
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
}