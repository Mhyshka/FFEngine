using UnityEngine;
using System.Collections;

public class PlayerManager
{
	protected Player _main = null;
	internal Player Main
	{
		get
		{
			 return _main;
		}
	}
	
	internal void RegisterMainPlayer(Player a_player)
	{
		if(_main == null)
			_main = a_player;
		else
			Debug.LogError("Multiple main player registered : " + _main.gameObject.name + " & " + a_player.gameObject.name);
	}
}
