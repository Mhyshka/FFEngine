using UnityEngine;
using System.Collections;

public enum EUnitAttribute
{
	Strength,
	Agility,
	Charisma,
	Intelligence,
	Willpower,
	Perception
}

[System.Serializable]
public class IntUnitAttribute : IntValue
{
	#region Inspector Properties
	public Unit 			unit 		= null;
	public EUnitAttribute	attribute 	= EUnitAttribute.Strength;
	#endregion

	#region Properties
	#endregion

	#region Methods
	internal override int Value
	{
		get
		{
			int result = 0;
			if(unit != null)
			{
				switch(attribute)
				{
					case EUnitAttribute.Strength :
					//result = unit.stats.attributes.strength.Value;
					break;
					
					case EUnitAttribute.Agility :
					//result = unit.stats.attributes.agility.Value;
					break;
					
					case EUnitAttribute.Charisma :
					//result = unit.stats.attributes.charisma.Value;
					break;
					
					case EUnitAttribute.Intelligence :
					//result = unit.stats.attributes.intelligence.Value;
					break;
					
					case EUnitAttribute.Willpower : 
					//result = unit.stats.attributes.willpower.Value;
					break;
					
					case EUnitAttribute.Perception :
					//result = unit.stats.attributes.perception.Value;
					break;
				}
			}
			
			return result;
		}
	}
	#endregion

}
