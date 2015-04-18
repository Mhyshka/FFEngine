using UnityEngine;
using System.Collections;

/*internal enum EInputKeyAction
{
	Button,
	Key
}*/

internal abstract class AInputEvent
{
	internal delegate void InputAction();
	
	#region Properties
	internal string eventKeyName;
	internal InputAction onActivation = null;
	#endregion
	
	internal virtual void DoUpdate(){}
	
	internal AInputEvent(EInputEventKey a_eventKey) : this(a_eventKey.ToString())
	{
	}
	
	internal AInputEvent(string a_eventKeyName)
	{
		eventKeyName = a_eventKeyName;
		if(!FFEngine.Inputs.keys.ContainsKey(eventKeyName))
			FFEngine.Inputs.keys.Add(eventKeyName,this);
		else
			FFEngine.Inputs.keys[eventKeyName] = this;
	}
}
