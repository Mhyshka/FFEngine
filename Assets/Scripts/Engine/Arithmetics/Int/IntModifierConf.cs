using UnityEngine;
using System.Collections;

[System.Serializable]
public class IntModifierConf
{
	#region Inspector Properties
	public FloatValue percent = null;
	public IntValue flat = null;
	#endregion
	
	#region Properties
	internal IntModifier Compute()
	{
		IntModifier mod = new IntModifier();
		mod.percent = percent.Value;
		mod.flat = flat.Value;
		return mod;
	}
	#endregion
	
	#region Methods
	#endregion
}

[System.Serializable]
public class IntModifierInspectorConf
{
	#region Inspector Properties
	public float percent = 0f;
	public int flat = 0;
	#endregion
	
	#region Properties
	internal IntModifier Compute()
	{
		IntModifier mod = new IntModifier();
		mod.percent = percent;
		mod.flat = flat;
		return mod;
	}
	#endregion
	
	#region Methods
	#endregion
}