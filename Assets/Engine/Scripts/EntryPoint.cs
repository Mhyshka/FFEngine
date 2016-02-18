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
            Application.targetFrameRate = 1000;
            //Application.targetFrameRate = 300;

#if UNITY_ANDROID
            //Application.targetFrameRate = 200;
#endif
        }

        void Start()
        {
            _engine.DoStart();
        }
		
		void Update()
		{
			_engine.DoUpdate();

            _timeElapsed += Time.deltaTime;
            _frameCount++;
            if (_timeElapsed > 0.5f)
            {
                _framerate = Mathf.RoundToInt(_frameCount / _timeElapsed);

                _frameCount = 0;
                _timeElapsed = 0f;
            }
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

        protected float _timeElapsed;
        protected int _frameCount = 0;

        protected int _framerate;
        void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("FPS : " + _framerate);
            GUILayout.EndHorizontal();
        }
	}
}