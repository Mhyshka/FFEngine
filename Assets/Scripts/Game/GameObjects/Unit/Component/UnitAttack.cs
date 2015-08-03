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
	}
	
	#region Armor Penetration
	#endregion
	
	#region Bonus Physical Damages
	internal IntModifier BonusPhysicalDamages
	{
		get
		{
			IntModifier modifier = new IntModifier();
			modifier.percent += _unit.stats.strength.Value * FFEngine.Game.Constants.STRENGTH_DAMAGE_PERCENT_PER_POINT;
			return modifier;
		}
	}
	
	internal IntModifier BonusPhysicalPenetration
	{
		get
		{
			IntModifier reduc = new IntModifier();
			reduc.flat += Mathf.FloorToInt(_unit.stats.strength.Value * FFEngine.Game.Constants.STRENGTH_ARPEN_PER_POINT);
			return reduc;
		}
	}
	#endregion
	
	#region Bonus Magical Damages
	internal IntModifier BonusMagicDamages
	{
		get
		{
			IntModifier modifier = new IntModifier();
			modifier.percent += _unit.stats.intelligence.Value * FFEngine.Game.Constants.STRENGTH_DAMAGE_PERCENT_PER_POINT;
			
			return modifier;
		}
	}
	
	internal IntModifier BonusMagicPenetration
	{
		get
		{
			IntModifier reduc = new IntModifier();
			return reduc;
		}
	}
	#endregion
	
	#region Critical
	internal CritBonus criticalBonus = null;
	internal float CriticalChances
	{
		get
		{
			float result = FFEngine.Game.Constants.CRITICAL_BASE_CHANCE;
			result += _unit.stats.agility.Value * FFEngine.Game.Constants.AGILITY_CRITICAL_CHANCE_PER_POINT;
			return result;
		}
	}
	
	internal IntModifier CriticalDamages
	{
		get
		{
			IntModifier modifier = new IntModifier();
			modifier.percent = FFEngine.Game.Constants.CRITICAL_DAMAGE_BONUS_PERCENT;
			modifier.flat = FFEngine.Game.Constants.CRITICAL_DAMAGE_BONUS_FLAT;
			return modifier;
		}
	}
	
	internal IntModifier CriticalArpen
	{
		get
		{
			IntModifier reduction = new IntModifier();
			reduction.percent = FFEngine.Game.Constants.CRITICAL_ARMOR_PERCENT_REDUCTION;
			reduction.flat = FFEngine.Game.Constants.CRITICAL_ARMOR_FLAT_REDUCTION;
			
			return reduction;
		}
	}
	#endregion
	
	#region Penetrating
	internal CritBonus penetrationBonus = null;
	internal float PenetrationChances
	{
		get
		{
			float result = FFEngine.Game.Constants.PENETRATION_BASE_CHANCES;
			result += _unit.stats.agility.Value * FFEngine.Game.Constants.AGILITY_PENETRATING_CHANCE_PER_POINT;
			return result;
		}
	}

	
	internal IntModifier PenetrationDamages
	{
		get
		{
			IntModifier modifier = new IntModifier();
			modifier.percent = FFEngine.Game.Constants.PENETRATION_DAMAGE_BONUS_PERCENT;
			modifier.flat = FFEngine.Game.Constants.PENETRATION_DAMAGE_BONUS_FLAT;
			
			return modifier;
		}
	}
	
	internal IntModifier PenetrationArpen
	{
		get
		{
			IntModifier arpen = new IntModifier();
			arpen.percent = FFEngine.Game.Constants.CRITICAL_ARMOR_PERCENT_REDUCTION;
			arpen.flat = FFEngine.Game.Constants.CRITICAL_ARMOR_FLAT_REDUCTION;

			return arpen;
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
				
			case EDamageType.Fire :
			case EDamageType.Frost :
			case EDamageType.Ligthing :
				mod = BonusMagicDamages;
				break;
				
			default : mod = new IntModifier();
				break;
		}
		
		return mod;
	}
	
	internal IntModifier GetPenetration(EDamageType a_type)
	{
		IntModifier result;
		
		switch(a_type)
		{
		case EDamageType.Slashing :
		case EDamageType.Crushing :
		case EDamageType.Piercing : 
			result = new IntModifier();
			break;
			
		case EDamageType.Fire :
		case EDamageType.Frost :
		case EDamageType.Ligthing :
			result = new IntModifier();
			break;
			
		default : result = new IntModifier();
			break;
		}
		
		return result;
	}
	#endregion
}

public class BonusArpen
{
	internal FloatModified percent = null;
	internal IntModified flat = null;
	
	internal BonusArpen()
	{
		percent = new FloatModified();
		percent.bonusIsFlatFirst = FFEngine.Game.Constants.ARPEN_MODIFIED_CONF.bonus;
		percent.malusIsFlatFirst = FFEngine.Game.Constants.ARPEN_MODIFIED_CONF.malus;
		percent.reducStackMethod = FFEngine.Game.Constants.ARPEN_MODIFIED_CONF.stack;
	}
	
	internal IntModifier Reduction
	{
		get
		{
			IntModifier reduction = new IntModifier();
			reduction.percent = FFEngine.Game.Constants.PENETRATION_ARMOR_PERCENT_REDUCTION;
			reduction.percent += percent.Value;
			
			reduction.flat = FFEngine.Game.Constants.PENETRATION_ARMOR_FLAT_REDUCTION;
			reduction.flat += flat.Value;
			
			return reduction;
		}
	}
}

public class AttackBonus
{
	internal IntModified damage = null;
	internal BonusArpen arpen = null;
	
	internal AttackBonus(int a_baseDamage)
	{
		arpen = new BonusArpen();
		
		damage = new IntModified();
		damage.bonusIsFlatFirst = FFEngine.Game.Constants.DAMAGE_MODIFIED_CONF.bonus;
		damage.malusIsFlatFirst = FFEngine.Game.Constants.DAMAGE_MODIFIED_CONF.malus;
		damage.reducStackMethod = FFEngine.Game.Constants.DAMAGE_MODIFIED_CONF.stack;
	}
}

public class CritBonus : AttackBonus
{
	internal FloatModified critChances = null;
	
	internal CritBonus(int a_baseDamage, float a_baseCritChance) : base(a_baseDamage)
	{
		critChances = new FloatModified();
		// NO PERCENT MODIFICATION ON CRIT STATS. Only flat that already grants %.
	}
}
