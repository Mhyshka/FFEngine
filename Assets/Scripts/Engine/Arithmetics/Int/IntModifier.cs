using UnityEngine;
using System.Collections;

public class IntModifier
{
	#region Inspector Properties
	public float percent = 0f;
	public int flat = 0;
	#endregion

	#region Properties
	internal bool isFlatFirst = false;
	#endregion

	#region Methods
	internal int Compute(int a_value, bool a_isFlatFirst)
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
	
	internal int ComputeAdditive(int a_value, int a_stack)
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
