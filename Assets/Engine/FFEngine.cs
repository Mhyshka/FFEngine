using UnityEngine;
using System.Collections;

using FF.UI;
using FF.Networking;
using FF.Input;
using FF.Multiscreen;

namespace FF
{
	internal class FFEngine
	{	
		#region Properties
		private static FFEngine s_instance;
		internal static FFEngine Instance
		{
			get
			{
				return s_instance;
			}
		}
		
		private EventManager _eventManager;
		internal static EventManager Events{get{return s_instance._eventManager;}}
		
		private UIManager _uiManager;
		internal static UIManager UI{get{return s_instance._uiManager;}}
		
		private InputManager _inputManager;
		internal static InputManager Inputs{get{return s_instance._inputManager;}}
		
		private FFNetworkManager _networkManager;
		internal static FFNetworkManager Network{get{return s_instance._networkManager;}}
		
		private GameManager _gameManager;
		internal static GameManager Game{get{return s_instance._gameManager;}}
		
		private FFMultiscreenManager _multiscreenManager;
		internal static FFMultiscreenManager MultiScreen{get{return s_instance._multiscreenManager;}}
		#endregion
		
		internal FFEngine()
		{
			s_instance = this;
			_gameManager = new GameManager();
			_uiManager = new UIManager();
			_eventManager = new EventManager();
			_networkManager = new FFNetworkManager();
			_inputManager = new InputManager();
			_multiscreenManager = new FFMultiscreenManager();
			
#if UNITY_iOS && !UNITY_EDITOR
			iOSBackgroundTask.ios_registerForPushNotification();
#endif
		}
		
		internal void OnApplicationPause(bool a_isPause)
		{
			if(a_isPause)
			{
#if UNITY_iOS && !UNITY_EDITOR
				iOSBackgroundTask.ios_startBackgroundTask("Please kill me", "I don't wanna live anymore.", "Save me", 20);
#endif
			}
		}
		
		// Update is called once per frame
		internal void DoUpdate ()
		{
			if(_inputManager != null)
				_inputManager.DoUpdate();
			if(_networkManager != null)
				_networkManager.DoUpdate();
		}
		
		internal void Destroy()
		{
			_networkManager.Destroy();
			s_instance = null;
		}
	}
}