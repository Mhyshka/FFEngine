using UnityEngine;
using System.Collections;

//Custom Inspector
public abstract class AGameState : MonoBehaviour
{
	#region Inspector Properties
	public AGameState outState = null;
	
	[HideInInspector]
	public int panelsToShow = 0;
	#endregion
	
	#region Properties
	protected AGameMode _gameMode;
	protected float _timeElapsedSinceEnter = 0f;
	
	private int _requestedStateId = 0;
	#endregion
	
	#region States Methods
	internal abstract int ID
	{
		get;
	}

	internal virtual void Enter ()
	{
		_timeElapsedSinceEnter = 0f;
		_requestedStateId = -1;
		
		if(panelsToShow != 0)
		{
			FFEngine.UI.SwitchToPanels(FFUtils.BitMaskToPanels(panelsToShow,
		                                                  	  _gameMode.loading.loadedPanelsScenes));
		}
		                                                  	  
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
}
