using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AttackConf
{
	#region Inspector Properties
	public string attackName = "attack";
	public FloatModified range = null;
	public FloatModified areaOfEffect = null;
	public FloatModified cooldown = null;
	public FloatModified rechargeRate = null;
	public List<EffectDamageConf> damages = null;
	#endregion

	#region Properties
	internal AttackWrapper Compute(Unit a_src)
	{
		AttackWrapper attack = new AttackWrapper();
		attack.name = attackName;
		attack.source = a_src;
		attack.damages = new List<EffectDamage>();
		
		EAttackStrikeType type = EAttackStrikeType.Normal;
		float rand = Random.value;
		if(rand <= a_src.attack.CriticalChances)
		{
			type = EAttackStrikeType.Crititcal;
		}
		else if(rand <= a_src.attack.PenetrationChances)
		{
			type = EAttackStrikeType.Penetration;
		}
		attack.strikeType = type;
		
		foreach(EffectDamageConf each in damages)
		{
			EffectDamage dmg = each.Compute(a_src) as EffectDamage;
			attack.damages.Add (dmg);
		}
		
		return attack;
	}
	
	protected void StrikeTypeProcess(EffectDamage dmg, EAttackStrikeType a_strike)
	{
		/**/
	}
	#endregion

	#region Methods
	#endregion
}
