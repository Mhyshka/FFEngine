using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

internal class InputReference
{
	internal AInputEvent input;
	internal EInputTriggerType trigger;
	
	internal InputReference(AInputEvent a_input, EInputTriggerType a_eventType)
	{
		trigger = a_eventType;
		input = a_input;
	}
}

internal class InputManager
{
	#region Properties
	internal Dictionary<string, AInputEvent> keys;
	internal Dictionary<string, AInputSwitch> switchs;
	internal Dictionary<string, AInputAxis> axis;
	
	protected bool _isPollingKey = false;
	protected bool _isPollingAxis = false;
	#endregion

	#region Methods
	#endregion

	
	internal InputManager()
	{
		keys = new Dictionary<string, AInputEvent>();
		switchs = new Dictionary<string, AInputSwitch>();
		axis = new Dictionary<string, AInputAxis>();
	}
	
	internal void DoUpdate ()
	{
		foreach (AInputEvent each in keys.Values)
		{
			each.DoUpdate();
		}
		foreach (AInputAxis each in axis.Values)
		{
			each.DoUpdate();
		}
		
		if(_isPollingKey)
		{
			InputKeyBinding binding = InputKeyBinding.Current;
			if(binding != null)
			{
				FFEventParameter args = new FFEventParameter();
				args.data = binding;
				FFEngine.Events.FireEvent("InputKeyDetected", args);
				_isPollingKey = false;
			}
		}
		
		if(_isPollingAxis)
		{
			InputControllerAxisBinding binding = InputControllerAxisBinding.Current;
			if(binding != null)
			{
				FFEventParameter args = new FFEventParameter();
				args.data = binding;
				FFEngine.Events.FireEvent("InputAxisDetected", args);
				_isPollingAxis = false;
			}
		}
		
	}
	
	internal void StartKeyPoll()
	{
		_isPollingKey = true;
	}
	
	internal void StartAxisPoll()
	{
		_isPollingAxis = true;
	}
}