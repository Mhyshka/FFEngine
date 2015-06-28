using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class UnitAnimation : AUnitComponent
{
	public enum EState
	{
		Idle = 0,
		Move = 1,
		Attack = 2
	}
	
	public enum EMovementType
	{
		Run = 0,
		Walk = 1
	}
	public enum EIdleType
	{
		Stand = 0,
		Sit = 1
	}
	
	public enum EAttackType
	{
		RightHandWeapon = 0,
		RightHandUnarmed = 1,
		
		LeftHandWeapon = 2,
		LeftHandUnarmed = 3,
		
		TwoHandedWeapon = 4,
		TwoHandedUnarmed = 5
	}
	
	
	#region Inspector Properties
	#endregion

	#region Properties
	internal Animator animator = null;
	
	protected EState _state;
	internal EState State
	{
		get
		{
			return _state;
		}
		set
		{
			if(value != _state)
			{
				animator.SetInteger("State",(int)value);
				animator.SetTrigger("StateChange");
			}
		}
	}
	#endregion

	#region Methods
	internal override void Init (AInteractable a_unit)
	{
		base.Init (a_unit);
		animator = GetComponent<Animator>();
	}
	#endregion
}
