using UnityEngine;
using System.Collections;

[System.Serializable]
public class FloatInspector : FloatValue
{
	public float value = 0;
	
	internal override float Value
	{
		get
		{
			return value;
		}
	}
}
