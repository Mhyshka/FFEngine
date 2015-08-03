using UnityEngine;
using System.Collections;

[System.Serializable]
public class FloatModifiedModifierConf
{
	#region Inspector Propertes
	public FloatModifiedCustomConf percent = null;
	public FloatModifiedCustomConf flat = null;
	#endregion
	
	internal FloatModifiedModifier Compute()
	{
		FloatModifiedModifier mod = new FloatModifiedModifier();
		mod.percent = percent.Compute();
		mod.flat = flat.Compute();
		return mod;
	}
}
