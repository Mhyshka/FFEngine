using UnityEngine;
using System.Collections;

[System.Serializable]
/// <summary>
/// Standard damage effect. Can Crit & penetrate.
/// </summary>
public class EffectDamageConf : EffectConf
{
	#region Inspector Properties
	public IntRange range = null;
	public Reduction arpen = new Reduction(); 
	public EDamageType type = new EDamageType();
	#endregion

	#region Properties
	internal EAttackStrikeType strikeType = EAttackStrikeType.Normal;
	private bool wasSet = false;
	#endregion

	#region Methods
	/// <summary>
	/// Returns a DamageEffect. You should consider calling the overload methods that provide EAttackStrikeType as an additional parameter.
	/// </summary>
	internal override Effect Compute(Unit a_src)
	{
		EffectDamage dmg = new EffectDamage();
		
		IntModifier damageModifier = a_src.attack.GetBonusDamage(type);
		Reduction arpenModifier = a_src.attack.GetPenetration(type);
		
		if(wasSet && strikeType == EAttackStrikeType.Crititcal)
		{
			damageModifier.percent += a_src.attack.CriticalDamages;
			arpenModifier.percent = Mathf.Clamp01(arpenModifier.percent + a_src.attack.CriticalArpen);
		}
		else if(wasSet && strikeType == EAttackStrikeType.Penetration)
		{
			damageModifier.percent += a_src.attack.PenetrationDamages;
			arpenModifier.percent = Mathf.Clamp01(arpenModifier.percent + a_src.attack.PenetrationArpen);
		}
		
		dmg.amount = range.Value;
		
		dmg.amount = damageModifier.Compute(dmg.amount, GameConstants.DAMAGE_BONUS_IS_FLAT_FIRST);
		dmg.arpen =  arpenModifier;
		
		dmg.type = type;
		
		wasSet = false;
		
		return dmg;
	}
	
	/// <summary>
	/// Returns a DamageEffect
	/// </summary>
	internal virtual Effect Compute(Unit a_src, EAttackStrikeType a_type)
	{
		wasSet = true;
		strikeType = a_type;
		return Compute (a_src);
	}
	#endregion
}

/*

+ arpen*/