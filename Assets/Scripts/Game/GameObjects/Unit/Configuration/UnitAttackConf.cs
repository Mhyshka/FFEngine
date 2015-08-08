using UnityEngine;
using System.Collections;

[System.Serializable]
public class UnitAttackConf
{
	#region Inspector Properties
	public AttackConf basicAttack = null;
	
	public AttackBonusConf physicalBonus = null;
	
	public AttackBonusConf magicalBonus = null;
	
	public CritBonusConf criticalBonus = null;
	public CritBonusConf penetratingBonus = null;
	#endregion
}

[System.Serializable]
public class AttackBonusConf
{
	public IntModifierInspectorConf damage = null;
	public IntModifierInspectorConf arpen = null;
	
	internal AttackBonus Compute()
	{
		AttackBonus bonus = new AttackBonus();
		
		bonus.damage.percent.BaseValue = damage.percent;
		bonus.damage.flat.BaseValue = damage.flat;
		
		bonus.arpen.percent.BaseValue = arpen.percent;
		bonus.arpen.flat.BaseValue = arpen.flat;
		
		return bonus;
	}
}

[System.Serializable]
public class CritBonusConf
{
	public IntModifierInspectorConf damage = null;
	public IntModifierInspectorConf arpen = null;
	public float critChances = 0f;
	
	internal CritBonus Compute()
	{
		CritBonus bonus = new CritBonus();
		
		bonus.damage.percent.BaseValue = damage.percent;
		bonus.damage.flat.BaseValue = damage.flat;
		
		bonus.arpen.percent.BaseValue = arpen.percent;
		bonus.arpen.flat.BaseValue = arpen.flat;
		
		bonus.critChances.BaseValue = critChances;
		
		return bonus;
	}
}
