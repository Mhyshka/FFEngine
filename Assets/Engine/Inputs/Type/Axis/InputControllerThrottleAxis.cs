using UnityEngine;
using System.Collections;

namespace FF.Input
{
	internal class InputControllerThrottleAxis : InputControllerAxis
	{
		#region Properties
		#endregion
		
		internal InputControllerThrottleAxis(EInputAxisKey a_eventKey, 
		                                     InputControllerAxisBinding a_binding,
		                                     bool a_isInverted = false,
		                                     float a_deadZone = 0.05f) : this(a_eventKey.ToString(), a_binding, a_isInverted, a_deadZone)
		{
		}
		
		internal InputControllerThrottleAxis(string a_eventKeyName, 
				                             InputControllerAxisBinding a_binding,
				                             bool a_isInverted = false,
		                                     float a_deadZone = 0.05f) : base(a_eventKeyName, a_binding, a_isInverted, a_deadZone)
		{
			_binding = a_binding;
			isInverted = a_isInverted;
			deadZone = Mathf.Clamp01(a_deadZone);
		}
		
		internal override float Value
		{
			get
			{
				float lvalue = _binding.Value;
				lvalue = FFUtils.Rerange(lvalue,
										new Vector2(-1f + cropBorder, 1f - cropBorder),
										new Vector2(0f  , 1f));
				
				if(isInverted)
					lvalue = 1f - lvalue;
					
				return lvalue;
			}
		}
	}
}