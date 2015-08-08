using UnityEngine;
using System.Collections;

public class UnitMovement : AUnitComponent
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
			return _speed.BaseValue;
		}
		set
		{
			_speed.BaseValue = value;
			UpdateSpeed();
		}
	}
	#endregion

	#region Methods
	internal override void Init (AInteractable a_unit)
	{
		base.Init (a_unit);
		if(walker != null)
			walker.updateRotation = false;
		
		_speed = new FloatModified();
		
		if(walker != null)
			BaseSpeed = walker.speed;
			
		UpdateSpeed();
	}
	
	protected virtual void Update()
	{
		if(walker != null && walker.hasPath)
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
	
	#region Events
	internal override void RegisterForEvents ()
	{
		base.RegisterForEvents ();
		_unit.onStatsModification += OnStatsModification;
	}
	
	protected override void UnregisterForEvents ()
	{
		base.UnregisterForEvents ();
		_unit.onStatsModification -= OnStatsModification;
	}
	
	protected void OnStatsModification ()
	{
		UpdateSpeed();
	}
	#endregion
	
	#region Movement
	internal void SetDestination(Vector3 a_destination)
	{
		if(walker != null)
			walker.SetDestination(a_destination);
		if(_unit.onDestinationUpdated!= null)
			_unit.onDestinationUpdated(a_destination);
	}
	
	internal void Stop()
	{
		if(walker != null)
			walker.Stop();
	}
	#endregion
	
	#region ComputeSpeed
	protected float AgilitySpeed
	{
		get
		{
			return  FFEngine.Game.Constants.AGILITY_MOVE_SPEED_PER_POINT * _unit.stats.agility.Value;
		}
	}
	
	protected void UpdateSpeed()
	{
		/*_speed.modifier.flat = AgilitySpeed + EffectFlatSpeed;
		_speed.modifier.percent = EffectPercentSpeed;*/
		
		if(walker != null)
			walker.speed = CurrentSpeed;
	}
	#endregion
	
	#region Speed Modification
	#endregion
}
