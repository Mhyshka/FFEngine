using UnityEngine;
using System.Collections;

namespace FF.Input
{
	internal class InputKeyThrottleAxis : InputKeyAxis
	{
		#region Properties
		#endregion
	
		#region Constructor & Destructor
		internal InputKeyThrottleAxis(EInputAxisKey a_eventKey, 
		                              InputEventKey a_positive,
		                              InputEventKey a_negative,
		                      			float a_gravity = 0f,
		                      			float a_sensitivity = 2f) : this(a_eventKey.ToString(), a_positive, a_negative, a_gravity, a_sensitivity)
		{
		}
		
		internal InputKeyThrottleAxis(string a_eventKeyName, 
		                              InputEventKey a_positive,
		                              InputEventKey a_negative,
		                              float a_gravity = 0f,
		                              float a_sensitivity = 2f) : base(a_eventKeyName, a_positive, a_negative, a_gravity, a_sensitivity)
		{
		}
		#endregion
		
		
		#region Event
		protected override void Increase()
		{
			_currentValue += _sensitibity * Time.deltaTime;
			_currentValue = Mathf.Clamp(_currentValue, 0f, 1f);
		}
		
		protected override void Decrease()
		{
			_currentValue -= _sensitibity * Time.deltaTime;
			_currentValue = Mathf.Clamp(_currentValue, 0f, 1f);
		}
		#endregion
	}
}