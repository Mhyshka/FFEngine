using UnityEngine;
using System.Collections;

[System.Serializable]
public class FloatModifier
{
	#region Inspector Properties
	public float percent = 0f;
	public float flat = 0f;
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
	
	public static FloatModifier operator + (FloatModifier x, FloatModifier y)
	{
		FloatModifier result = new FloatModifier();
		result.flat = x.flat + y.flat;
		result.percent = x.percent + y.percent;
		return result;
	}
	#endregion
}
