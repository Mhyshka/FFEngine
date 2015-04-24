using UnityEngine;
using System.Collections;

[System.Serializable]
internal class IntModifier
{
	#region Inspector Properties
	public float percent = 0f;
	public int flat = 0;
	public bool isFlatFirst = false;
	#endregion

	#region Properties
	#endregion

	#region Methods
	internal int Compute(int a_value)
	{
		int result = a_value;
		if(isFlatFirst)
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
	#endregion
}
