using UnityEngine;
using System.Collections;

public class UnitMovement : AUnitComponent, IStatsModificationCallbacks
{
	#region Inspector Properties
	public NavMeshAgent walker = null;
	#endregion

	#region Properties
	protected FloatModified _speed = null;
	internal float CurrentSpeed
	{
		get
		{
			return _speed.Value;
		}
	}
	
	internal float BaseSpeed
	{
		get
		{
			return _speed.baseValue;
		}
		set
		{
			_speed.baseValue = value;
			UpdateSpeed();
		}
	}
	#endregion

	#region Methods
	internal override void Init (AInteractable a_unit)
	{
		base.Init (a_unit);
		walker.updateRotation = false;
		
		_speed = new FloatModified();
		BaseSpeed = walker.speed;
		_speed.isFlatFirst = GameConstants.MOVE_SPEED_IS_FLAT_FIRST;
		UpdateSpeed();
	}
	
	protected virtual void Update()
	{
		//Debug.Log(walker.nextPosition.ToString());
		if(walker.hasPath)
		{
			Vector3 direction = walker.steeringTarget - _unit.transform.position;
			direction.y = 0;
			_unit.transform.rotation = Quaternion.Lerp(_unit.transform.rotation,
	                                                    Quaternion.LookRotation(direction),
	                                                    10f * Time.deltaTime);
			_unit.transform.rotation = Quaternion.RotateTowards(_unit.transform.rotation,
																Quaternion.LookRotation(direction),
																100f * Time.deltaTime);
		}
	}
	#endregion
	

	
	#region Movement
	internal void SetDestination(Vector3 a_destination)
	{
		walker.SetDestination(a_destination);
		_unit.OnDestinationUpdated(a_destination);
	}
	
	internal void Stop()
	{
		walker.Stop();
	}
	#endregion
	
	#region ComputeSpeed
	protected float AgilitySpeed
	{
		get
		{
			return  GameConstants.AGILITY_MOVE_SPEED_PER_POINT * _unit.stats.agility.Value;
		}
	}
	
	protected float EffectFlatSpeed
	{
		get
		{
			return 0f;
		}
	}
	
	protected float EffectPercentSpeed
	{
		get
		{
			return 0f;
		}
	}
	
	public void OnStatsModification ()
	{
		UpdateSpeed();
	}
	
	protected void UpdateSpeed()
	{
		_speed.modifier.flat = AgilitySpeed + EffectFlatSpeed;
		_speed.modifier.percent = EffectPercentSpeed;
		walker.speed = CurrentSpeed;
	}
	#endregion
}
