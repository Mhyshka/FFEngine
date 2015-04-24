using UnityEngine;
using System.Collections;

public class Unit : FullInspector.BaseBehavior
{
	#region Inspector Properties
	public string unitName = "A unit";
	public UnitStats stats = null;
	public UnitVisual visual = null;
	#endregion

	#region Properties
	#endregion

	#region Methods
	protected override void Awake ()
	{
		base.Awake();
		stats.Init (this);
		visual.Init(this);
	}

	protected virtual void Update ()
	{
  		if(Input.GetKeyDown(KeyCode.Space))
		{
			visual.PlayAttackAnimation();
		}
	}
	#endregion


	#region Callbacks
	internal void OnAttackComplete(AttackConf a_attack)
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
	}
	
	internal void Damage(AttackWrapper a_attack)
	{
		Debug.Log("Damage received!");
		
		stats.hitpoints.Damage(stats.defense.ApplyDamage(a_attack).final);
	}
	#endregion
}
