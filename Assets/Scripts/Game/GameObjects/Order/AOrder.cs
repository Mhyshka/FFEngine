using UnityEngine;
using System.Collections;



public abstract class AOrder
{
	internal enum EOrderType
	{
		None = 0,
		Attack = 1,
		Movement = 2,
		Interaction = 4,
		Stop = 8
	}
	
	internal virtual EOrderType OrderType
	{
		get
		{
			return EOrderType.None;
		}
	}

	internal virtual bool IsInstant
	{
		get
		{
			return false;
		}
	}
}
