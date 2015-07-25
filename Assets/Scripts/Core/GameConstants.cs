using UnityEngine;
using System.Collections;

public class GameConstants
{	
	#region Attacks
	public float   CRITICAL_ARMOR_REDUCTION = 0.70f;
	public float   CRITICAL_DAMAGE_BONUS_PERCENT = 1f;
	
	public float   PENETRATION_ARMOR_REDUCTION = 0.50f;
	public float   PENETRATION_DAMAGE_BONUS_PERCENT = 0f;
	
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
	internal bool LIFE_BONUS_HP_IS_FLAT_FIRST = true;
	internal bool ARPEN_REDUCTION_IS_FLAT_FIRST = false;
	
	internal bool ARMOR_REDUCTION_IS_FLAT_FIRST = false;
	internal bool ARMOR_SCORE_IS_FLAT_FIRST = true;
	
	internal bool ATTRIBUTES_SCORE_IS_FLAT_FIRST = true;
	
	internal bool MOVE_SPEED_IS_FLAT_FIRST = true;
	
	internal bool DAMAGE_BONUS_IS_FLAT_FIRST = true;
	#endregion
}


