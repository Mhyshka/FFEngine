using UnityEngine;
using System.Collections;



/// <summary>
/// Attack damage effect. Can Crit & penetrate. you must call the Compute() that use an additional parameter.
/// </summary>
[System.Serializable]
public class EffectDamageConf : EffectConf
{
	#region Inspector Properties
	public IntValue baseAmount = null;
	public IntModifierConf arpen = null; 
	public EDamageType type = EDamageType.Slashing;
	#endregion

	#region Properties
	#endregion

	#region Methods
	internal override AEffect Compute(AttackInfos a_attackInfos)
	{
		EffectDamage dmg = new EffectDamage();
		dmg.attackInfos = a_attackInfos;
		
		IntModifier damageFromStats = a_attackInfos.source.attack.GetBonusDamage(type);
		IntModifier arpenFromStats = a_attackInfos.source.attack.GetPenetration(type);
		if(canCrit && a_attackInfos.critType == ECriticalType.Crititcal)
		{
			damageFromStats.Add(a_attackInfos.source.attack.CriticalDamages);
			arpenFromStats.Add(a_attackInfos.source.attack.CriticalArpen);
		}
		else if(canCrit && a_attackInfos.critType == ECriticalType.Penetration)
		{
			damageFromStats.Add(a_attackInfos.source.attack.PenetrationDamages);
			arpenFromStats.Add(a_attackInfos.source.attack.PenetrationArpen);
		}
		
		dmg.amount = baseAmount.Value;
		dmg.amount = damageFromStats.Compute(dmg.amount, FFEngine.Game.Constants.DAMAGE_BONUS_IS_FLAT_FIRST);
		dmg.arpen =  arpenFromStats;
		
		dmg.type = type;
		
		return dmg;
	}
	#endregion
}

/*

+ arpen*/