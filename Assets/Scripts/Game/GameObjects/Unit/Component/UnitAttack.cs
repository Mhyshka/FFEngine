using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitAttack : AUnitComponent
{
	#region Properties
	internal Attack basicAttack = null;
	#endregion
	
	internal override void Init (AInteractable a_unit)
	{
		base.Init (a_unit);
		
		if(_unit.UnitConf != null && _unit.UnitConf.attack != null)
		{
			basicAttack = _unit.UnitConf.attack.basicAttack.Compute();
			
			bonusPhysical = _unit.UnitConf.attack.physicalBonus.Compute();
			bonusMagical = _unit.UnitConf.attack.magicalBonus.Compute();
			
			criticalBonus = _unit.UnitConf.attack.criticalBonus.Compute();
			criticalBonus.critChances.BaseValue += FFEngine.Game.Constants.CRITICAL_BASE_CHANCE;
			criticalBonus.damage.percent.BaseValue += FFEngine.Game.Constants.CRITICAL_DAMAGE_BONUS_PERCENT;
			criticalBonus.damage.flat.BaseValue += FFEngine.Game.Constants.CRITICAL_DAMAGE_BONUS_FLAT;
			criticalBonus.arpen.percent.BaseValue += FFEngine.Game.Constants.CRITICAL_ARMOR_PERCENT_REDUCTION;
			criticalBonus.arpen.flat.BaseValue += FFEngine.Game.Constants.CRITICAL_ARMOR_FLAT_REDUCTION;	
			
			penetrationBonus = _unit.UnitConf.attack.penetratingBonus.Compute();
			penetrationBonus.critChances.BaseValue += FFEngine.Game.Constants.PENETRATION_BASE_CHANCES;
			penetrationBonus.damage.percent.BaseValue += FFEngine.Game.Constants.PENETRATION_DAMAGE_BONUS_PERCENT;
			penetrationBonus.damage.flat.BaseValue += FFEngine.Game.Constants.PENETRATION_DAMAGE_BONUS_FLAT;
			penetrationBonus.arpen.percent.BaseValue += FFEngine.Game.Constants.PENETRATION_ARMOR_PERCENT_REDUCTION;
			penetrationBonus.arpen.flat.BaseValue += FFEngine.Game.Constants.PENETRATION_ARMOR_FLAT_REDUCTION;	
		}
		else
		{
			Debug.LogWarning("Unit Conf Issue in UnitAttack.");
		}
	}
	
	#region Armor Penetration
	#endregion
	
	#region Bonus Physical Damages
	internal AttackBonus bonusPhysical = null;
	internal IntModifier BonusPhysicalDamages
	{
		get
		{
			//bonusPhysical.damage.percent.BaseValue = _unit.stats.strength.Value * FFEngine.Game.Constants.STRENGTH_DAMAGE_PERCENT_PER_POINT;
			return bonusPhysical.damage.Value;
		}
	}
	
	internal IntModifier BonusPhysicalArpen
	{
		get
		{
			//bonusPhysical.arpen.flat.BaseValue = Mathf.FloorToInt(_unit.stats.strength.Value * FFEngine.Game.Constants.STRENGTH_ARPEN_PER_POINT);
			return bonusPhysical.arpen.Value;
		}
	}
	#endregion
	
	#region Bonus Magical Damages
	internal AttackBonus bonusMagical = null;
	internal IntModifier BonusMagicDamages
	{
		get
		{
			//bonusMagical.damage.percent.BaseValue = _unit.stats.intelligence.Value * FFEngine.Game.Constants.STRENGTH_DAMAGE_PERCENT_PER_POINT;
			return bonusMagical.damage.Value;
		}
	}
	
	internal IntModifier BonusMagicArpen
	{
		get
		{
			return bonusMagical.arpen.Value;
		}
	}
	#endregion
	
	#region Critical
	internal CritBonus criticalBonus = null;
	internal float CriticalChances
	{
		get
		{
			//criticalBonus.critChances.BaseValue += _unit.stats.agility.Value * FFEngine.Game.Constants.AGILITY_CRITICAL_CHANCE_PER_POINT;
			return criticalBonus.critChances.Value;
		}
	}
	
	internal IntModifier CriticalDamages
	{
		get
		{
			
			return criticalBonus.damage.Value;
		}
	}
	
	internal IntModifier CriticalArpen
	{
		get
		{
					
			return criticalBonus.arpen.Value;
		}
	}
	#endregion
	
	#region Penetrating
	internal CritBonus penetrationBonus = null;
	internal float PenetrationChances
	{
		get
		{
			//penetrationBonus.critChances.BaseValue += _unit.stats.agility.Value * FFEngine.Game.Constants.AGILITY_PENETRATING_CHANCE_PER_POINT;
			return penetrationBonus.critChances.Value;
		}
	}

	
	internal IntModifier PenetrationDamages
	{
		get
		{
			return penetrationBonus.damage.Value;
		}
	}
	
	internal IntModifier PenetrationArpen
	{
		get
		{		
			return penetrationBonus.arpen.Value;
		}
	}
	#endregion
	
	#region Attack
	internal Attack BasicAttack
	{
		get
		{
			return basicAttack;
		}
	}
	
	/// <summary>
	/// Check for possible targets and send them the attack.
	/// </summary>
	internal void FireAttack(Attack a_attack)
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

public class BonusArpen : IntModifiedModifier
{
	internal BonusArpen()
	{
		percent = new FloatModified();
		flat = new IntModified();
		
		if(Application.isPlaying)
		{
			percent.bonusIsFlatFirst = FFEngine.Game.Constants.ARPEN_MODIFIED_CONF.bonus;
			percent.malusIsFlatFirst = FFEngine.Game.Constants.ARPEN_MODIFIED_CONF.malus;
			percent.reducStackMethod = FFEngine.Game.Constants.ARPEN_MODIFIED_CONF.stack;
			
			
			flat.bonusIsFlatFirst = FFEngine.Game.Constants.ARPEN_MODIFIED_CONF.bonus;
			flat.malusIsFlatFirst = FFEngine.Game.Constants.ARPEN_MODIFIED_CONF.malus;
			flat.reducStackMethod = FFEngine.Game.Constants.ARPEN_MODIFIED_CONF.stack;
		}
	}
}

public class BonusDamage : IntModifiedModifier
{
	internal BonusDamage()
	{
		percent = new FloatModified();
		flat = new IntModified();
		if(Application.isPlaying)
		{
			percent.bonusIsFlatFirst = FFEngine.Game.Constants.DAMAGE_MODIFIED_CONF.bonus;
			percent.malusIsFlatFirst = FFEngine.Game.Constants.DAMAGE_MODIFIED_CONF.malus;
			percent.reducStackMethod = FFEngine.Game.Constants.DAMAGE_MODIFIED_CONF.stack;
			
			flat.bonusIsFlatFirst = FFEngine.Game.Constants.DAMAGE_MODIFIED_CONF.bonus;
			flat.malusIsFlatFirst = FFEngine.Game.Constants.DAMAGE_MODIFIED_CONF.malus;
			flat.reducStackMethod = FFEngine.Game.Constants.DAMAGE_MODIFIED_CONF.stack;
		}
		
	}
}

public class AttackBonus
{
	internal BonusDamage damage = null;
	internal BonusArpen arpen = null;
	
	internal AttackBonus()
	{
		damage = new BonusDamage();
		arpen = new BonusArpen();
	}
}

public class CritBonus : AttackBonus
{
	internal FloatModified critChances = null;
	
	internal CritBonus() : base()
	{
		critChances = new FloatModified();
		if(Application.isPlaying)
		{
			critChances.bonusIsFlatFirst = FFEngine.Game.Constants.CRIT_CHANCES_MODIFIED_CONF.bonus;
			critChances.malusIsFlatFirst = FFEngine.Game.Constants.CRIT_CHANCES_MODIFIED_CONF.malus;
			critChances.reducStackMethod = FFEngine.Game.Constants.CRIT_CHANCES_MODIFIED_CONF.stack;
		}
	}
}
