using UnityEngine;
using System.Collections;



/// <summary>
/// Attack damage effect. Can Crit & penetrate. you must call the Compute() that use an additional parameter.
/// </summary>
[System.Serializable]
public class EffectDamageConf : EffectConf
{
	#region Inspector Properties
	public IntRange range = null;
	public Reduction arpen = new Reduction(); 
	public EDamageType type = new EDamageType();
	#endregion

	#region Properties
	#endregion

	#region Methods
	/// <summary>
	/// Returns a DamageEffect.
	/// </summary>
	internal override Effect Compute(AttackInfos a_attackInfos)
	{
		EffectDamage dmg = new EffectDamage();
		dmg.attackInfos = a_attackInfos;
		
		IntModifier damageModifier = a_attackInfos.source.attack.GetBonusDamage(type);
		Reduction arpenModifier = a_attackInfos.source.attack.GetPenetration(type);
		
		if(canCrit && a_attackInfos.critType == ECriticalType.Crititcal)
		{
			damageModifier.percent += a_attackInfos.source.attack.CriticalDamages;
			arpenModifier.percent = Mathf.Clamp01(arpenModifier.percent + a_attackInfos.source.attack.CriticalArpen);
		}
		else if(canCrit && a_attackInfos.critType == ECriticalType.Penetration)
		{
			damageModifier.percent += a_attackInfos.source.attack.PenetrationDamages;
			arpenModifier.percent = Mathf.Clamp01(arpenModifier.percent + a_attackInfos.source.attack.PenetrationArpen);
		}
		
		dmg.amount = range.Value;
		
		dmg.amount = damageModifier.Compute(dmg.amount, FFEngine.Game.Constants.DAMAGE_BONUS_IS_FLAT_FIRST);
		dmg.arpen =  arpenModifier;
		
		dmg.type = type;
		
		return dmg;
	}
	#endregion
}

/*

+ arpen*/