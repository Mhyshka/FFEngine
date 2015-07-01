using UnityEngine;
using System.Collections;

public class UnitMovement : AUnitComponent
{
	#region Inspector Properties
	public FloatModified speed = null;
	#endregion

	#region Properties
	#endregion

	#region Methods
	internal override void Init (AInteractable a_unit)
	{
		base.Init (a_unit);
		speed.isFlatFirst = GameConstants.MOVE_SPEED_IS_FLAT_FIRST;
	}
	#endregion

}
