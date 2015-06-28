using UnityEngine;
using System.Collections;

[System.Serializable]
public class AttributeStats
{
	#region Inspector Properties
	//Physical
	public IntModified strength = null;
	public IntModified agility = null;
	public IntModified charisma = null;
	
	//Mind
	public IntModified intelligence = null;
	public IntModified willpower = null;
	public IntModified perception = null;
	
	//Spirit
	/*public IntModified strenght = null;
	public IntModified strenght = null;
	public IntModified strenght = null;*/
	#endregion

	#region Armor Penetration
	internal Reduction PhysicalPenetration
	{
		get
		{
			Reduction reduc = new Reduction();
			reduc.flat += strength.Value * 0.5f;
			reduc.flat += agility.Value * 0.5f;
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
				result = PhysicalPenetration;
			break;
			
			default : result = new Reduction();
			break;
		}
		
		return result;
	}
	#endregion
	
	#region Attack Power
	internal IntModifier PhysicalBonusDamages
	{
		get
		{
			IntModifier modifier = new IntModifier();
			
			modifier.flat += Mathf.FloorToInt(strength.Value);
			
			modifier.percent += strength.Value * 0.5f;
			modifier.percent += agility.Value * 0.5f;
			
			return modifier;
		}
	}
	
	internal IntModifier GetPower(EDamageType a_type)
	{
		IntModifier mod = null;
		
		switch(a_type)
		{
			case EDamageType.Slashing :
			case EDamageType.Crushing :
		case EDamageType.Piercing : mod = PhysicalBonusDamages;
			break;
			
			default : mod = new IntModifier();
			break;
		}
		
		return mod;
	}
	#endregion

	#region Methods
	#endregion
}
