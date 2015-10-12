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

        protected Stack<SimpleCallback> _backCallbacksStack = null;
		
		protected bool _isPollingKey = false;
		protected bool _isPollingAxis = false;
        #endregion

        internal InputManager()
        {
            keys = new Dictionary<string, AInputEvent>();
            switchs = new Dictionary<string, AInputSwitch>();
            axis = new Dictionary<string, AInputAxis>();
            _backCallbacksStack = new Stack<SimpleCallback>();

            FFEngine.Events.RegisterForEvent(EEventType.Back, OnBackEvent);
        }

        ~InputManager()
        {
            FFEngine.Events.UnregisterForEvent(EEventType.Back, OnBackEvent);
        }

        #region Engine
        internal void DoUpdate ()
		{
			if(UnityEngine.Input.GetKeyDown(KeyCode.Escape))
			{
                HandleOnBackPressed();
			}
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
				KeyPoll();
			}
			
			if(_isPollingAxis)
			{
				AxisPoll();
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

        #region Polling
        internal void StartKeyPoll()
        {
            _isPollingKey = true;
        }

        protected void KeyPoll()
        {
            InputKeyBinding binding = InputKeyBinding.Current;
            if (binding != null)
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
            if (InputKeyBinding.IsEscapePressed())
            {
                FFEngine.Events.FireEvent("InputAxisDetected", null);
            }
            else
            {
                InputControllerAxisBinding binding = InputControllerAxisBinding.Current;
                if (binding != null)
                {
                    FFEventParameter args = new FFEventParameter();
                    args.data = binding;
                    FFEngine.Events.FireEvent("InputAxisDetected", args);
                    _isPollingAxis = false;
                }
            }
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
				int length = UnityEngine.Input.GetJoystickNames().Length;
				return length > 0;
			}
		}
		
		internal bool ShouldUseNavigation
		{
			get
			{
				return FFEngine.MultiScreen.IsTV || HasJoystickConnected;
			}
		}
        #endregion

        #region Back
        protected void OnBackEvent(FFEventParameter a_args)
        {
            HandleOnBackPressed();
        }

        protected void HandleOnBackPressed()
        {
            if (_backCallbacksStack.Count > 0)
            {
                SimpleCallback curCallback = _backCallbacksStack.Peek();
                if (curCallback != null)
                    curCallback();
            }
        }

        internal void PushOnBackCallback(SimpleCallback a_callback)
        {
            _backCallbacksStack.Push(a_callback);
        }

        internal void PopOnBackCallback()
        {
            _backCallbacksStack.Pop();
        }
        #endregion
    }
}