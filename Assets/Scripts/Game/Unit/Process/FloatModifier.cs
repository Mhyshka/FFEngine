using UnityEngine;
using System.Collections;

[System.Serializable]
internal class FloatModifier
{
	#region Inspector Properties
	public float percent = 0f;
	public float flat = 0f;
	public bool isFlatFirst = false;
	#endregion
	
	#region Properties
	#endregion
	
	#region Methods
	internal float Compute(float a_value)
	{
		float result = a_value;
		if(isFlatFirst)
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
	#endregion
}
