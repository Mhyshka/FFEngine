using UnityEngine;
using System.Collections;

public class ATargetOrder : AOrder
{
	protected Unit _target;
	
	internal Unit Target
	{
		get
		{
			return _target;
		}
	}
	
	internal ATargetOrder(Unit a_target)
	{
		_target = a_target;
	}
}
