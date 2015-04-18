using UnityEngine;
using System.Collections;

internal class FFEngine
{	
	#region Properties
	private static FFEngine s_instance;
	
	private EventManager _eventManager;
	internal static EventManager Events{get{return s_instance._eventManager;}}
	
	private UIManager _uiManager;
	internal static UIManager UI{get{return s_instance._uiManager;}}
	
	private InputManager _inputManager;
	internal static InputManager Inputs{get{return s_instance._inputManager;}}
	
	private GameManager _gameManager;
	internal static GameManager Game{get{return s_instance._gameManager;}}
	#endregion
	
	internal FFEngine()
	{
		s_instance = this;
		_gameManager = new GameManager();
		_uiManager = new UIManager();
		_eventManager = new EventManager();
		_inputManager = new InputManager();
	}

	
	// Update is called once per frame
	internal void DoUpdate ()
	{
		_inputManager.DoUpdate();
	}
}