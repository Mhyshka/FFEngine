using UnityEngine;
using System.Collections;
using System;

internal class TimerWidget : MonoBehaviour
{
	#region Inspector Properties
	public UILabel milliLabel = null;
	public UILabel secondsLabel = null;
	public UILabel minutesLabel = null;
	#endregion

	#region Properties
	private float _timeRemaining = 0f;
	internal float TimeRemaining
	{
		get
		{
			return _timeRemaining;
		}
	}
	#endregion

	void Update ()
	{
		if (_timeRemaining > 0f)
		{
			_timeRemaining -= Time.deltaTime;
			if(_timeRemaining <= 0f)
			{
				FFEngine.Events.FireEvent("TimesUp");
				_timeRemaining = 0f;
			}
			DisplayValue();
		}
	}

	internal void SetTimer(float a_value)
	{
		_timeRemaining = a_value;
		DisplayValue ();
	}

	private void DisplayValue()
	{
		TimeSpan span = TimeSpan.FromSeconds (_timeRemaining);
		milliLabel.text = (span.Milliseconds / 10).ToString ("00") + "\""; 
		secondsLabel.text = span.Seconds.ToString ("00") + "'"; 
	}
}
