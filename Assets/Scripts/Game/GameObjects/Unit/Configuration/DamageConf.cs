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
	internal DamageWrapper Compute(Unit a_src, EAttackStrikeType a_strike)
	{
		DamageWrapper dmg = new DamageWrapper();
		
		IntModifier damageModifier = a_src.attack.GetBonusDamage(type);
		Reduction arpenModifier = a_src.attack.GetPenetration(type);
		
		if(a_strike == EAttackStrikeType.Crititcal)
		{
			damageModifier.percent += a_src.attack.CriticalDamages;
			arpenModifier.percent = Mathf.Clamp01(arpenModifier.percent + a_src.attack.CriticalArpen);
		}
		else if(a_strike == EAttackStrikeType.Penetration)
		{
			damageModifier.percent += a_src.attack.PenetrationDamages;
			arpenModifier.percent = Mathf.Clamp01(arpenModifier.percent + a_src.attack.PenetrationArpen);
		}
		
		dmg.amount = range.Value;
		
		dmg.amount = damageModifier.Compute(dmg.amount, GameConstants.DAMAGE_BONUS_IS_FLAT_FIRST);
		dmg.arpen =  arpenModifier + arpen;
		
		dmg.type = type;
		
		return dmg;
	}
	#endregion
}
