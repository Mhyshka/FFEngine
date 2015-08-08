using UnityEngine;
using System.Collections;

[System.Serializable]
public class FloatModifiedModifierConf
{
	#region Inspector Propertes
	public FloatValue percent = null;
	public FloatValue flat = null;
	#endregion
	
	internal FloatModifiedModifier Compute()
	{
		FloatModifiedModifier mod = new FloatModifiedModifier();
		
		FloatModified newMod = new FloatModified();
		newMod.BaseValue = percent.Value;
		mod.percent = newMod;
		
		newMod = new FloatModified();
		newMod.BaseValue = flat.Value;
		mod.flat = newMod;
		
		return mod;
	}
}
