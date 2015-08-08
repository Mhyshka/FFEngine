using UnityEngine;
using System.Collections;

public class GameConstants
{	
	#region Attacks
	public float   CRITICAL_BASE_CHANCE = 0.03f;
	public int     CRITICAL_DAMAGE_BONUS_FLAT = 0;
	public float   CRITICAL_DAMAGE_BONUS_PERCENT = 1f;
	public float   CRITICAL_ARMOR_PERCENT_REDUCTION = 0.70f;
	public int     CRITICAL_ARMOR_FLAT_REDUCTION = 0;
	
	public float   PENETRATION_BASE_CHANCES = 0.03f;
	public int     PENETRATION_DAMAGE_BONUS_FLAT = 0;
	public float   PENETRATION_DAMAGE_BONUS_PERCENT = 0f;
	public float   PENETRATION_ARMOR_PERCENT_REDUCTION = 0.50f;
	public int     PENETRATION_ARMOR_FLAT_REDUCTION = 0;
	
	public float   SCRATCH_DAMAGE_MULTIPLIER = 0.15f;
	#endregion
	
	#region Attributes
	public int     STRENGTH_HP_PER_POINT = 25;
	public float   STRENGTH_DAMAGE_PERCENT_PER_POINT = 0.05f;
	public float   STRENGTH_ARPEN_PER_POINT = 2f; //Flat
	public float   STRENGTH_MAX_BLOCK_PER_POINT = 2f;
	public int     STRENGTH_SLOT_PER_POINT = 2;
	
	public float   AGILITY_CRITICAL_CHANCE_PER_POINT = 0.01f;
	public float   AGILITY_PENETRATING_CHANCE_PER_POINT = 0.015f;
	public float   AGILITY_ATTACK_SPEED_PER_POINT = 0.03f; //Multiplicative
	public float   AGILITY_MOVE_SPEED_PER_POINT = 1f; //Flat
	public float   AGILITY_CAST_SPEED_PER_POINT = 0.03f; //Multiplicative
	public float   AGILITY_DODGE_CHANCE_PER_POINT = 1f;
	public float   AGILITY_PARRY_CHANCE_PER_POINT = 1f;
	
	public float   INTELLIGENCE_DAMAGE_PERCENT_PER_POINT = 0.05f;
	public int     INTELLIGENCE_MAX_SPELL_LEVEL_INCREMENTATION = 2;
	public float   INTELLIGENCE_CRITICAL_CHANCE_PER_POINT = 0.01f;
	
	public float   SPIRIT_MANA_REGEN_PER_POINT = 0.15f;
	public int     SPIRIT_MANA_PER_POINT = 10;
	public int     SPIRIT_ARMOR_PER_POINT = 3;
	#endregion
	
	#region Modifiers
	internal bool DAMAGE_BONUS_IS_FLAT_FIRST = true;
	internal bool ARMOR_REDUCTION_FROM_ARPEN_IS_FLAT_FIRST = false;
	internal bool DAMAGE_REDUCTION_FROM_ARMOR_IS_FLAT_FIRST = true;
	
	internal ModifiedConf ARPEN_MODIFIED_CONF = new ModifiedConf();
	internal ModifiedConf DAMAGE_MODIFIED_CONF = new ModifiedConf();
	internal ModifiedConf CRIT_CHANCES_MODIFIED_CONF = new ModifiedConf();
	
	internal ModifiedConf ARMOR_MODIFIED_CONF = new ModifiedConf();
	
	internal ModifiedConf ATTRIBUTES_MODIFIED_CONF = new ModifiedConf();
	internal ModifiedConf LIFE_MODIFIED_CONF = new ModifiedConf();
	internal ModifiedConf MOVE_SPEED_MODIFIED_CONF = new ModifiedConf();
	#endregion
	
	internal static float ArmorPercentReduction(int a_armor, IntModifier a_arpen)
	{
		int effectiveArmor = a_armor;
		
		if(a_arpen != null)
			effectiveArmor = a_arpen.Compute(effectiveArmor, FFEngine.Game.Constants.ARMOR_REDUCTION_FROM_ARPEN_IS_FLAT_FIRST);
		
		float reduction = 1f + (- 1f / Mathf.Exp(effectiveArmor/50f));
		
		return reduction;
	}
}


