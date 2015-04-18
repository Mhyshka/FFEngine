using UnityEngine;
using System.Collections;
using System.Collections.Generic;

internal class LevelRegisterer : MonoBehaviour
{
	public List<GameLevelData> levels = null;
	// Use this for initialization
	void Awake ()
	{
		FFEngine.Game.levels = levels;
	}
}
