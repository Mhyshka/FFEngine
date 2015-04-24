using UnityEngine;
using System.Collections;

[System.Serializable]
public class IntModified : IntValue
{
	#region Inspector Properties
	public int baseValue = 0;
	#endregion
	
	#region Properties
	internal IntModifier modifier = new IntModifier();
	
	internal override int Value
	{
		get
		{
			return modifier.Compute(baseValue);
		}
	}
	#endregion
	
	#region Methods
	#endregion
	
	public static int operator +(IntModified x, IntModified y)
	{
		return x.Value + y.Value;
	}
}
