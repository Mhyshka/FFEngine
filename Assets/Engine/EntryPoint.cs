using UnityEngine;
using System.Collections;

namespace FF
{
	public class EntryPoint : MonoBehaviour
	{
		public string mainScene = "MenuGameMode";
		
		private FFEngine _engine;
	
		// Use this for initialization
		void Awake()
		{
			_engine = new FFEngine();
			Application.LoadLevelAdditive("UI");
			FFEngine.Game.RequestGameMode(mainScene);
		}
		
		void Update()
		{
			_engine.DoUpdate();
		}
		
		void OnApplicationQuit()
		{
			FFLog.LogError("Quit");
		}
		
		void OnDestroy()
		{
			FFLog.LogError("Destroy EP");
			_engine.Destroy();
			_engine = null;
		}
	}
}