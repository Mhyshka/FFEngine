using UnityEngine;
using System.Collections;

internal enum EKeyModifier
{
	None,
	LeftShift,
	LeftCtrl,
	LeftAlt,
	RightShift,
	RightCtrl,
	RightAlt
}

internal enum EInputTriggerType
{
	Down,
	Pressed,
	Up
}

[System.Serializable]
internal class InputKeyBinding
{
	public KeyCode key;
	public EKeyModifier modifier;
	public EInputTriggerType trigger;
	
	#region IsTrigerring?
	internal bool IsTriggering()
	{
		if(IsModifierTriggered())
		{
			if(trigger == EInputTriggerType.Down)
				return Input.GetKeyDown(key);
			else if(trigger == EInputTriggerType.Pressed)
				return Input.GetKey(key);
			else if(trigger == EInputTriggerType.Up)
				return Input.GetKeyUp(key);
		}
		return false;
	}
	
	internal bool IsModifierTriggered()
	{
		switch(modifier)
		{
			case EKeyModifier.None : return true;
			
			case EKeyModifier.LeftShift : return Input.GetKey(KeyCode.LeftShift);
			
			case EKeyModifier.LeftAlt : return Input.GetKey(KeyCode.LeftAlt);
			
			case EKeyModifier.LeftCtrl : return Input.GetKey(KeyCode.LeftControl);
			
			case EKeyModifier.RightShift : return Input.GetKey(KeyCode.RightShift);
			
			case EKeyModifier.RightAlt : return Input.GetKey(KeyCode.RightAlt);
			
			case EKeyModifier.RightCtrl : return Input.GetKey(KeyCode.RightControl);
		}
		
		return false;
	}
	#endregion
	
	public override string ToString ()
	{
		string str = "";
		if(modifier != EKeyModifier.None)
		{
			str += "<" + modifier.ToString() + "> + ";
		}
		str += "<"+key.ToString()+">";
		
		return str;
	}
	
	#region Poll
	internal static InputKeyBinding Current
	{
		get
		{
			InputKeyBinding binding = null;
			if(Input.anyKeyDown)
			{
				foreach(KeyCode each in System.Enum.GetValues(typeof(KeyCode)))
				{
					if(Input.GetKeyDown(each) && !IsModifier(each))
					{
						binding = new InputKeyBinding();
						binding.key = each;
						binding.modifier = CurrentModifier;
						binding.trigger = EInputTriggerType.Down;
						break;
					}
				}
			}
			
			return binding;
		}
	}
	
	internal static bool IsModifier(KeyCode a_eventKey)
	{
		return  a_eventKey == KeyCode.LeftAlt || 
				a_eventKey == KeyCode.LeftShift || 
				a_eventKey == KeyCode.LeftControl || 
				a_eventKey == KeyCode.RightAlt || 
				a_eventKey == KeyCode.RightShift || 
				a_eventKey == KeyCode.RightControl;
	}
	
	internal static EKeyModifier CurrentModifier
	{
		get
		{
			EKeyModifier modifier = EKeyModifier.None;
			
			if(Input.GetKey(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.LeftAlt))
				modifier = EKeyModifier.LeftAlt;
				
			else if(Input.GetKey(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.LeftShift))
				modifier = EKeyModifier.LeftShift;
				
			else if(Input.GetKey(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.LeftControl))
				modifier = EKeyModifier.LeftCtrl;
				
			else if(Input.GetKey(KeyCode.RightAlt) || Input.GetKeyDown(KeyCode.RightAlt))
				modifier = EKeyModifier.RightAlt;
				
			else if(Input.GetKey(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.RightShift))
				modifier = EKeyModifier.RightShift;
				
			else if(Input.GetKey(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.RightControl))
				modifier = EKeyModifier.RightCtrl;
			
			return modifier;
		}
	}
	
	internal static bool IsEscapePressed()
	{
		return Current != null && Current.key == KeyCode.Escape;
	}
	#endregion
}
