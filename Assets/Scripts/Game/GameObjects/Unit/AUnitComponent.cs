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

internal interface ITargettingCallbacks
{
	#region Target Callbacks
	/// <summary>
	/// Called when this GO is targetted by another GO
	/// </summary>
	void OnTargetted(Unit a_other, AOrder a_order);
	
	/// <summary>
	/// Called when this GO targets another GO
	/// </summary>
	void OnTargetAcquire(Unit a_target, AOrder a_order);
	
	/// <summary>
	/// Called when this GO's target is lost
	/// </summary>
	void OnTargetLost(Unit a_target, AOrder a_order);
	
	/// <summary>
	/// Called when this GO's target dies
	/// </summary>
	void OnTargetDeath(Unit a_target, AOrder a_order);
	#endregion
}
	
internal interface IStatusCallbacks
{
	#region Status Callbacks
	/// <summary>
	/// Called when this GO dies
	/// </summary>
	void OnDeath();
	#endregion
}
	
internal interface IDamagesCallbacks
{
	#region Damages Callbacks
	/// <summary>
	/// Called when this GO have some damages applied on him
	/// </summary>
	void OnDamageTaken(DamageReport a_report);
	
	/// <summary>
	/// Called when this GO inflict some damages
	/// </summary>
	void OnDamageDealt(Unit a_unit, DamageReport a_report);
	
	/// <summary>
	/// Called when this unit receive an attack.
	/// </summary>
	void OnAttackReceived(AttackWrapper a_attack);
	
	/// <summary>
	/// Called just before when this unit sends an attack.
	/// </summary>
	void OnAttackDelivered(Unit a_target, AttackWrapper a_attack);
	
	/// <summary>
	/// Called when an attack is thrown
	/// </summary>
	void OnAttackThrown(AttackConf a_attack);
	#endregion
	
}

internal interface IMovementCallbacks
{
	#region Movement Callbacks
	/*/// <summary>
	/// Called when this GO starts moving toward a destination.
	/// </summary>
	void OnFacingChange(Quaternion a_rotation);*/
	
	/// <summary>
	/// Called when this GO starts moving toward a destination.
	/// </summary>
	void OnStartsMoving(Vector3 a_destination);
	
	/// <summary>
	/// Called when this GO stops moving.
	/// </summary>
	void OnStopsMoving();
	
	/// <summary>
	/// Called when this GO changes its destination
	/// </summary>
	void OnDestinationUpdated(Vector3 a_destination);
	
	/// <summary>
	/// Called when the unit reaches its destination
	/// </summary>
	void OnDestinationReached(Vector3 a_destination);
	#endregion
}

internal interface IOrderCallbacks
{
	#region Orders Callbacks
	/// <summary>
	/// Called whenever this GO receive an order
	/// </summary>
	void OnNewOrderReceived(AOrder a_order);
	
	/// <summary>
	/// Called whenever this GO receive an aggressive order
	/// </summary>
	void OnAttackOrderReceived(AOrder a_order);
	
	/// <summary>
	/// Called when this GO receive a move order.
	/// </summary>
	void OnMoveOrderReceived(AOrder a_order, Vector3 a_destination);
	
	/// <summary>
	/// Called when this GO receive a STOP order.
	/// </summary>
	void OnStopOrderReceived(StopOrder a_order);
	
	/// <summary>
	/// Called when this GO's order is complete or becomes invalid.
	/// </summary>
	void OnOrderCompleted(AOrder a_order);
	#endregion
}

internal interface IStatsModificationCallbacks
{
	#region Orders Callbacks
	/// <summary>
	/// Called whenver this unit stats are modified
	/// </summary>
	void OnStatsModification();
	#endregion
}
