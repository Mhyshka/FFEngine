using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : AInteractable
{
	#region Inspector Properties
	public UnitStats 			stats = null;
	public UnitAttack 			attack = null;
	public UnitDefense			defense = null;
	public UnitLife 			life = null;
	public new UnitAnimation    animation = null;
	public UnitMovement			movement = null;
	
	public EffectOverTimeConf effect = null;
	#endregion

	#region Properties
	#endregion
	
	#region Interface Lists
	#endregion

	#region Starts Methods
	protected override void Awake ()
	{
		base.Awake();
	}
	
	protected override void ParseComponent(AInteractableComponent each)
	{		
		base.ParseComponent(each);
		
		//AUnitComponent unitComp = each as AUnitComponent;
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
	/// <summary>
	/// Call this when you want the character to inflict basic attack damages.
	/// </summary>
	internal void ThrowNormalAttack()
	{	
		attack.ThrowAttack(attack.basicAttack);
	}
	
	/// <summary>
	/// Call this when you want to apply an attack damages to a target.
	/// </summary>
	internal void DeliverAttack(Unit a_target, AttackWrapper a_attack)
	{
		if(onAttackDelivered != null)
			onAttackDelivered(a_target, a_attack);
		a_target.ApplyAttack(a_attack);
	}
	
	/// <summary>
	/// Call this when this unit should received an attack damages.
	/// </summary>
	internal void ApplyAttack(AttackWrapper a_attack)
	{
		if(onAttackReceived != null)
			onAttackReceived(a_attack);
		defense.ApplyDamage(a_attack);
	}
	
	/// <summary>
	/// Called by the ApplyAttack() method after damages reduction from armor.
	/// </summary>
	internal void ApplyDamages(DamageReport a_report)
	{
		if(onDamageTaken!= null)
			onDamageTaken(a_report);
		Debug.Log(a_report.ToString());
		life.Damage(a_report.final);
	}
	#endregion
	
	#region Target Callbacks
	internal OrderUnitCallback ontargetted;
	internal OrderUnitCallback onTargetAcquired;
	internal OrderUnitCallback onTargetLost;
	internal OrderUnitCallback onTargetDeath;
	#endregion
	
	
	#region Order Callbacks
	internal OrderCallback onNewOrderReceived;
	internal OrderCallback onOrderCompletetd;
	#endregion

	#region Movement Callbacks
	internal Vector3Callback onStartsMoving;
	internal SimpleCallback onStopsMoving;
	internal Vector3Callback onDestinationUpdated;
	internal Vector3Callback onDestinationReached;
	#endregion

	#region Damage Callbacks
	internal DamageReportCallback onDamageTaken;
	internal UnitDamageCallback onDamageDealt;
	#endregion
	
	#region Attack Callbacks
	internal AttackWrapperCallback onAttackReceived;
	internal AttackCallback onAttackDelivered;
	internal AttackConfCallback onAttackThrown;
	#endregion
	
	#region Status Callbacks
	internal SimpleCallback onDeath;
	#endregion
	
	#region Stats Modification Callbacks
	internal SimpleCallback onStatsModification;
	#endregion
}
