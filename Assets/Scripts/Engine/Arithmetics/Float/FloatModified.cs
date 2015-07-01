using UnityEngine;
using System.Collections;

[System.Serializable]
public class FloatModified
{
	#region Inspector Properties
	public float baseValue = 0f;
	#endregion
	
	#region Properties
	internal FloatModifier modifier = new FloatModifier();
	internal bool isFlatFirst = false;
	
	internal float Value
	{
		get
		{
			return modifier.Compute(baseValue, isFlatFirst);
		}
	}
	#endregion
	
	#region Methods
	#endregion
	public static float operator +(FloatModified x, FloatModified y)
	{
		return x.Value + y.Value;
	}
}
