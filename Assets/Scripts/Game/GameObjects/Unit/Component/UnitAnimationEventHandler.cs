using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class UnitAnimationEventHandler : AUnitComponent
{
	#region Inspector Properties
	#endregion

	#region Properties
	#endregion

	#region Methods
	
	/// <summary>
	/// Animation event : when the attack animation should apply damage.
	/// </summary>
	internal void OnAttackStrike()
	{
		_unit.attack.FireAttack(_unit.attack.BasicAttack);
	}
	#endregion
}
