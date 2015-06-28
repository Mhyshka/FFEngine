using UnityEngine;
using System.Collections;

[System.Serializable]
public class DamageConf
{
	#region Inspector Properties
	public IntRange range = null;
	public Reduction arpen = new Reduction(); 
	public EDamageType type = new EDamageType();
	#endregion

	#region Properties
	#endregion

	#region Methods
	internal DamageWrapper Compute(Unit a_src)
	{
		DamageWrapper dmg = new DamageWrapper();
		dmg.amount = range.Value;
		//dmg.amount = a_src.stats.attributes.GetPower(type).Compute(dmg.amount);
		//dmg.arpen = a_src.stats.attributes.GetPenetration(type) + arpen;
		dmg.type = type;
		return dmg;
	}
	#endregion
}
