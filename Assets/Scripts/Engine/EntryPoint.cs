using UnityEngine;
using System.Collections;

internal class EntryPoint : MonoBehaviour
{
	public string levelToLoad = "Menu";
	
	private FFEngine _engine;
	
	void Awake()
	{
		_engine = new FFEngine();
		Application.LoadLevel("UI");
		Application.LoadLevelAsync(levelToLoad);
	}
	
	void Update()
	{
		_engine.DoUpdate();
	}
}