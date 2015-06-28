using UnityEngine;
using System.Collections;

public class StopOrder : AOrder
{
	public enum EType
	{
		Stop,
		HoldPosition
	}
	
	#region Properties	
	protected EType _type;
	internal EType Type
	{
		get
		{
			return _type;
		}
	}
	
	internal override bool IsInstant
	{
		get
		{
			return true;
		}
	}
	#endregion
	
	
	internal StopOrder(EType a_type)
	{
		_type = a_type;
	}
}
