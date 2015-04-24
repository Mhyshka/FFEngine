using UnityEngine;
using System.Collections;

[System.Serializable]
public class UnitVisual : AUnitComponent
{
	#region Inspector Properties
	public GameObject model = null;
	public Animator animator = null;
	public UnitAnimationEventHandler animationEventHandler = null;
	#endregion

	#region Properties
	#endregion

	#region Methods
	internal override void Init (Unit a_unit)
	{
		base.Init (a_unit);
		animationEventHandler.Init(a_unit);
	}
	internal void PlayAttackAnimation()
	{
		animator.SetTrigger("Attack");
		animator.SetTrigger("Strike");
	}
	#endregion

}
