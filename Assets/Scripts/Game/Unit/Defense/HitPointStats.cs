using UnityEngine;
using System.Collections;

[System.Serializable]
public class HitPointStats : AUnitComponent
{	
	#region Inspector Properties
	public IntModified conf = null;
	[Range(0f,1f)]
	public float startPercentage = 1f;
	#endregion

	#region Properties
	internal int Base
	{
		get
		{
			return conf.baseValue;
		}
	}
	
	internal int Max
	{
		get
		{
			return conf.Value;
		}
	}
	
	internal int Current
	{
		get
		{
			return _current;
		}
	}
	
	protected int _current;
	#endregion

	#region Methods
	internal void Damage(int a_amount)
	{
		if(a_amount > 0)
		{
			//Debug.Log("Damage received : " + a_amount);
			_current -= a_amount;
			if(_current <= 0)
			{
				
			}
		}
		else if(a_amount == 0)
		{
		
		}
		else
		{
			Debug.LogWarning("Negative damage received : " + a_amount);
		}
	}
	
	internal override void Init (Unit a_unit)
	{
		base.Init (a_unit);
		_current = Mathf.Clamp(Mathf.CeilToInt(Max * startPercentage), 
								1,
								Max);
	}
	#endregion
}
