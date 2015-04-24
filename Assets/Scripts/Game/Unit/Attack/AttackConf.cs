using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public abstract class AttackConf
{
	#region Inspector Properties
	public FloatModified rechargeRate = null;
	public List<DamageConf> damages = null;
	#endregion

	#region Properties
	internal AttackWrapper Compute(Unit a_src)
	{
		AttackWrapper attack = new AttackWrapper();
		attack.source = a_src;
		attack.damages = new List<DamageWrapper>();
		
		foreach(DamageConf each in damages)
		{
			attack.damages.Add (each.Compute(a_src));
		}
		
		return attack;
	}
	#endregion

	#region Methods
	#endregion
}
