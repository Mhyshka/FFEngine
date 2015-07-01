using UnityEngine;
using System.Collections;

public class GameConstants
{
	#region Attacks
	public static float   CRITICAL_ARMOR_REDUCTION = 0.70f;
	public static float   CRITICAL_DAMAGE_BONUS_PERCENT = 1f;
	
	public static float   PENETRATION_ARMOR_REDUCTION = 0.50f;
	public static float   PENETRATION_DAMAGE_BONUS_PERCENT = 0f;
	
	public static float   SCRATCH_DAMAGE_MULTIPLIER = 0f;
	#endregion
	
	#region Attributes
	public static int     STRENGTH_HP_PER_POINT = 25;
	public static float   STRENGTH_DAMAGE_PERCENT_PER_POINT = 0.05f;
	public static float   STRENGTH_ARPEN_PER_POINT = 2f; //Flat
	public static float   STRENGTH_MAX_BLOCK_PER_POINT = 2f;
	public static int     STRENGTH_SLOT_PER_POINT = 2;
	
	public static float   AGILITY_CRITICAL_CHANCE_PER_POINT = 0.01f;
	public static float   AGILITY_PENETRATING_CHANCE_PER_POINT = 0.015f;
	public static float   AGILITY_ATTACK_SPEED_PER_POINT = 0.03f; //Multiplicative
	public static float   AGILITY_MOVE_SPEED_PER_POINT = 1f; //Flat
	public static float   AGILITY_CAST_SPEED_PER_POINT = 0.03f; //Multiplicative
	public static float   AGILITY_DODGE_CHANCE_PER_POINT = 1f;
	public static float   AGILITY_PARRY_CHANCE_PER_POINT = 1f;
	
	public static float   INTELLIGENCE_DAMAGE_PERCENT_PER_POINT = 0.05f;
	public static int     INTELLIGENCE_MAX_SPELL_LEVEL_INCREMENTATION = 2;
	public static float   INTELLIGENCE_CRITICAL_CHANCE_PER_POINT = 0.01f;
	
	public static float   SPIRIT_MANA_REGEN_PER_POINT = 0.15f;
	public static int     SPIRIT_MANA_PER_POINT = 10;
	public static int     SPIRIT_ARMOR_PER_POINT = 3;
	#endregion
	
	#region Modifiers
	internal static bool LIFE_BONUS_HP_IS_FLAT_FIRST = true;
	internal static bool ARPEN_REDUCTION_IS_FLAT_FIRST = false;
	
	internal static bool ARMOR_REDUCTION_IS_FLAT_FIRST = false;
	internal static bool ARMOR_SCORE_IS_FLAT_FIRST = true;
	
	internal static bool ATTRIBUTES_SCORE_IS_FLAT_FIRST = true;
	
	internal static bool MOVE_SPEED_IS_FLAT_FIRST = true;
	
	internal static bool DAMAGE_BONUS_IS_FLAT_FIRST = true;
	#endregion
}
