using UnityEngine;
using System.Collections;

namespace FFEngine
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
	}
}