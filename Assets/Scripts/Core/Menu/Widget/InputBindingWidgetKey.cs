using UnityEngine;
using System.Collections;

internal class InputBindingWidgetKey : InputBindingWidget
{
	#region Inspector Properties
	public UILabel triggerModeLabel = null;
	
	public EInputEventKey key = EInputEventKey.Select;
	
	public InputKeyBinding binding = null;
	
	public SimpleButton keycodebutton = null,
						triggerModeButton = null;
						
	public UISprite 	check = null;
	#endregion
	
	#region Properties
	internal InputEventKey _inputKey;
	private bool isRegistered = false;
	#endregion
	
	#region Methods
	protected virtual void Awake ()
	{
		Debug.Log(name + " : " + key.ToString());
		keycodebutton.callback += ModifyKeyCode;
		triggerModeButton.callback += ToggleActivationMode;
		
		_inputKey = new InputEventKey(key,binding);
		
		UpdateDisplay();
	}
	
	protected virtual void OnDestroy()
	{
		keycodebutton.callback -= ModifyKeyCode;
		triggerModeButton.callback -= ToggleActivationMode;
		
		if(isRegistered)
			FFEngine.Events.UnregisterForEvent("InputKeyDetected", OnInputDetected);
	}
	
	protected virtual void Update()
	{
		check.enabled = binding.IsTriggering();
	}
	#endregion

	#region Settings methods
	internal void ModifyKeyCode()
	{
		FFEventParameter args = new FFEventParameter();
		args.data = this;
		FFEngine.Events.FireEvent("RequestKeyPoll",args);
		
		FFEngine.Events.RegisterForEvent("InputKeyDetected", OnInputDetected);
		isRegistered = true;
	}
	
	internal void OnInputDetected(FFEventParameter args)
	{
		FFEngine.Events.UnregisterForEvent("InputKeyDetected", OnInputDetected);
		isRegistered = false;
		InputKeyBinding newBinding = args.data as InputKeyBinding;
		if(newBinding.key != KeyCode.Escape)
		{
			EInputTriggerType type = binding.trigger;
			binding = newBinding;
			binding.trigger = type;
			_inputKey.UpdateBinding(binding);
		}
		UpdateDisplay();
	}
	
	internal void ToggleActivationMode()
	{
		if(binding.trigger == EInputTriggerType.Down)
			binding.trigger = EInputTriggerType.Up;
		else if(binding.trigger == EInputTriggerType.Up)
			binding.trigger = EInputTriggerType.Pressed;
		else
			binding.trigger = EInputTriggerType.Down;
		UpdateDisplay();
	}
	
	internal void UpdateDisplay()
	{
		bindingLabel.text = binding.ToString();
		triggerModeLabel.text = binding.trigger.ToString();
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
