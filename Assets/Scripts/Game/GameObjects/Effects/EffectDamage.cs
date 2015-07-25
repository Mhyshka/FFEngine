using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EDamageType
{
//Physical
	Slashing,
	Piercing,
	Crushing,
	
//Magic
	Magic,
	/*Fire,
	Frost,
	Poison,
	Ligthing,*/
	Spirit,
	
	True
}

public class EffectDamage : Effect
{
	#region Inspector Properties
	internal EffectDamageConf conf;
	internal EDamageType type;
	internal int amount;
	internal Reduction arpen;
	#endregion

	#region Properties
	internal override int MetaStrength
	{
		get
		{
			return amount;
		}
	}
	
	internal override bool IsRevertOnDestroy
	{
		get
		{
			return false;
		}
	}
	#endregion

	#region Methods
	#endregion
	
	#region Effect
	internal override AEffectReport Apply(Unit a_target)
	{
		DamageReport report = new DamageReport();
		
		Reduction reduction = a_target.defense.GetResistance(type, arpen);
		
		report.attackInfos = attackInfos;
		report.target = a_target;
		report.type = type;
		report.unreduced = amount;
		report.final = reduction.Compute(report.unreduced, FFEngine.Game.Constants.ARMOR_REDUCTION_IS_FLAT_FIRST);
		
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
	
	internal override AEffectReport Revert (Unit a_target)
	{
		return new DamageReport();
	}
	#endregion
	
}