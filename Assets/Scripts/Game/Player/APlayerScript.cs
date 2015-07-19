using UnityEngine;
using System.Collections;
using FullInspector;

public abstract class APlayerScript : FullInspector.BaseBehavior
{
	protected Player _player;
	
	internal virtual void Init(Player a_player)
	{
		_player = a_player;
	}
}
