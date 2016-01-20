using UnityEngine;
using System.Collections;

namespace FF.Input
{
	internal class InputKeyAxis : AInputAxis
	{
		#region Properties
		protected InputEventKey _inputPositive;
        protected InputEventKey _inputNegative;
		
		protected float _currentValue = 0f;
		protected float _sensitibity = 2f;
		protected float _gravity = 0f;
		#endregion
		
		#region Constructor & Destructor
		internal InputKeyAxis(EInputAxisKey a_eventKey, 
		                      InputEventKey a_positive,
		                      InputEventKey a_negative,
	                          float a_gravity = 0f,
	                  		  float a_sensitivity = 2f) : this(a_eventKey.ToString(), a_positive, a_negative, a_gravity, a_sensitivity)
		{
		}
		
		internal InputKeyAxis(string a_eventKeyName, 
		                      InputEventKey a_positive,
		                      InputEventKey a_negative,
		                      float a_gravity = 0f,
		                      float a_sensitivity = 2f) : base(a_eventKeyName)
		{
			_sensitibity = a_sensitivity;
			_currentValue = 0f;
			_gravity = a_gravity;
			
			_inputPositive = a_positive;
			
			_inputNegative = a_negative;
		}
		#endregion
	
		
		#region Event
		protected virtual void Increase()
		{
			_currentValue += _sensitibity * Time.deltaTime;
			_currentValue = Mathf.Clamp(_currentValue, -1f, 1f);
		}
		
		protected virtual void Decrease()
		{
			_currentValue -= _sensitibity * Time.deltaTime;
			_currentValue = Mathf.Clamp(_currentValue, -1f, 1f);
		}
		#endregion
		
		internal override float Value
		{
			get
			{
				return _currentValue;
			}
		}
		
		internal override void DoUpdate ()
		{
			base.DoUpdate ();

            if (_inputPositive.IsPressed)
                Increase();
            if (_inputNegative.IsPressed)
                Decrease();

			_currentValue = Mathf.MoveTowards(_currentValue, 0f, _gravity * Time.deltaTime);
		}
	}
}