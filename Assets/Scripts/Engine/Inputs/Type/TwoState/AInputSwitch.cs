using UnityEngine;
using System.Collections;

internal class AInputSwitch
{
	internal delegate void InputAction();
	
	#region Properties
	internal string eventKeyName;
	internal InputAction onActivation = null;
	internal InputAction onDeactivation = null;
	
	protected bool _isActive;
	internal bool IsActive{get{return _isActive;}}
	#endregion
	
	internal virtual void DoUpdate(){}
	
	internal AInputSwitch(EInputEventKey a_eventKey) : this(a_eventKey.ToString())
	{
	}
	
	internal AInputSwitch(string a_eventKeyName)
	{
		eventKeyName = a_eventKeyName;
		FFEngine.Inputs.switchs.Add(eventKeyName,this);
	}
}
