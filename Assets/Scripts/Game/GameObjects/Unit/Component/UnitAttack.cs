using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitAttack : AUnitComponent
{
	#region Inspector Properties
	public AttackConf basicAttack = null;
	#endregion
	
	internal override void Init (AInteractable a_unit)
	{
		base.Init (a_unit);
		bonusPhysicalDamage = new IntModifier();;
		bonusPhysicalArpen = new FloatModifier();
		
		bonusMagicArpen = new FloatModifier();
		bonusMagicDamage = new IntModifier();
	}
	
	#region Armor Penetration
	internal FloatModifier bonusPhysicalArpen = null;
	internal FloatModifier bonusMagicArpen = null;
	
	internal Reduction BonusPhysicalPenetration
	{
		get
		{
			Reduction reduc = new Reduction();
			reduc.flat 		+= bonusPhysicalArpen.flat;
			reduc.percent 	+= bonusPhysicalArpen.percent;
			
			reduc.flat += _unit.stats.strength.Value * FFEngine.Game.Constants.STRENGTH_ARPEN_PER_POINT;
			
			return reduc;
		}
	}
	
	internal Reduction BonusMagicPenetration
	{
		get
		{
			Reduction reduc = new Reduction();
			reduc.flat 		+= bonusMagicArpen.flat;
			reduc.percent 	+= bonusMagicArpen.percent;
			
			return reduc;
		}
	}
	
	internal Reduction GetPenetration(EDamageType a_type)
	{
		Reduction result;
		
		switch(a_type)
		{
		case EDamageType.Slashing :
		case EDamageType.Crushing :
		case EDamageType.Piercing : 
			result = BonusPhysicalPenetration;
			break;
			
		case EDamageType.Magic :
			result = BonusMagicPenetration;
			break;
			
		default : result = new Reduction();
			break;
		}
		
		return result;
	}
	#endregion
	
	#region Bonus Damages
	internal IntModifier   bonusPhysicalDamage = null;
	internal IntModifier   bonusMagicDamage = null;
	
	internal IntModifier BonusPhysicalDamages
	{
		get
		{
			IntModifier modifier = new IntModifier();
			modifier.flat += bonusPhysicalDamage.flat;
			modifier.percent += bonusPhysicalDamage.percent;
			
			modifier.percent += _unit.stats.strength.Value * FFEngine.Game.Constants.STRENGTH_DAMAGE_PERCENT_PER_POINT;
			
			return modifier;
		}
	}
	
	internal IntModifier BonusMagicDamages
	{
		get
		{
			IntModifier modifier = new IntModifier();
			modifier.flat += bonusMagicDamage.flat;
			modifier.percent += bonusMagicDamage.percent;
			
			modifier.percent += _unit.stats.intelligence.Value * FFEngine.Game.Constants.STRENGTH_DAMAGE_PERCENT_PER_POINT;
			
			return modifier;
		}
	}
	
	internal IntModifier GetBonusDamage(EDamageType a_type)
	{
		IntModifier mod = null;
		
		switch(a_type)
		{
		case EDamageType.Slashing :
		case EDamageType.Crushing :
		case EDamageType.Piercing : 
			mod = BonusPhysicalDamages;
			break;
			
		case EDamageType.Magic :
			mod = BonusMagicDamages;
			break;
			
		default : mod = new IntModifier();
			break;
		}
		
		return mod;
	}
	#endregion
	
	#region Critical
	internal float	   		   bonusCriticalChance = 0f;
	internal float	   		   bonusCriticalDamage = 0;
	internal float			   bonusCriticalArpen = 0f;
	
	internal float CriticalChances
	{
		get
		{
			return _unit.stats.agility.Value * FFEngine.Game.Constants.AGILITY_CRITICAL_CHANCE_PER_POINT + bonusCriticalChance;
		}
	}
	
	internal float CriticalDamages
	{
		get
		{
			return FFEngine.Game.Constants.CRITICAL_DAMAGE_BONUS_PERCENT + bonusCriticalDamage;
		}
	}
	
	internal float CriticalArpen
	{
		get
		{
			return FFEngine.Game.Constants.CRITICAL_ARMOR_REDUCTION - bonusCriticalArpen;
		}
	}
	#endregion
	
	#region Penetrating
	internal float	  		   bonusPenetrationChance = 0f;
	internal float	  		   bonusPenetrationDamage = 0;
	internal float			   bonusPenetrationArpen = 0f;
	
	internal float PenetrationChances
	{
		get
		{
			return _unit.stats.agility.Value * FFEngine.Game.Constants.AGILITY_PENETRATING_CHANCE_PER_POINT + bonusPenetrationChance;
		}
	}
	
	internal float PenetrationDamages
	{
		get
		{
			return FFEngine.Game.Constants.PENETRATION_DAMAGE_BONUS_PERCENT + bonusPenetrationDamage;
		}
	}
	
	internal float PenetrationArpen
	{
		get
		{
			return FFEngine.Game.Constants.PENETRATION_ARMOR_REDUCTION - bonusPenetrationArpen;
		}
	}
	#endregion
	
	#region Attack
	/// <summary>
	/// Check for possible targets and send them the attack.
	/// </summary>
	internal void FireAttack(AttackConf a_attack)
	{
		Vector3 pos = a_attack.TargetPosition(_unit);
		List<Unit> targets = a_attack.SeekTargets(_unit, pos);
		if(targets.Count > 0)
		{
			AttackInfos attackInfos = new AttackInfos();
			attackInfos.affectedTargets = targets;
			attackInfos.targetPosition = pos;
			attackInfos.critType = TryToCrit();
			attackInfos.source = _unit;
			
			AttackWrapper wrapper = a_attack.Compute(attackInfos);
			foreach(Unit each in targets)
			{
				Debug.Log(wrapper.Apply(each).ToString());
			}
		}
	}
	
	internal ECriticalType TryToCrit()
	{
		ECriticalType critType = ECriticalType.Normal;
		
		float rand = Random.value;
		if(rand <= CriticalChances)
		{
			critType = ECriticalType.Crititcal;
		}
		else if(rand <= PenetrationChances)
		{
			critType = ECriticalType.Penetration;
		}
	
		return critType;
	}
	#endregion
}
