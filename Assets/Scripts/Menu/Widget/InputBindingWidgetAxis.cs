using UnityEngine;
using System.Collections;

internal class InputBindingWidgetAxis : InputBindingWidget
{
	#region Inspector Properties
	public EInputAxisKey key = EInputAxisKey.Horizontal;
	
	public InputControllerAxisBinding 	 binding = null;
	
	public SimpleButton  				 axisButton = null,
										 throttleButton = null,
										 invertedButton = null;
						
	public UISlider 	 				 valueSlider = null;
	
	public UILabel						 invertedLabel = null,
										 throttleLabel = null;
										 
	#endregion
	
	#region Properties
	private bool isRegistered = false;
	private InputControllerAxis _inputAxis = null;
	#endregion
	
	#region Methods
	protected virtual void Awake ()
	{
		axisButton.callback += ModifyKeyCode;
		invertedButton.callback += ToggleIsInverted;
		throttleButton.callback += ToggleThrottle;
		
		_inputAxis = new InputControllerAxis(key, binding);
		
		UpdateDisplay();
	}
	
	protected virtual void OnDestroy()
	{
		axisButton.callback -= ModifyKeyCode;
		invertedButton.callback -= ToggleIsInverted;
		throttleButton.callback -= ToggleThrottle;
		
		if(isRegistered)
			FFEngine.Events.UnregisterForEvent("InputAxisDetected", OnInputDetected);
	}
	
	protected virtual void Update()
	{
		if(_inputAxis != null)
		{
			valueSlider.value = (1f + _inputAxis.Value) / 2f;
		}
	}
	#endregion

	#region Settings methods
	internal void ModifyKeyCode()
	{
		FFEventParameter args = new FFEventParameter();
		args.data = this;
		FFEngine.Events.FireEvent("RequestAxisPoll",args);
		
		FFEngine.Events.RegisterForEvent("InputAxisDetected", OnInputDetected);
		isRegistered = true;
	}
	
	internal void OnInputDetected(FFEventParameter args)
	{
		FFEngine.Events.UnregisterForEvent("InputAxisDetected", OnInputDetected);
		isRegistered = false;
		
		binding = args.data as InputControllerAxisBinding;
		
		_inputAxis.binding = binding;
		
		UpdateDisplay();
	}
	
	internal void ToggleIsInverted()
	{
		_inputAxis.isInverted = !_inputAxis.isInverted;
		UpdateDisplay();
	}
	
	internal void ToggleThrottle()
	{
		if(!(_inputAxis is InputControllerThrottleAxis))
			_inputAxis = new InputControllerThrottleAxis(key, 
													_inputAxis.binding,
													_inputAxis.isInverted);
		else
			_inputAxis = new InputControllerAxis(key, 
	                                             _inputAxis.binding,
	                                             _inputAxis.isInverted);
		UpdateDisplay();
	}
	
	
	internal void UpdateDisplay()
	{
		bindingLabel.text = _inputAxis.binding.ToString();
		invertedLabel.text = "Inverted : " + _inputAxis.isInverted.ToString();
		if(_inputAxis is InputControllerThrottleAxis)
			throttleLabel.text = "Type : Throttle";
		else
			throttleLabel.text = "Type : Joystick";
	}
	#endregion
	
	/*
	switch(mode)
		{
			case EInputKeyMode.Key : 
			break;
			
			case EInputKeyMode.Switch : 
				InputEventKey keyEvent = new InputEventKey(key.ToString()+"On", defaultKey);
				InputReference reference = new InputReference(keyEvent,EInputTriggerType.Activation);
				inputEvent = new InputEventToggle(key, reference);
			break;
			
			case EInputKeyMode.Toggle : 
				InputEventKey keyEvent = new InputEventKey(key.ToString()+"Toggle", defaultKey);
				InputReference reference = new InputReference(keyEvent,EInputTriggerType.Activation);
				inputEvent = new InputEventToggle(key, reference);
			break;
		}
	*/
}
