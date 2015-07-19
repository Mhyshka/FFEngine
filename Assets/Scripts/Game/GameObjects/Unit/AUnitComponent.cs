using UnityEngine;
using System.Collections;

public abstract class AUnitComponent : AInteractableComponent
{
	#region Inspector Properties
	#endregion

	#region Properties
	protected Unit _unit;
	#endregion

	#region Methods
	internal override void Init(AInteractable a_unit)
	{
		base.Init(a_unit);
		_unit = a_unit as Unit;
	}
	#endregion
}