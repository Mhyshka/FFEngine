using UnityEngine;
using System.Collections;

public class UnitDefense : AUnitComponent
{
	#region Defensive Stats
	internal Armor general = null;
	
		internal Armor physical = null;
			internal Armor slashing = null;
			internal Armor crushing = null;
			internal Armor piercing = null;
	
		internal Armor magic = null;
			internal Armor fire = null;
			internal Armor frost = null;
			internal Armor lightning = null;
			
		internal Armor bleed = null;
		internal Armor poison = null;
		internal Armor spirit = null;
	#endregion
	
	internal override void Init (AInteractable a_unit)
	{
		base.Init (a_unit);
		
		if(_unit.UnitConf != null && _unit.UnitConf.defense != null)
		{
			general = _unit.UnitConf.defense.general.Compute();
			
			physical = _unit.UnitConf.defense.physical.Compute();
			slashing = _unit.UnitConf.defense.slashing.Compute();
			crushing = _unit.UnitConf.defense.crushing.Compute();
			piercing = _unit.UnitConf.defense.piercing.Compute();
			
			magic = _unit.UnitConf.defense.magic.Compute();
			fire = _unit.UnitConf.defense.fire.Compute();
			frost = _unit.UnitConf.defense.frost.Compute();
			lightning = _unit.UnitConf.defense.lightning.Compute();
			
			bleed = _unit.UnitConf.defense.bleed.Compute();
			poison = _unit.UnitConf.defense.poison.Compute();
			spirit = _unit.UnitConf.defense.spirit.Compute();
		}
		else
		{
			Debug.LogWarning("Unit Conf Issue in UnitDefense.");
		}
	}
	
	#region Armor
	protected Resistance _currentResistance = new Resistance();
	internal Resistance GetResistance(EDamageType type)
	{
		_currentResistance.armor = 0;
		_currentResistance.flat = 0;
		
		switch(type)
		{
		//PHYSICAL
			case EDamageType.Slashing :
				_currentResistance.Add(general);
				_currentResistance.Add(physical);
				_currentResistance.Add(slashing);
				break;
				
			case EDamageType.Crushing :
				_currentResistance.Add(general);
				_currentResistance.Add(physical);
				_currentResistance.Add(crushing);
				break;
				
			case EDamageType.Piercing :
				_currentResistance.Add(general);
				_currentResistance.Add(physical);
				_currentResistance.Add(piercing);
				break;
				
		//MAGIC
			case EDamageType.Fire :
				_currentResistance.Add(general);
				_currentResistance.Add(magic);
				_currentResistance.Add(fire);
				break;
				
			case EDamageType.Frost :
				_currentResistance.Add(general);
				_currentResistance.Add(magic);
				_currentResistance.Add(frost);
				break;
				
			case EDamageType.Ligthing :
				_currentResistance.Add(general);
				_currentResistance.Add(magic);
				_currentResistance.Add(lightning);
				break;
				
		//STANDALONE
			case EDamageType.Bleed :
				_currentResistance.Add(general);
				//_currentResistance.Add(physical);
				_currentResistance.Add(bleed);
				break;
				
			case EDamageType.Poison :
				_currentResistance.Add(general);
				//_currentResistance.Add(magic);
				_currentResistance.Add(poison);
				break;
				
			case EDamageType.Spirit :
				_currentResistance.Add(general);
				//_currentResistance.Add(magic);
				_currentResistance.Add(spirit);
				break;
		}
		
		return _currentResistance;
	}
	#endregion
	
	#region Damage management
	internal bool ShouldScratch(EffectDamage a_dmg)
	{
		float rand = Random.value * 100f;
		int armorAfterReduction = a_dmg.arpen.Compute(GetResistance(a_dmg.type).armor, FFEngine.Game.Constants.ARMOR_REDUCTION_FROM_ARPEN_IS_FLAT_FIRST);
		if(rand < Mathf.Clamp(armorAfterReduction, 0f, 100f))
		{
			return true;
		}
		
		return false;
	}
	#endregion
}

public class Armor
{
	internal IntModified armor = null;
	internal IntModified flat = null;
	
	internal Armor()
	{
		armor = new IntModified();
		armor.bonusIsFlatFirst = FFEngine.Game.Constants.ARMOR_MODIFIED_CONF.bonus;
		armor.malusIsFlatFirst = FFEngine.Game.Constants.ARMOR_MODIFIED_CONF.malus;
		armor.reducStackMethod = FFEngine.Game.Constants.ARMOR_MODIFIED_CONF.stack;
		
		flat = new IntModified();
		flat.bonusIsFlatFirst = FFEngine.Game.Constants.ARMOR_MODIFIED_CONF.bonus;
		flat.malusIsFlatFirst = FFEngine.Game.Constants.ARMOR_MODIFIED_CONF.malus;
		flat.reducStackMethod = FFEngine.Game.Constants.ARMOR_MODIFIED_CONF.stack;
	}
}

public class ArmorConf
{
	/// <summary>
	/// The armor. Converted into % damage reduction.
	/// </summary>
	public int armor = 0;
	
	/// <summary>
	/// The flat reduction. Reduce the damage received by this amount.
	/// </summary>
	public int flat = 0;
	
	internal Armor Compute()
	{
		Armor result = new Armor();
		
		result.armor.BaseValue = armor;
		result.flat.BaseValue = flat;
		
		return result;
	}
}

public class Resistance
{
	internal int armor = 0;
	internal int flat = 0;
	
	internal void Add(Armor a_armor)
	{
		armor += a_armor.armor.Value;
		flat += a_armor.flat.Value;
	}
	
	internal IntModifier Compute(IntModifier a_arpen)
	{
		IntModifier reduc = new IntModifier();
		reduc.percent = GameConstants.ArmorPercentReduction(armor, a_arpen);
		reduc.flat = flat - Mathf.CeilToInt(flat * a_arpen.percent);
		return reduc;
	}
}


