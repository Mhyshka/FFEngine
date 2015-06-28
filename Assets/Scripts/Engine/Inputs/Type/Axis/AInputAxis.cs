using UnityEngine;
using System.Collections;

internal abstract class AInputAxis
{
	#region Properties
	internal string eventName;
	internal abstract float Value{get;}
	#endregion
	
	internal AInputAxis(EInputAxisKey a_eventKey,
							bool a_isInverted = false) : this(a_eventKey.ToString(), a_isInverted)
	{
	}
	
	internal AInputAxis(string a_eventKeyName,
	                    bool a_isInverted = false)
	{
		eventName = a_eventKeyName;
		if(!FFEngine.Inputs.axis.ContainsKey(eventName))
			FFEngine.Inputs.axis.Add(eventName,this);
		else
			FFEngine.Inputs.axis[eventName] = this;
	}
	
	internal bool IsTriggering()
	{
		return Mathf.Abs(Value) > 0.4f;
	}
	
	internal virtual void DoUpdate (){}
}