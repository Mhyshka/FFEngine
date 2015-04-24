using UnityEngine;
using System.Collections;

public class UnitAnimationEventHandler : MonoBehaviour
{
	#region Inspector Properties
	#endregion

	#region Properties
	private Unit _unit;
	#endregion

	#region Methods
	internal void Init(Unit a_unit)
	{
		_unit = a_unit;
	}
	
	public void OnAttackStrike()
	{
		_unit.OnAttackComplete(_unit.stats.attack.basicAttack);
	}
	#endregion
}
