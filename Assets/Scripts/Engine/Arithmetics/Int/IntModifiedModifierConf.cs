using UnityEngine;
using System.Collections;

[System.Serializable]
public class IntModifiedModifierConf
{
	#region Inspector Propertes
	public FloatModifiedCustomConf percent = null;
	public IntModifiedCustomConf flat = null;
	#endregion
	
	internal IntModifiedModifier Compute()
	{
		IntModifiedModifier mod = new IntModifiedModifier();
		mod.percent = percent.Compute();
		mod.flat = flat.Compute();
		return mod;
	}
}
