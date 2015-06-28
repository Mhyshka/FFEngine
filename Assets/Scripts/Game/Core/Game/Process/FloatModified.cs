using UnityEngine;
using System.Collections;

[System.Serializable]
public class FloatModified
{
	#region Inspector Properties
	public float baseValue = 0f;
	#endregion
	
	#region Properties
	internal FloatModifier modifier = null;
	
	internal float Total
	{
		get
		{
			return modifier.Compute(baseValue);
		}
	}
	#endregion
	
	#region Methods
	#endregion
	public static float operator +(FloatModified x, FloatModified y)
	{
		return x.Total + y.Total;
	}
}
