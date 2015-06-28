using UnityEngine;
using System.Collections;
using System.Collections.Generic;

internal class GameManager
{
	#region Properties
	internal List<GameLevelData> levels = new List<GameLevelData>();
	internal int levelIndex = 0;
	
	protected AGameMode _currentGameMode = null;
	protected string _currentSceneName = "";
	#endregion

	internal GameManager()
	{
	
	}

	#region Game Mode Management
	internal void RegisterGameMode(AGameMode a_gameMode)
	{
		if(_currentGameMode != null)
		{
			Debug.LogError("Two game modes are registered at the same time.");
		}
		
		_currentGameMode = a_gameMode;
	}
	
	internal void ReleaseGameMode()
	{
		if(_currentGameMode == null)
		{
			Debug.LogError("No Game Mode to release.");
		}
		_currentGameMode = null;
	}
	
	internal AGameMode CurrentGameMode()
	{
		if(_currentGameMode != null)
		{
			return _currentGameMode;
		}
		
		Debug.LogError("No current game mode.");
		return null;
	}
	
	internal T CurrentGameMode<T>() where T : AGameMode
	{
		if(_currentGameMode != null)
		{
			return _currentGameMode as T;
		}
		
		Debug.LogError("No current game mode.");
		return null;
	}
	
	internal int CurrentGameModeID
	{
		get
		{
			if(_currentGameMode != null)
			{
				return _currentGameMode.ID;
			}
			
			Debug.LogError("No current game mode.");
			return -1;
		}
	}
	#endregion
	
	#region Loading
	internal virtual void RequestGameMode(string a_sceneName)
	{
		Application.LoadLevelAsync (a_sceneName);
	}
	#endregion
}