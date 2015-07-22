using UnityEngine;
using System.Collections;

[System.Serializable]
public class DamageConf : EffectConf
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
	/// Returns a DamageEffect
	/// </summary>
	internal override Effect Compute(Unit a_src)
	{
		DamageEffect dmg = new DamageEffect();
		
		IntModifier damageModifier = a_src.attack.GetBonusDamage(type);
		Reduction arpenModifier = a_src.attack.GetPenetration(type);
		
		
		
		dmg.amount = range.Value;
		
		dmg.amount = damageModifier.Compute(dmg.amount, GameConstants.DAMAGE_BONUS_IS_FLAT_FIRST);
		dmg.arpen =  arpenModifier;
		
		dmg.type = type;
		
		return dmg;
	}
	#endregion
}

/*

+ arpen*/