using UnityEngine;
using System.Collections;

public class UnitDefense : AUnitComponent
{
	#region Defensive Stats
	public ArmorConf generalArmor = null;
	
	public ArmorConf slashingArmor = null;
	public ArmorConf crushingArmor = null;
	public ArmorConf piercingArmor = null;
	
	public ArmorConf magicArmor = null;
	
	public ArmorConf spiritArmor = null;
	#endregion
	
	internal override void Init (AInteractable a_unit)
	{
		base.Init (a_unit);
		generalArmor.armor.isFlatFirst = FFEngine.Game.Constants.ARMOR_SCORE_IS_FLAT_FIRST;
		generalArmor.flat.isFlatFirst = FFEngine.Game.Constants.ARMOR_SCORE_IS_FLAT_FIRST;
		
		slashingArmor.armor.isFlatFirst = FFEngine.Game.Constants.ARMOR_SCORE_IS_FLAT_FIRST;
		slashingArmor.flat.isFlatFirst = FFEngine.Game.Constants.ARMOR_SCORE_IS_FLAT_FIRST;
		
		crushingArmor.armor.isFlatFirst = FFEngine.Game.Constants.ARMOR_SCORE_IS_FLAT_FIRST;
		crushingArmor.flat.isFlatFirst = FFEngine.Game.Constants.ARMOR_SCORE_IS_FLAT_FIRST;
		
		piercingArmor.armor.isFlatFirst = FFEngine.Game.Constants.ARMOR_SCORE_IS_FLAT_FIRST;
		piercingArmor.flat.isFlatFirst = FFEngine.Game.Constants.ARMOR_SCORE_IS_FLAT_FIRST;
		
		magicArmor.armor.isFlatFirst = FFEngine.Game.Constants.ARMOR_SCORE_IS_FLAT_FIRST;
		magicArmor.flat.isFlatFirst = FFEngine.Game.Constants.ARMOR_SCORE_IS_FLAT_FIRST;
		
		spiritArmor.armor.isFlatFirst = FFEngine.Game.Constants.ARMOR_SCORE_IS_FLAT_FIRST;
		spiritArmor.flat.isFlatFirst = FFEngine.Game.Constants.ARMOR_SCORE_IS_FLAT_FIRST;
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
		int effectiveArmor = a_arpen.Compute(armor, FFEngine.Game.Constants.ARPEN_REDUCTION_IS_FLAT_FIRST);
		
		float reduction = 1f + (- 1f / Mathf.Exp(effectiveArmor/50f));
		
		return reduction;
	}
	
	internal float ComputeFlatReduction(int flat, Reduction a_arpen)
	{
		return flat * (1f - a_arpen.percent);
	}
	#endregion
	
	#region Damage management
	internal bool ShouldScratch(EffectDamage a_dmg)
	{
		float rand = Random.value * 100f;
		int armorAfterReduction = a_dmg.arpen.Compute(GerArmor(a_dmg.type).armor, FFEngine.Game.Constants.ARPEN_REDUCTION_IS_FLAT_FIRST);
		if(rand < Mathf.Clamp(armorAfterReduction, 0f, 100f))
		{
			return true;
		}
		
		return false;
	}
	#endregion
}
