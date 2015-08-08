using UnityEngine;
using System.Collections;

[System.Serializable]
public class IntModifiedModifierCustomConf
{
	#region Inspector Propertes
	public FloatValue percent = null;
	public IntValue flat = null;
	#endregion
	
	internal IntModifiedModifier Compute()
	{
		IntModifiedModifier mod = new IntModifiedModifier();
		
		FloatModified newModF = new FloatModified();
		newModF.BaseValue = percent.Value;
		mod.percent = newModF;
		
		IntModified newMod = new IntModified();
		newMod.BaseValue = flat.Value;
		mod.flat = newMod;
		
		return mod;
	}
}

[System.Serializable]
public class IntModifiedModifierConf
{
	#region Inspector Propertes
	public FloatModifiedConf percent = null;
	public IntModifiedConf flat = null;
	#endregion
	
	internal IntModifiedModifier Compute()
	{
		IntModifiedModifier mod = new IntModifiedModifier();
		mod.percent = percent.Compute();
		mod.flat = flat.Compute();
		return mod;
	}
}