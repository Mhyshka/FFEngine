using UnityEngine;
using System.Collections;

namespace FF.Input
{
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
		Active,
		Up
	}
	
	[System.Serializable]
	internal class InputKeyBinding
	{
		public KeyCode key;
		public EKeyModifier modifier;
		
		#region IsTrigerring?
		internal bool IsTriggering(EInputTriggerType a_triggerType)
		{
			if(IsModifierTriggered())
			{
				if(a_triggerType == EInputTriggerType.Down)
					return UnityEngine.Input.GetKeyDown(key);
				else if(a_triggerType == EInputTriggerType.Active)
					return UnityEngine.Input.GetKey(key);
				else if(a_triggerType == EInputTriggerType.Up)
					return UnityEngine.Input.GetKeyUp(key);
			}
			return false;
		}
		
		internal bool IsModifierTriggered()
		{	
			switch(modifier)
			{
			case EKeyModifier.None : return true;
				
			case EKeyModifier.LeftShift : return UnityEngine.Input.GetKey(KeyCode.LeftShift);
				
			case EKeyModifier.LeftAlt : return UnityEngine.Input.GetKey(KeyCode.LeftAlt);
				
			case EKeyModifier.LeftCtrl : return UnityEngine.Input.GetKey(KeyCode.LeftControl);
				
			case EKeyModifier.RightShift : return UnityEngine.Input.GetKey(KeyCode.RightShift);
				
			case EKeyModifier.RightAlt : return UnityEngine.Input.GetKey(KeyCode.RightAlt);
				
			case EKeyModifier.RightCtrl : return UnityEngine.Input.GetKey(KeyCode.RightControl);
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
				if(UnityEngine.Input.anyKeyDown)
				{
					foreach(KeyCode each in System.Enum.GetValues(typeof(KeyCode)))
					{
						if(UnityEngine.Input.GetKeyDown(each) && !IsModifier(each))
						{
							binding = new InputKeyBinding();
							binding.key = each;
							binding.modifier = CurrentModifier;
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
				
				if(UnityEngine.Input.GetKey(KeyCode.LeftAlt) || UnityEngine.Input.GetKeyDown(KeyCode.LeftAlt))
					modifier = EKeyModifier.LeftAlt;
					
				else if(UnityEngine.Input.GetKey(KeyCode.LeftShift) || UnityEngine.Input.GetKeyDown(KeyCode.LeftShift))
					modifier = EKeyModifier.LeftShift;
					
				else if(UnityEngine.Input.GetKey(KeyCode.LeftControl) || UnityEngine.Input.GetKeyDown(KeyCode.LeftControl))
					modifier = EKeyModifier.LeftCtrl;
					
				else if(UnityEngine.Input.GetKey(KeyCode.RightAlt) || UnityEngine.Input.GetKeyDown(KeyCode.RightAlt))
					modifier = EKeyModifier.RightAlt;
					
				else if(UnityEngine.Input.GetKey(KeyCode.RightShift) || UnityEngine.Input.GetKeyDown(KeyCode.RightShift))
					modifier = EKeyModifier.RightShift;
					
				else if(UnityEngine.Input.GetKey(KeyCode.RightControl) || UnityEngine.Input.GetKeyDown(KeyCode.RightControl))
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
}
