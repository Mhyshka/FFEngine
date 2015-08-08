using UnityEngine;
using System.Collections;

public class FloatModifierFromEffect
{
	internal FloatModifier modifier = null;
	internal EffectOverTime source = null;
}

public class FloatModifier
{
	#region Inspector Properties
	internal float percent = 0f;
	internal float flat = 0f;
	#endregion
	
	#region Properties
	#endregion
	
	#region Methods
	internal float Compute(float a_value, bool a_isFlatFirst)
	{
		float result = a_value;
		if(a_isFlatFirst)
		{
			result += flat;
			result += result * percent;
		}
		else
		{
			result += result * percent;
			result += flat;
		}
		
		return result;
	}
	
	internal float ComputeAdditive(float a_value, int a_stack)
	{
		float result = a_value;
		
		result += result * percent * (a_stack - 1);
		result += flat * (a_stack - 1);
		
		return result;
	}
	
	internal virtual int ComputeAdditive(int a_value, int a_stack)
	{
		int result = a_value;
		
		result += Mathf.FloorToInt(result * percent * (a_stack - 1));
		result += Mathf.FloorToInt(flat * (a_stack - 1));
		
		return result;
	}
	
	internal void Add(FloatModifier a_toAdd)
	{
		percent += a_toAdd.percent;
		flat += a_toAdd.flat;
	}
	
	internal bool IsBonus
	{
		get
		{
			return percent >= 0f && flat >= 0f;
		}
	}
	
	internal bool IsUseless
	{
		get
		{
			return percent == 0f && flat == 0;
		}
	}
	#endregion
	
	public static FloatModifier operator + (FloatModifier x, FloatModifier y)
	{
		FloatModifier result = new FloatModifier();
		result.flat = x.flat + y.flat;
		result.percent = x.percent + y.percent;
		return result;
	}
}
