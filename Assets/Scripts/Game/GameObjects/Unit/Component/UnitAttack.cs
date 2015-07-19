using UnityEngine;
using System.Collections;

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
			
			reduc.flat += _unit.stats.strength.Value * GameConstants.STRENGTH_ARPEN_PER_POINT;
			
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
			
			modifier.percent += _unit.stats.strength.Value * GameConstants.STRENGTH_DAMAGE_PERCENT_PER_POINT;
			
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
			
			modifier.percent += _unit.stats.intelligence.Value * GameConstants.STRENGTH_DAMAGE_PERCENT_PER_POINT;
			
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
			return _unit.stats.agility.Value * GameConstants.AGILITY_CRITICAL_CHANCE_PER_POINT + bonusCriticalChance;
		}
	}
	
	internal float CriticalDamages
	{
		get
		{
			return GameConstants.CRITICAL_DAMAGE_BONUS_PERCENT + bonusCriticalDamage;
		}
	}
	
	internal float CriticalArpen
	{
		get
		{
			return GameConstants.CRITICAL_ARMOR_REDUCTION - bonusCriticalArpen;
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
			return _unit.stats.agility.Value * GameConstants.AGILITY_PENETRATING_CHANCE_PER_POINT + bonusPenetrationChance;
		}
	}
	
	internal float PenetrationDamages
	{
		get
		{
			return GameConstants.PENETRATION_DAMAGE_BONUS_PERCENT + bonusPenetrationDamage;
		}
	}
	
	internal float PenetrationArpen
	{
		get
		{
			return GameConstants.PENETRATION_ARMOR_REDUCTION - bonusPenetrationArpen;
		}
	}
	#endregion
	
	#region Attack
	internal void ThrowAttack(AttackConf a_attack)
	{
		AttackWrapper wrapper = a_attack.Compute(_unit);
		
		Vector3 pos = transform.position + transform.forward * a_attack.range.Value;
		wrapper.targetPosition = pos;
		
		Collider[] targets = Physics.OverlapSphere(pos, a_attack.areaOfEffect.Value, 1 << LayerMask.NameToLayer("Unit"));
		
		foreach(Collider each in targets)
		{
			Unit victim = each.GetComponent<UnitTarget>().Unit;
			if(victim != _unit)
			{
				if(victim != null)
				{
					_unit.DeliverAttack(victim, wrapper);
				}
			}
		}
	}
	#endregion
}
