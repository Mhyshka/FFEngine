using UnityEngine;
using System.Collections;

public class IntModifierFromEffect
{
	internal IntModifier modifier = null;
	internal EffectOverTime source = null;
}

public class IntModifier
{
	#region Properties
	internal float percent = 0f;
	internal int flat = 0;
	#endregion

	#region Methods
	internal virtual int Compute(int a_value, bool a_isFlatFirst)
	{
		int result = a_value;
		if(a_isFlatFirst)
		{
			result += flat;
			result += Mathf.FloorToInt(result * percent);
		}
		else
		{
			result += Mathf.FloorToInt(result * percent);
			result += flat;
		}
		
		return result;
	}
	
	internal virtual int ComputeAdditive(int a_value, int a_stack)
	{
		int result = a_value;
		
		result += Mathf.FloorToInt(result * percent * (a_stack - 1));
		result += flat * (a_stack - 1);
		
		return result;
	}
	
	internal void Add(IntModifier a_toAdd)
	{
		
	}
	
	internal bool IsBonus
	{
		get
		{
			return percent >= 0f && flat >= 0;
		}
	}
	
	
	public static IntModifier operator + (IntModifier x, IntModifier y)
	{
		IntModifier result = new IntModifier();
		result.flat = x.flat + y.flat;
		result.percent = x.percent + y.percent;
		return result;
	}
	#endregion
}
