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
	
	internal class InputManager : BaseManager
	{
		#region Properties
		protected Dictionary<string, AInputEvent> _keys;
        protected Dictionary<string, AInputSwitch> _switchs;
        protected Dictionary<string, AInputAxis> _axis;

        protected Stack<SimpleCallback> _backCallbacksStack = null;
		
		protected bool _isPollingKey = false;
		protected bool _isPollingAxis = false;
        #endregion

        #region Manager
        internal InputManager(bool a_registerForBack = false)
        {
            _keys = new Dictionary<string, AInputEvent>();
            _switchs = new Dictionary<string, AInputSwitch>();
            _axis = new Dictionary<string, AInputAxis>();
            _backCallbacksStack = new Stack<SimpleCallback>();

            SetupDefaultEvents();

            if (a_registerForBack)
                Engine.Events.RegisterForEvent(FFEventType.Back, OnBackEvent);
        }

        internal override void DoUpdate()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                HandleOnBackPressed();
            }
            foreach (AInputEvent each in _keys.Values)
            {
                each.DoUpdate();
            }
            foreach (AInputAxis each in _axis.Values)
            {
                each.DoUpdate();
            }

            if (_isPollingKey)
            {
                KeyPoll();
            }

            if (_isPollingAxis)
            {
                AxisPoll();
            }
        }

        internal override void TearDown()
        {
            Engine.Events.UnregisterForEvent(FFEventType.Back, OnBackEvent);
        }
        #endregion

        #region Register
        internal void RegisterInputEvent(AInputEvent a_input)
		{
            if (!_keys.ContainsKey(a_input.eventKeyName))
                _keys.Add(a_input.eventKeyName, a_input);
            else
                _keys[a_input.eventKeyName] = a_input;
		}
		
		internal void RegisterInputAxis(AInputAxis a_input)
		{
            if (!_axis.ContainsKey(a_input.eventKeyName))
                _axis.Add(a_input.eventKeyName, a_input);
            else
                _axis[a_input.eventKeyName] = a_input;
		}

        internal void RegisterInputSwitch(AInputSwitch a_input)
        {
            if (!_switchs.ContainsKey(a_input.eventKeyName))
                _switchs.Add(a_input.eventKeyName, a_input);
            else
                _switchs[a_input.eventKeyName] = a_input;
        }
        #endregion

        #region Get
        internal AInputEvent EventForKey(EInputEventKey a_inputKey)
        {
            return EventForKey(a_inputKey.ToString());
        }

        internal AInputEvent EventForKey(string a_key)
        {
            AInputEvent inputEvent = null;
            _keys.TryGetValue(a_key, out inputEvent);
            return inputEvent;
        }

        internal AInputAxis AxisForKey(EInputAxisKey a_inputKey)
        {
            return AxisForKey(a_inputKey.ToString());
        }

        internal AInputAxis AxisForKey(string a_key)
        {
            AInputAxis inputAxis = null;
            _axis.TryGetValue(a_key, out inputAxis);
            return inputAxis;
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
                Engine.Events.FireEvent("InputKeyDetected", args);
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
                Engine.Events.FireEvent("InputAxisDetected", null);
            }
            else
            {
                InputControllerAxisBinding binding = InputControllerAxisBinding.Current;
                if (binding != null)
                {
                    FFEventParameter args = new FFEventParameter();
                    args.data = binding;
                    Engine.Events.FireEvent("InputAxisDetected", args);
                    _isPollingAxis = false;
                }
            }
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
				return Engine.MultiScreen.IsTV || HasJoystickConnected;
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

        protected virtual void SetupDefaultEvents()
        {
            //KeyCode
            RegisterInputEvent(new InputEventKey(EInputEventKey.Up,
                                                new InputKeyBinding(KeyCode.UpArrow)));
            RegisterInputEvent(new InputEventKey(EInputEventKey.Down,
                                                new InputKeyBinding(KeyCode.DownArrow)));
            RegisterInputEvent(new InputEventKey(EInputEventKey.Left,
                                                new InputKeyBinding(KeyCode.LeftArrow)));
            RegisterInputEvent(new InputEventKey(EInputEventKey.Right,
                                                new InputKeyBinding(KeyCode.RightArrow)));
            RegisterInputEvent(new InputEventKey(EInputEventKey.Back,
                                                new InputKeyBinding(KeyCode.Escape)));
            RegisterInputEvent(new InputEventKey(EInputEventKey.Submit,
                                                new InputKeyBinding(KeyCode.Return)));
            RegisterInputEvent(new InputEventKey(EInputEventKey.Action,
                                                new InputKeyBinding(KeyCode.Return)));
        }
    }
}