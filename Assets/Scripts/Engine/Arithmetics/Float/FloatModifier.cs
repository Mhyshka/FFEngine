using UnityEngine;
using System.Collections;

[System.Serializable]
internal class FloatModifier
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
	
	public static FloatModifier operator + (FloatModifier x, FloatModifier y)
	{
		FloatModifier result = new FloatModifier();
		result.flat = x.flat + y.flat;
		result.percent = x.percent + y.percent;
		return result;
	}
	#endregion
}
