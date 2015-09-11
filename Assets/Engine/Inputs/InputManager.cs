using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using FF;

namespace FF.Input
{	
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
	
		#region Engine
		internal InputManager()
		{
			keys = new Dictionary<string, AInputEvent>();
			switchs = new Dictionary<string, AInputSwitch>();
			axis = new Dictionary<string, AInputAxis>();
		}
		
		internal void DoUpdate ()
		{
			/*foreach (AInputEvent each in keys.Values)
			{
				each.DoUpdate();
			}
			foreach (AInputAxis each in axis.Values)
			{
				each.DoUpdate();
			}
			
			if(_isPollingKey)
			{
				KeyPoll();
			}
			
			if(_isPollingAxis)
			{
				AxisPoll();
			}*/
		}
		
		internal void StartKeyPoll()
		{
			_isPollingKey = true;
		}
		
		protected void KeyPoll()
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
		
		internal void StartAxisPoll()
		{
			_isPollingAxis = true;
		}
		
		protected void AxisPoll()
		{
			if(InputKeyBinding.IsEscapePressed())
			{
				FFEngine.Events.FireEvent("InputAxisDetected", null);
			}
			else
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
		
		internal void Register(AInputEvent a_input)
		{
			keys.Add(a_input.eventName, a_input);
		}
		
		internal void Register(AInputAxis a_input)
		{
			axis.Add(a_input.eventName, a_input);
		}
		#endregion
	
		#region EventListening
		internal AInputEvent GetInputKey(EInputEventKey a_key)
		{
			return GetInputKey(a_key.ToString());
		}
		
		internal AInputEvent GetInputKey(string a_keyName)
		{
			if(keys.ContainsKey(a_keyName))
			{
				return keys[a_keyName];
			}
			return null;
		}
		#endregion
		
		#region Joystick
		internal bool HasJoystickConnected
		{
			get
			{
				return true;
				int length = UnityEngine.Input.GetJoystickNames().Length;
				FFLog.LogError("Count : " + length.ToString());
				return length > 0;
			}
		}
		#endregion
	}
}