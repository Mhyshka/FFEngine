using UnityEngine;
using System.Collections;

public class UnitAttributes : AUnitComponent
{	
	#region Attributes
	internal IntModified strength = null;
	internal IntModified agility = null;
	
	internal IntModified intelligence = null;
	internal IntModified perception = null;
	internal IntModified spirit = null;
	
	internal IntModified charisma = null;
	#endregion
	
	#region Others
	internal int level = 1;
	#endregion
	
	internal override void Init (AInteractable a_unit)
	{
		base.Init (a_unit);
		
		if(_unit.UnitConf != null && _unit.UnitConf.attributes != null)
		{
			strength = new IntModified();
			strength.BaseValue = _unit.UnitConf.attributes.strength;
			
			agility = new IntModified();
			agility.BaseValue = _unit.UnitConf.attributes.agility;
			
			intelligence = new IntModified();
			intelligence.BaseValue = _unit.UnitConf.attributes.intelligence;
			
			perception = new IntModified();
			perception.BaseValue = _unit.UnitConf.attributes.perception;
			
			spirit = new IntModified();
			spirit.BaseValue = _unit.UnitConf.attributes.spirit;
			
			charisma = new IntModified();
			charisma.BaseValue = _unit.UnitConf.attributes.charisma;
		}
		else
		{
			Debug.LogWarning("Unit Conf Issue in UnitAttributes.");
		}
	}
}
