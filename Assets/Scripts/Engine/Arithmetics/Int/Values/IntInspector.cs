using UnityEngine;
using System.Collections;

[System.Serializable]
public class IntHardcode : IntValue
{
	#region Inspector Properties
	public int value = 0;
	#endregion

	#region Properties
	#endregion

	#region Methods
	internal override int Value
	{
		get
		{
			return value;
		}
	}
	#endregion

}
