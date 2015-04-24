using UnityEngine;
using System.Collections;

public struct Resistance
{
	internal int armor;
	internal int flat;
}

[System.Serializable]
public class ResistanceConf
{
	
	
	#region Inspector Properties
	public IntModified flat = null;
	public IntModified armor = null;
	#endregion
	
	#region Properties
	#endregion
	
	#region Methods
	public static Resistance operator +(ResistanceConf x, ResistanceConf y) 
	{
		Resistance result = new Resistance();
		result.armor = x.armor + y.armor;
		result.flat = x.flat + y.flat;
		return result;
	}
	#endregion
	
	
}
