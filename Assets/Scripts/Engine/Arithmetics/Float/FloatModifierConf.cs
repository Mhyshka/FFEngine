using UnityEngine;
using System.Collections;

[System.Serializable]
public class FloatModifierConf
{
	#region Inspector Properties
	public FloatValue percent = null;
	public FloatValue flat = null;
	#endregion
	
	#region Properties
	#endregion
	
	#region Methods
	internal FloatModifier Compute()
	{
		FloatModifier modifier = new FloatModifier();
		
		modifier.flat = flat.Value;
		modifier.percent = percent.Value;
		return modifier;
	}
	#endregion
}

