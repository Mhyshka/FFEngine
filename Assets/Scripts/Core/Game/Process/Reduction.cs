using UnityEngine;
using System.Collections;

public struct Reduction
{
	public float percent;
	public float flat;
	
	internal float Compute(float a_value, bool a_isFlatFirst)
	{
		float result = a_value;
		if(a_isFlatFirst)
		{
			result = Mathf.MoveTowards(result,
			                           0f,
			                           flat);
			result = Mathf.Lerp(result,
			                    0f,
			                    percent);
		}
		else
		{
			result = Mathf.Lerp(result,
			                    0f,
			                    percent);
			result = Mathf.MoveTowards(result,
			                           0f,
			                           flat);
		}
		
		return result;
	}
	
	internal int Compute(int a_value, bool a_isFlatFirst)
	{
		int result = a_value;
		if(a_isFlatFirst)
		{
			result = Mathf.CeilToInt(Mathf.MoveTowards(result,
			                                           0f,
			                                           Mathf.FloorToInt(flat)));
			result = Mathf.CeilToInt(Mathf.Lerp(result,
			                                    0f,
			                                    Mathf.Clamp01(percent)));
		}
		else
		{
			result = Mathf.CeilToInt(Mathf.Lerp(result,
			                                    0f,
			                                    Mathf.Clamp01(percent)));
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
		result.percent = Mathf.Clamp01(x.percent + y.percent);
		return result;
	}
}