using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace FF
{
	public class EntryPoint : MonoBehaviour
	{
		public string mainScene = "MenuGameMode";
		
		private Engine _engine;
	
		// Use this for initialization
		void Awake()
		{
			_engine = new Engine();
			SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync("MultiplayerLoadingScreen", LoadSceneMode.Additive);
            Engine.Game.RequestGameMode(mainScene);

#if UNITY_ANDROID
            Application.targetFrameRate = 200;
#endif
        }

        void Start()
        {
            _engine.DoStart();
        }
		
		void Update()
		{
			_engine.DoUpdate();
		}

        void FixedUpdate()
        {
            _engine.DoFixedUpdate();
        }
		
		void OnApplicationQuit()
		{
			FFLog.LogError("Quit");
		}
		
		void OnDestroy()
		{
			FFLog.LogError("Destroy EP");
			_engine.TearDown();
			_engine = null;
		}
		
		void OnApplicationPause(bool a_isPause)
		{
			_engine.OnApplicationPause(a_isPause);
		}
	}
}