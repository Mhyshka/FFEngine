using UnityEngine;
using System.Collections;

public class UnitStats : AUnitComponent
{	
	#region Attributes
	public IntModified strength = null;
	public IntModified agility = null;
	
	public IntModified intelligence = null;
	public IntModified spirit = null;
	
	public IntModified charisma = null;
	#endregion
	
	#region Others
	public int level = 1;
	#endregion
	
	internal override void Init (AInteractable a_unit)
	{
		base.Init (a_unit);
		
		PrepareAttributeIntModified(strength);
		PrepareAttributeIntModified(agility);
		PrepareAttributeIntModified(intelligence);
		PrepareAttributeIntModified(spirit);
		PrepareAttributeIntModified(charisma);
	}
	
	static internal void PrepareAttributeIntModified(IntModified a_mod)
	{
		/*a_mod.bonusIsFlatFirst = FFEngine.Game.Constants.ATTRIBUTES_BONUS_IS_FLAT_FIRST;
		a_mod.malusIsFlatFirst = FFEngine.Game.Constants.ATTRIBUTES_MALUS_IS_FLAT_FIRST;
		a_mod.reducStack = FFEngine.Game.Constants.ATTRIBUTES_REDUC_STACK;*/
	}
}
