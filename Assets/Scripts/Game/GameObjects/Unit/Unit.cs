using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : AInteractable
{
	#region Inspector Properties
	public UnitAttributes 			stats = null;
	public UnitAttack 			attack = null;
	public UnitDefense			defense = null;
	public UnitLife 			life = null;
	public new UnitAnimation    animation = null;
	public UnitMovement			movement = null;
	public UnitTimeHandler		time = null;
	public UnitEffectHandler	effect = null;
	#endregion

	#region Properties
	internal UnitConf UnitConf
	{
		get
		{
			return conf as UnitConf;
		}
	}
	#endregion
	
	#region Interface Lists
	#endregion

	#region Starts Methods
	protected override void Awake ()
	{
		base.Awake();
		FFEngine.Game.Units.RegisterUnit(this);
	}
	
	protected virtual void OnDestroy()
	{
		FFEngine.Game.Units.UnregisterUnit(this);
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
			animation.animator.SetTrigger("Attack_Strike");
			animation.State = UnitAnimation.EState.Attack;

		}
	}
	#endregion
	
	#region Damages & Attack reception
	
	internal void ReceiveAttack(EffectDamage a_dmg)
	{
		/*if(onDamageReceived != null)
			onDamageReceived(a_dmg);*/
	}
	
	/// <summary>
	/// Called by the ApplyAttack() method after damages reduction from armor.
	/// </summary>
	protected void ApplyDamages(DamageReport a_report)
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
	#endregion
	
	#region Status Callbacks
	internal SimpleCallback onDeath;
	#endregion
	
	#region Stats Modification Callbacks
	internal SimpleCallback onStatsModification;
	#endregion
}
