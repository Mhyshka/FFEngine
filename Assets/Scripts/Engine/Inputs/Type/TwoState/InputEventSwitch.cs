using UnityEngine;
using System.Collections;

internal class InputEventSwitch : AInputSwitch
{
	#region properties
	internal InputEventKey toggleOn;
	internal InputEventKey toggleOff;
	#endregion

	#region Constructor & Destructor
	internal InputEventSwitch(EInputEventKey a_eventKey, InputEventKey a_inputOn, InputEventKey a_inputOff, bool a_state = false) : this(a_eventKey.ToString(), a_inputOn, a_inputOff, a_state)
	{
	}
	
	internal InputEventSwitch(string a_eventKeyName, InputEventKey a_inputOn, InputEventKey a_inputOff, bool a_state = false) : base(a_eventKeyName)
	{
		_isActive = a_state;
		
		toggleOn = a_inputOn;
		toggleOn.onActivation += OnToggleOn;
		
		toggleOff = a_inputOff;
		toggleOff.onActivation += OnToggleOff;
	}
	
	~InputEventSwitch()
	{
		toggleOn.onActivation -= OnToggleOn;
		toggleOff.onActivation -= OnToggleOff;
	}
	#endregion

	#region Input Methods
	internal void OnToggleOn ()
	{
		_isActive = true;
		if(onActivation != null)
			onActivation.Invoke();
	}

	internal void OnToggleOff ()
	{
		_isActive = false;
		if(onDeactivation != null)
			onDeactivation.Invoke();
	}
	#endregion
}