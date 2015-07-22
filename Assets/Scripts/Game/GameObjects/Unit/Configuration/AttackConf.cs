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
	public List<DamageConf> damages = null;
	#endregion

	#region Properties
	internal AttackWrapper Compute(Unit a_src)
	{
		AttackWrapper attack = new AttackWrapper();
		attack.name = attackName;
		attack.source = a_src;
		attack.damages = new List<DamageEffect>();
		
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
		
		foreach(DamageConf each in damages)
		{
			DamageEffect dmg = each.Compute(a_src) as DamageEffect;
			attack.damages.Add (dmg);
		}
		
		return attack;
	}
	
	protected void StrikeTypeProcess(DamageEffect dmg, EAttackStrikeType a_strike)
	{
		/*if(a_strike == EAttackStrikeType.Crititcal)
		{
			damageModifier.percent += a_src.attack.CriticalDamages;
			arpenModifier.percent = Mathf.Clamp01(arpenModifier.percent + a_src.attack.CriticalArpen);
		}
		else if(a_strike == EAttackStrikeType.Penetration)
		{
			damageModifier.percent += a_src.attack.PenetrationDamages;
			arpenModifier.percent = Mathf.Clamp01(arpenModifier.percent + a_src.attack.PenetrationArpen);
		}*/
	}
	#endregion

	#region Methods
	#endregion
}
