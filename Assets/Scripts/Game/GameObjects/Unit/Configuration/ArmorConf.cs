using UnityEngine;
using System.Collections;

[System.Serializable]
public class ArmorConf
{
	#region Inspector Properties
	public IntModifiedCustomConf armor = null;
	#endregion
	
	#region Properties
	internal IntModifiedCustomConf flat = null;
	#endregion
	
	#region Methods	
	internal Resistance Compute()
	{
		Resistance result = new Resistance();
		result.armor = armor.Value;
		result.flat = flat.Value;
		return result;
	}
	#endregion
}
