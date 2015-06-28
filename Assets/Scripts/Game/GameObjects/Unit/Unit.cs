using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : AInteractable, ITargettingCallbacks, IOrderCallbacks, IMovementCallbacks, IDamagesCallbacks, IStatusCallbacks
{
	#region Inspector Properties
	//public UnitStats stats = null;
	#endregion

	#region Properties
	#endregion
	
	#region Interface Lists
	List<AUnitComponent> _targetCallbacks	 	= new List<AUnitComponent>();
	List<AUnitComponent> _orderCallbacks 		= new List<AUnitComponent>();
	List<AUnitComponent> _movementCallbacks		= new List<AUnitComponent>();
	List<AUnitComponent> _damageCallbacks		= new List<AUnitComponent>();
	List<AUnitComponent> _statusCallbacks		= new List<AUnitComponent>();
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
		}
	}
 	#endregion
 
	#region Update Loop
	protected virtual void Update ()
	{
  		if(Input.GetKeyDown(KeyCode.Space))
		{
			//GetComponent<Animation>().State = UnitAnimation.EState.Attack;
		}
	}
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
		foreach(AUnitComponent each in _orderCallbacks)
		{
			if(each.enabled)
			{
				((IOrderCallbacks)each).OnAttackOrderReceived(a_order);
			}
		}
	}

	public void OnMoveOrderReceived (AOrder a_order)
	{
		foreach(AUnitComponent each in _orderCallbacks)
		{
			if(each.enabled)
			{
				((IOrderCallbacks)each).OnMoveOrderReceived(a_order);
			}
		}
	}

	public void OnStopOrderReceived (StopOrder a_order)
	{
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
	
	public void OnAttackReceived(AttackWrapper a_attack)
	{
		foreach(AUnitComponent each in _damageCallbacks)
		{
			if(each.enabled)
			{
				((IDamagesCallbacks)each).OnAttackReceived(a_attack);
			}
		}	
		//stats.hitpoints.Damage(stats.defense.ApplyDamage(a_attack).final);
	}
	
	public void OnAttackDelivered(AttackWrapper a_attack)
	{
		foreach(AUnitComponent each in _damageCallbacks)
		{
			if(each.enabled)
			{
				((IDamagesCallbacks)each).OnAttackDelivered(a_attack);
			}
		}	
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

	#region Callbacks
	/*internal void OnAttackComplete(AttackConf a_attack)
	{
		Vector3 pos = transform.position + transform.forward * 0.5f;
		
		Collider[] targets = Physics.OverlapSphere(pos, 0.7f);
		
		
		foreach(Collider each in targets)
		{
			Unit victim = each.GetComponent<Unit>();
			if(victim != this)
			{
				if(victim != null)
				{
					victim.Damage(a_attack.Compute(this));
				}
			}
		}
	}*/
	#endregion
}
