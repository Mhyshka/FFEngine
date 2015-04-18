using UnityEngine;
using System.Collections;

internal class InputEventKey : AInputEvent
{
	#region properties
	protected InputKeyBinding _binding;
	internal InputKeyBinding Binding{get{return _binding;}}
	#endregion
	
	internal InputEventKey(EInputEventKey a_eventKey, InputKeyBinding a_binding) : this(a_eventKey.ToString(),a_binding)
	{
	}
	
	internal InputEventKey(string a_eventKeyName, InputKeyBinding a_binding) : base(a_eventKeyName)
	{
		_binding = a_binding;
	}
	
	#region Input Methods
	internal override void DoUpdate ()
	{
		base.DoUpdate ();
		if (_binding.IsTriggering())
		{
			if(onActivation != null)
				onActivation.Invoke();
		}
	}
	#endregion
}
