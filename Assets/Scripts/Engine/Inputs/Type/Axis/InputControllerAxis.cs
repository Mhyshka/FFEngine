using UnityEngine;
using System.Collections;

internal class InputControllerAxis : AInputAxis
{
	#region Properties
	internal InputControllerAxisBinding binding = null;
	internal float deadZone = 0.05f;
	
	internal bool isInverted;
	
	internal float cropBorder = 0.075f;
	#endregion
	internal InputControllerAxis(EInputAxisKey a_eventKey, 
	                             InputControllerAxisBinding a_binding,
	                             bool a_isInverted = false,
	                             float a_deadZone = 0.05f) : this(a_eventKey.ToString(), a_binding, a_isInverted, a_deadZone)
	{
	}
	
	internal InputControllerAxis(string a_eventKeyName, 
	                             InputControllerAxisBinding a_binding,
	                             bool a_isInverted = false,
	                             float a_deadZone = 0.05f) : base(a_eventKeyName)
	{
		binding = a_binding;
		isInverted = a_isInverted;
		deadZone = Mathf.Clamp01(a_deadZone);
	}
	
	internal override float Value
	{
		get
		{
			float lvalue = binding.Value;
				
			lvalue = FFUtils.Rerange(lvalue, new Vector2(-1f + cropBorder, 1f - cropBorder), new Vector2(-1f  , 1f));
			
			if(isInverted)
				lvalue = -lvalue;
				
			/*lvalue = Mathf.MoveTowards(lvalue, 0f, deadZone);
			lvalue /= (1f - deadZone);*/
			return lvalue;
		}
	}
	
	internal override void DoUpdate ()
	{
		base.DoUpdate ();
	}
	
}
