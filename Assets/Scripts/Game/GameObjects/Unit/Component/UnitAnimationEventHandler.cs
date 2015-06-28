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
		//AttackWrapper wrapper = _unit.stats.attack.basicAttack.Compute(_unit);
		//_unit.OnAttackDelivered(wrapper);
	}
	#endregion
}
