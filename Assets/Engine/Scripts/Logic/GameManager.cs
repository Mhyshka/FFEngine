using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace FF.Logic
{
	/// <summary>
	/// Basic Game Manager that handles loading of GameModes.
	/// </summary>
	internal class GameManager : BaseManager
	{
		#region Properties
		protected AGameMode _currentGameMode = null;
		protected string _currentSceneName = "";
        protected AsyncOperation _mainSceneLoading = null;
        #endregion

        #region Manager
        #endregion

        #region Game Mode Management
        internal void RegisterGameMode(AGameMode a_gameMode)
		{
			if(_currentGameMode != null)
			{
                FFLog.LogError(EDbgCat.Logic, "Two game modes are registered at the same time.");
			}
			
			_currentGameMode = a_gameMode;
		}
		
		internal void ReleaseGameMode()
		{
			if(_currentGameMode == null)
			{
                FFLog.LogError(EDbgCat.Logic, "No Game Mode to release.");
			}
			_currentGameMode = null;
		}
		
		internal AGameMode CurrentGameMode
		{
            get
            {
                if (_currentGameMode != null)
                {
                    return _currentGameMode;
                }

                FFLog.LogError(EDbgCat.Logic,"No current game mode.");
                return null;
            }
		}
		
		internal T GetCurrentGameMode<T>() where T : AGameMode
		{
			if(_currentGameMode != null)
			{
				return _currentGameMode as T;
			}

            FFLog.LogError(EDbgCat.Logic, "No current game mode.");
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
            if (CurrentGameMode != null)
            {
                CurrentGameMode.TearDown();
            }
            _currentSceneName = a_sceneName;
            _mainSceneLoading = SceneManager.LoadSceneAsync (a_sceneName, LoadSceneMode.Single);
		}

        internal float MainSceneLoadingProgress
        {
            get
            {
                if (_mainSceneLoading != null)
                    return _mainSceneLoading.progress;

                return 0f;
            }
        }
		#endregion
	}
}