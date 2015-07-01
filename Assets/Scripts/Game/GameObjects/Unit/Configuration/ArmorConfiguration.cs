using UnityEngine;
using System.Collections;

public struct Armor
{
	internal int armor;
	internal int flat;
}

[System.Serializable]
public class ArmorConfiguration
{
	#region Inspector Properties
	public IntModified armor = new IntModified();
	#endregion
	
	#region Properties
	internal IntModified flat = new IntModified();
	#endregion
	
	#region Methods	
	public Armor ToResistance()
	{
		Armor result = new Armor();
		result.armor = armor.Value;
		result.flat = flat.Value;
		return result;
	}
	
	public static Armor operator +(ArmorConfiguration x, ArmorConfiguration y) 
	{
		Armor result = new Armor();
		result.armor = x.armor + y.armor;
		result.flat = x.flat + y.flat;
		return result;
	}
	#endregion
	
	
}
