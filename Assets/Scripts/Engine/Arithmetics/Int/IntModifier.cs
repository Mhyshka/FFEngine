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
	internal bool canGoUnderZero = true;
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
		
		if(!canGoUnderZero)
		{
			result = Mathf.Min(a_value, 0);
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
	
	
	public static IntModifier operator + (IntModifier x, IntModifier y)
	{
		IntModifier result = new IntModifier();
		result.flat = x.flat + y.flat;
		result.percent = x.percent + y.percent;
		return result;
	}
	#endregion
}
