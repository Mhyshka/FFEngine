using UnityEngine;
using System.Collections;

internal abstract class AInputAxis
{
	#region Properties
	internal string bindingName;
	internal abstract float Value{get;}
	#endregion
	
	internal AInputAxis(EInputAxisKey a_eventKey,
							bool a_isInverted = false) : this(a_eventKey.ToString(), a_isInverted)
	{
	}
	
	internal AInputAxis(string a_eventKeyName,
	                    bool a_isInverted = false)
	{
		bindingName = a_eventKeyName;
		if(!FFEngine.Inputs.axis.ContainsKey(bindingName))
			FFEngine.Inputs.axis.Add(bindingName,this);
		else
			FFEngine.Inputs.axis[bindingName] = this;
	}
	
	internal bool IsTriggering()
	{
		return Mathf.Abs(Value) > 0.4f;
	}
	
	internal virtual void DoUpdate (){}
}