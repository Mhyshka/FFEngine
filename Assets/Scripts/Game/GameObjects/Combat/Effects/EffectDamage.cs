using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum EDamageType
{
//Physical
	Slashing,
	Piercing,
	Crushing,

	
//Magic
	Fire,
	Frost,
	Ligthing,
	
//StandAlone
	Poison,
	Bleed,
	Spirit,
	True
}

public class EffectDamage : AEffect
{
	#region Inspector Properties
	internal EffectDamageConf conf;
	internal EDamageType type;
	internal int amount;
	internal IntModifier arpen;
	#endregion

	#region Properties
	internal override int MetaStrength
	{
		get
		{
			return amount;
		}
	}
	#endregion

	#region Methods
	#endregion
	
	#region Effect
	internal override AEffectReport Apply(Unit a_target)
	{
		DamageReport report = new DamageReport();
		
		IntModifier reduction = a_target.defense.GetResistance(type).Compute(arpen);
		
		report.attackInfos = attackInfos;
		report.effectInfos = effectInfos;
		
		int baseDmg = ComputeStackModifications();
		
		report.target = a_target;
		report.type = type;
		report.unreduced = baseDmg;
		report.final = reduction.Compute(report.unreduced,
										FFEngine.Game.Constants.DAMAGE_REDUCTION_FROM_ARMOR_IS_FLAT_FIRST);
		
		//Scratching
		if(attackInfos.critType == ECriticalType.Normal && a_target.defense.ShouldScratch(this))
		{
			report.didScratch = true;
			report.final = Mathf.FloorToInt(report.final * FFEngine.Game.Constants.SCRATCH_DAMAGE_MULTIPLIER);
		}
		else
		{
			report.didScratch = false;
		}
		
		report.reducedByArmor = report.final - report.unreduced;
		
		return report;
	}
	
	internal int ComputeStackModifications()
	{
		if(effectInfos != null)
		{
			if(effectInfos.doesStack && effectInfos.effectOverTime.CurrentStackCount > 1)
			{
				return effectInfos.perStackModifier.ComputeAdditive(amount, 
				                                                     effectInfos.effectOverTime.CurrentStackCount);
			}
		}
		
		return amount;
	}
	
	internal override AEffectReport Revert (Unit a_target)
	{
		return null;
	}
	#endregion
	
}