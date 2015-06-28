using UnityEngine;
using System.Collections;

public abstract class PositionOrder : AOrder
{
	protected Vector3 _position;
	internal Vector3 Position
	{
		get
		{
			return _position;
		}
	}
	
	internal PositionOrder (Vector3 a_position)
	{
		_position = a_position;
	}
}
