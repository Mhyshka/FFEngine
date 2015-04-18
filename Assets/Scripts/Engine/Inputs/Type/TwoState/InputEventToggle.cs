using UnityEngine;
using System.Collections;

internal class InputEventToggle : AInputSwitch
{
	#region properties
	internal InputEventKey toggleKey;
	#endregion

	internal InputEventToggle(EInputEventKey a_eventKey, InputEventKey a_input, bool a_state = false) : this(a_eventKey.ToString(), a_input, a_state)
	{
	}
	
	internal InputEventToggle(string a_eventKeyName, InputEventKey a_input, bool a_state = false) : base(a_eventKeyName)
	{
		_isActive = a_state;
		
		toggleKey = a_input;
		toggleKey.onActivation += OnToggle;
	}
	
	~InputEventToggle()
	{
		toggleKey.onActivation -= OnToggle;
	}

	#region Input Methods
	internal virtual void OnToggle()
	{
		_isActive = !_isActive;
		if (IsActive && onActivation != null)
			onActivation.Invoke ();
		else if(onDeactivation != null)
			onDeactivation.Invoke();
	}
	#endregion
}