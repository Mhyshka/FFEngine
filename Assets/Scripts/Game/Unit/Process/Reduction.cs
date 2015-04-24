using UnityEngine;
using System.Collections;

public struct Reduction
{
	public float percent;
	public float PercentMultiplier
	{
		get
		{
			return 1f - percent;
		}
	}
	public float flat;
	public bool isFlatFirst;
	
	internal float Compute(float a_value)
	{
		float result = a_value;
		if(isFlatFirst)
		{
			result = Mathf.MoveTowards(result,
			                           0f,
			                           flat);
			result = Mathf.Lerp(result,
			                    0f,
			                    1f - percent);
		}
		else
		{
			result = Mathf.Lerp(result,
			                    0f,
			                    1f - percent);
			result = Mathf.MoveTowards(result,
			                           0f,
			                           flat);
		}
		
		return result;
	}
	
	internal int Compute(int a_value)
	{
		int result = a_value;
		if(isFlatFirst)
		{
			result = Mathf.CeilToInt(Mathf.MoveTowards(result,
			                                           0f,
			                                           Mathf.FloorToInt(flat)));
			result = Mathf.CeilToInt(Mathf.Lerp(0f,
			                                    result,
			                                    PercentMultiplier));
		}
		else
		{
			result = Mathf.CeilToInt(Mathf.Lerp(0f,
			                                    result,
			                                    PercentMultiplier));
			result = Mathf.CeilToInt(Mathf.MoveTowards(result,
			                                           0f,
			                                           Mathf.FloorToInt(flat)));
		}
		
		return result;
	}

	public static Reduction operator + (Reduction x, Reduction y)
	{
		Reduction result = new Reduction();
		result.flat = x.flat + y.flat;
		result.percent = 1f - (x.PercentMultiplier * y.PercentMultiplier);
		result.isFlatFirst = x.isFlatFirst && y.isFlatFirst;
		return result;
	}
}