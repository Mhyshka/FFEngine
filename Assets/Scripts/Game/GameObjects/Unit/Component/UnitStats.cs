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
		
		strength.isFlatFirst = FFEngine.Game.Constants.ATTRIBUTES_SCORE_IS_FLAT_FIRST;
		agility.isFlatFirst = FFEngine.Game.Constants.ATTRIBUTES_SCORE_IS_FLAT_FIRST;
		
		intelligence.isFlatFirst = FFEngine.Game.Constants.ATTRIBUTES_SCORE_IS_FLAT_FIRST;
		spirit.isFlatFirst = FFEngine.Game.Constants.ATTRIBUTES_SCORE_IS_FLAT_FIRST;
		
		charisma.isFlatFirst = FFEngine.Game.Constants.ATTRIBUTES_SCORE_IS_FLAT_FIRST;
	}
}
