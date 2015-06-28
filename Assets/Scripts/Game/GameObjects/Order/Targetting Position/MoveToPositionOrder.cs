using UnityEngine;
using System.Collections;

public class MoveToPositionOrder : PositionOrder
{
	public enum EType
	{
		Move,
		Look
	}
	
	protected EType _type;
	internal EType Type
	{
		get
		{
			return _type;
		}
	}
	
	internal MoveToPositionOrder(Vector3 a_position, EType a_type) : base (a_position)
	{
		_type = a_type;
	}
}