using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using FF.Logic;
using FF.UI;
using FF.Handler;
using FF.Network;
using FF.Network.Receiver;
using FF.Input;
using FF.Multiscreen;

namespace FF
{
	internal class Engine : BaseManager
	{	
		#region Properties
		private static Engine s_instance;
		internal static Engine Instance
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
		
		private ServerInputManager _inputManager;
		internal static ServerInputManager Inputs {get{return s_instance._inputManager;}}
		
		private NetworkStatusManager _networkStatusManager;
		internal static NetworkStatusManager NetworkStatus{get{return s_instance._networkStatusManager;}}
		
		private NetworkManager _networkManager;
		internal static NetworkManager Network{get{return s_instance._networkManager;}}

        private HandlerManager _handlerManager;
        internal static HandlerManager Handler { get { return s_instance._handlerManager; } }

        private ReceiverManager _receiverManager;
        internal static ReceiverManager Receiver { get { return s_instance._receiverManager; } }

        private NetworkGameManager _gameManager;
		internal static NetworkGameManager Game{get{return s_instance._gameManager;}}
		
		private MultiscreenManager _multiscreenManager;
		internal static MultiscreenManager MultiScreen{get{return s_instance._multiscreenManager;}}


        protected List<BaseManager> _managers;
		#endregion
		
		internal Engine()
		{
            s_instance = this;
            _managers = new List<BaseManager>();

            _gameManager = new NetworkGameManager();
            _managers.Add(_gameManager);

			_uiManager = new UIManager();
            _managers.Add(_uiManager);

            _eventManager = new EventManager();
            _managers.Add(_eventManager);

            _networkManager = new NetworkManager();
            _managers.Add(_networkManager);

            _inputManager = new ServerInputManager(true);
            _managers.Add(_inputManager);

            _multiscreenManager = new MultiscreenManager();
            _managers.Add(_multiscreenManager);

            _networkStatusManager = new NetworkStatusManager();
            _managers.Add(_networkStatusManager);

            _handlerManager = new HandlerManager();
            _managers.Add(_handlerManager);

            _receiverManager = new ReceiverManager();
            _managers.Add(_receiverManager);

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

        #region Manager methoods
        internal override void DoStart()
        {
            foreach (BaseManager each in _managers)
            {
                if (each != null)
                    each.DoStart();
            }
        }

        // Update is called once per frame
        internal override void DoUpdate ()
		{
            foreach (BaseManager each in _managers)
            {
                if(each != null)
                    each.DoUpdate();
            }
        }

        internal override void DoFixedUpdate()
        {
            foreach (BaseManager each in _managers)
            {
                if (each != null)
                    each.DoFixedUpdate();
            }
        }
		
		internal override void TearDown()
		{
            foreach (BaseManager each in _managers)
            {
                if (each != null)
                    each.TearDown();
            }
            s_instance = null;
		}
        #endregion
    }
}