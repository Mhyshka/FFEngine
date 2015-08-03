using UnityEngine;
using System.Collections;

[System.Serializable]
public class ArmorConf
{
	#region Inspector Properties
	public IntModifiedCustomConf armor = null;
	public IntModifiedCustomConf flat = null;
	#endregion
	
	#region Methods	
	internal Armor Compute()
	{
		Armor result = new Armor();
		result.armor = armor.Compute();
		result.flat = flat.Compute();
		return result;
	}
	#endregion
}
