using UnityEngine;
using System.Collections;

public class UnitDefense : AUnitComponent
{
	#region Defensive Stats
	public ArmorConfiguration generalArmor = null;
	
	public ArmorConfiguration slashingArmor = null;
	public ArmorConfiguration crushingArmor = null;
	public ArmorConfiguration piercingArmor = null;
	
	public ArmorConfiguration magicArmor = null;
	
	public ArmorConfiguration spiritArmor = null;
	#endregion
	
	internal override void Init (AInteractable a_unit)
	{
		base.Init (a_unit);
		generalArmor.armor.isFlatFirst = GameConstants.ARMOR_SCORE_IS_FLAT_FIRST;
		generalArmor.flat.isFlatFirst = GameConstants.ARMOR_SCORE_IS_FLAT_FIRST;
		
		slashingArmor.armor.isFlatFirst = GameConstants.ARMOR_SCORE_IS_FLAT_FIRST;
		slashingArmor.flat.isFlatFirst = GameConstants.ARMOR_SCORE_IS_FLAT_FIRST;
		
		crushingArmor.armor.isFlatFirst = GameConstants.ARMOR_SCORE_IS_FLAT_FIRST;
		crushingArmor.flat.isFlatFirst = GameConstants.ARMOR_SCORE_IS_FLAT_FIRST;
		
		piercingArmor.armor.isFlatFirst = GameConstants.ARMOR_SCORE_IS_FLAT_FIRST;
		piercingArmor.flat.isFlatFirst = GameConstants.ARMOR_SCORE_IS_FLAT_FIRST;
		
		magicArmor.armor.isFlatFirst = GameConstants.ARMOR_SCORE_IS_FLAT_FIRST;
		magicArmor.flat.isFlatFirst = GameConstants.ARMOR_SCORE_IS_FLAT_FIRST;
		
		spiritArmor.armor.isFlatFirst = GameConstants.ARMOR_SCORE_IS_FLAT_FIRST;
		spiritArmor.flat.isFlatFirst = GameConstants.ARMOR_SCORE_IS_FLAT_FIRST;
	}
	
	#region Armor
	internal Armor GerArmor(EDamageType type)
	{
		Armor armor;
		
		switch(type)
		{
		case EDamageType.Slashing :
			armor = generalArmor + slashingArmor;
			break;
			
		case EDamageType.Crushing :
			armor = generalArmor + crushingArmor;
			break;
			
		case EDamageType.Piercing :
			armor = generalArmor + piercingArmor;
			break;
			
		case EDamageType.Magic :
			armor = generalArmor + magicArmor;
			break;
			
		case EDamageType.Spirit :
			armor = generalArmor + spiritArmor;
			break;
			
		case EDamageType.True :
			armor = new Armor();
			break;
			
		default: 
			armor = new Armor();
			break;
		}
		
		return armor;
	}
	
	internal Reduction GetResistance(EDamageType type, Reduction a_arpen)
	{
		Armor armor = GerArmor(type);
		
		Reduction reduc = new Reduction();
		reduc.percent = ComputePercentReduction(armor.armor, a_arpen);
		reduc.flat = ComputeFlatReduction(armor.flat, a_arpen);
		return reduc;
	}
	
	internal float ComputePercentReduction(int armor, Reduction a_arpen)
	{
		int effectiveArmor = a_arpen.Compute(armor, GameConstants.ARPEN_REDUCTION_IS_FLAT_FIRST);
		
		float reduction = 1f + (- 1f / Mathf.Exp(effectiveArmor/50f));
		
		return reduction;
	}
	
	internal float ComputeFlatReduction(int flat, Reduction a_arpen)
	{
		return flat * (1f - a_arpen.percent);
	}
	#endregion
	
	#region Damage management
	internal void ApplyDamage(AttackWrapper a_attack)
	{
		DamageReport report;
		
		foreach(DamageEffect each in a_attack.damages)
		{
			report = ComputeDamage(each, a_attack.strikeType);
			_unit.ApplyDamages(report);
			
			if(a_attack.source.onDamageDealt != null)
				a_attack.source.onDamageDealt(_unit, report);
		}
	}
	
	internal DamageReport ComputeDamage(DamageEffect a_dmg, EAttackStrikeType a_strikeType)
	{
		DamageReport report = new DamageReport();
		Reduction reduction = GetResistance(a_dmg.type,a_dmg.arpen);
		
		report.applied = a_dmg.amount;
		report.strikeType = a_strikeType;
		report.final = reduction.Compute(report.applied, GameConstants.ARMOR_REDUCTION_IS_FLAT_FIRST);
		
		//Armor reduction
		if(a_strikeType == EAttackStrikeType.Normal && ShouldScratch(a_dmg))
		{
			report.didScratch = true;
			report.final = Mathf.FloorToInt(report.final * GameConstants.SCRATCH_DAMAGE_MULTIPLIER);
		}
		else
		{
			report.didScratch = false;
		}
		
		report.reducedByArmor = report.final - report.applied;
		
		return report;
	}
	
	internal bool ShouldScratch(DamageEffect a_dmg)
	{
		float rand = Random.value * 100f;
		int armorAfterReduction = a_dmg.arpen.Compute(GerArmor(a_dmg.type).armor, GameConstants.ARPEN_REDUCTION_IS_FLAT_FIRST);
		if(rand < Mathf.Clamp(armorAfterReduction, 0f, 100f))
		{
			return true;
		}
		
		return false;
	}
	#endregion
}
