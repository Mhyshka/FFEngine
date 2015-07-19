using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : AInteractable, ITargettingCallbacks, IOrderCallbacks, IMovementCallbacks, IDamagesCallbacks, IStatusCallbacks, IStatsModificationCallbacks
{
	#region Inspector Properties
	public UnitStats 			stats = null;
	public UnitAttack 			attack = null;
	public UnitDefense			defense = null;
	public UnitLife 			life = null;
	public new UnitAnimation    animation = null;
	public UnitMovement			movement = null;
	#endregion

	#region Properties
	#endregion
	
	#region Interface Lists
	List<AUnitComponent> _targetCallbacks	 	= new List<AUnitComponent>();
	List<AUnitComponent> _orderCallbacks 		= new List<AUnitComponent>();
	List<AUnitComponent> _movementCallbacks		= new List<AUnitComponent>();
	List<AUnitComponent> _damageCallbacks		= new List<AUnitComponent>();
	List<AUnitComponent> _statusCallbacks		= new List<AUnitComponent>();
	List<AUnitComponent> _statsCallbacks		= new List<AUnitComponent>();
	#endregion

	#region Starts Methods
	protected override void Awake ()
	{
		base.Awake();
	}
	
	protected override void ParseComponent(AInteractableComponent each)
	{		
		base.ParseComponent(each);
		
		AUnitComponent unitComp = each as AUnitComponent;
		if(unitComp != null)
		{
			//INTERFACE PARSING
			if(each is ITargettingCallbacks)
				_targetCallbacks.Add(unitComp);
			if(each is IOrderCallbacks)
				_orderCallbacks.Add(unitComp);
			if(each is IMovementCallbacks)
				_movementCallbacks.Add(unitComp);
			if(each is IDamagesCallbacks)
				_damageCallbacks.Add(unitComp);
			if(each is IStatusCallbacks)
				_statusCallbacks.Add(unitComp);
			if(each is IStatsModificationCallbacks)
				_statsCallbacks.Add (unitComp);
		}
	}
 	#endregion
 
	#region Update Loop
	protected virtual void Update ()
	{
  		if(Input.GetKeyDown(KeyCode.Space))
		{
			animation.State = UnitAnimation.EState.Attack;
		}
	}
	#endregion
	
	#region API
	#endregion
	
	#region Target Callbacks
	public void OnTargetted(Unit a_other, AOrder a_order)
	{
		foreach(AUnitComponent each in _targetCallbacks)
		{
			if(each.enabled)
			{
				((ITargettingCallbacks)each).OnTargetted(a_other, a_order);
			}
		}
	}
	
	public void OnTargetAcquire(Unit a_target, AOrder a_order)
	{
		foreach(AUnitComponent each in _targetCallbacks)
		{
			if(each.enabled)
			{
				((ITargettingCallbacks)each).OnTargetAcquire(a_target, a_order);
			}
		}
	}
	
	public void OnTargetLost(Unit a_target, AOrder a_order)
	{
		foreach(AUnitComponent each in _targetCallbacks)
		{
			if(each.enabled)
			{
				((ITargettingCallbacks)each).OnTargetLost(a_target, a_order);
			}
		}
	}
	
	public void OnTargetDeath(Unit a_target, AOrder a_order)
	{
		foreach(AUnitComponent each in _targetCallbacks)
		{
			if(each.enabled)
			{
				((ITargettingCallbacks)each).OnTargetLost(a_target, a_order);
			}
		}
	}
	#endregion
	
	
	#region Order Callbacks
	public void OnNewOrderReceived (AOrder a_order)
	{
		foreach(AUnitComponent each in _orderCallbacks)
		{
			if(each.enabled)
			{
				((IOrderCallbacks)each).OnNewOrderReceived(a_order);
			}
		}
	}

	public void OnAttackOrderReceived (AOrder a_order)
	{
		OnNewOrderReceived(a_order);
		foreach(AUnitComponent each in _orderCallbacks)
		{
			if(each.enabled)
			{
				((IOrderCallbacks)each).OnAttackOrderReceived(a_order);
			}
		}
	}

	public void OnMoveOrderReceived (AOrder a_order, Vector3 a_destination)
	{
		OnNewOrderReceived(a_order);
		foreach(AUnitComponent each in _orderCallbacks)
		{
			if(each.enabled)
			{
				((IOrderCallbacks)each).OnMoveOrderReceived(a_order, a_destination);
			}
		}
		
		movement.SetDestination(a_destination);
	}

	public void OnStopOrderReceived (StopOrder a_order)
	{
		OnNewOrderReceived(a_order);
		foreach(AUnitComponent each in _orderCallbacks)
		{
			if(each.enabled)
			{
				((IOrderCallbacks)each).OnStopOrderReceived(a_order);
			}
		}
	}

	public void OnOrderCompleted (AOrder a_order)
	{
		foreach(AUnitComponent each in _orderCallbacks)
		{
			if(each.enabled)
			{
				((IOrderCallbacks)each).OnOrderCompleted(a_order);
			}
		}
	}
	#endregion

	#region Movement Callbacks
	public void OnStartsMoving (Vector3 a_destination)
	{
		foreach(AUnitComponent each in _movementCallbacks)
		{
			if(each.enabled)
			{
				((IMovementCallbacks)each).OnStartsMoving(a_destination);
			}
		}	
	}

	public void OnStopsMoving ()
	{
		foreach(AUnitComponent each in _movementCallbacks)
		{
			if(each.enabled)
			{
				((IMovementCallbacks)each).OnStopsMoving();
			}
		}	
	}

	public void OnDestinationUpdated (Vector3 a_destination)
	{
		foreach(AUnitComponent each in _movementCallbacks)
		{
			if(each.enabled)
			{
				((IMovementCallbacks)each).OnDestinationUpdated(a_destination);
			}
		}	
	}

	public void OnDestinationReached (Vector3 a_destination)
	{
		foreach(AUnitComponent each in _movementCallbacks)
		{
			if(each.enabled)
			{
				((IMovementCallbacks)each).OnDestinationReached(a_destination);
			}
		}	
	}
	#endregion

	#region Damage Callbacks
	public void OnDamageTaken (DamageReport a_report)
	{
		foreach(AUnitComponent each in _damageCallbacks)
		{
			if(each.enabled)
			{
				((IDamagesCallbacks)each).OnDamageTaken(a_report);
			}
		}
		
		Debug.Log(a_report.ToString());
		life.Damage(a_report.final);
	}

	public void OnDamageDealt (Unit a_unit, DamageReport a_report)
	{
		foreach(AUnitComponent each in _damageCallbacks)
		{
			if(each.enabled)
			{
				((IDamagesCallbacks)each).OnDamageDealt(a_unit, a_report);
			}
		}	
	}
	#endregion
	
	#region Attack Callbacks
	public void OnAttackReceived(AttackWrapper a_attack)
	{
		foreach(AUnitComponent each in _damageCallbacks)
		{
			if(each.enabled)
			{
				((IDamagesCallbacks)each).OnAttackReceived(a_attack);
			}
		}	
		
		defense.ApplyDamage(a_attack);
	}
	
	public void OnAttackDelivered(Unit a_target, AttackWrapper a_attack)
	{
		foreach(AUnitComponent each in _damageCallbacks)
		{
			if(each.enabled)
			{
				((IDamagesCallbacks)each).OnAttackDelivered(a_target, a_attack);
			}
		}

		a_target.OnAttackReceived(a_attack);
	}
	
	public void OnAttackThrown(AttackConf a_attack)
	{
		foreach(AUnitComponent each in _damageCallbacks)
		{
			if(each.enabled)
			{
				((IDamagesCallbacks)each).OnAttackThrown(a_attack);
			}
		}
		
		attack.ThrowAttack(a_attack);
	}
	#endregion
	
	#region Status Callbacks
	public void OnDeath ()
	{
		foreach(AUnitComponent each in _statusCallbacks)
		{
			if(each.enabled)
			{
				((IStatusCallbacks)each).OnDeath();
			}
		}	
	}
	#endregion
	
	#region Stats Modification Callbacks
	public void OnStatsModification ()
	{
		foreach(AUnitComponent each in _statusCallbacks)
		{
			if(each.enabled)
			{
				((IStatsModificationCallbacks)each).OnStatsModification();
			}
		}	
	}
	#endregion
	/*
	internal delegate void SimpleCallback();
	
	internal delegate void UnitCallback(Unit a_unit);
	
	internal delegate void OrderCallback(AOrder a_order);
	internal delegate void OrderPositionCallback(AOrder a_order, Vector3 a_position);
	internal delegate void OrderUnitCallback(AOrder a_order, Unit a_target);
	
	internal delegate void Vector3Callback(Vector3 a_position);
	
	internal delegate void AttackConfCallback(AttackConf a_attackConf);
	internal delegate void AttackWrapperCallback(AttackWrapper a_attackWrapper);
	
	*/
}
