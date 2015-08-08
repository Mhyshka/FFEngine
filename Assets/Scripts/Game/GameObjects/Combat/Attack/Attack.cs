using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attack
{
	#region Inspector Properties
	#endregion

	#region Properties
	internal AttackConf conf = null;
	
	internal FloatModified range = null;
	internal FloatModified effectRadius = null;
	internal FloatModified cooldown = null;
	internal FloatModified recharge = null;
	
	internal List<EffectConf> onHitEffects = null;
	
	//TODO LOCALIZATION
	internal string Name
	{
		get
		{
			return conf.Name;
		}
	}
	
	internal virtual Vector3 TargetPosition(Unit a_source)
	{
		Vector3 pos = a_source.transform.position + a_source.transform.forward * range.Value;
		return pos;
	}
	
	internal virtual List<Unit> SeekTargets(Unit a_soucre, Vector3 a_attackPosition)
	{
		List<Unit> targets = new List<Unit>();
		
		Collider[] colliders = Physics.OverlapSphere(a_attackPosition, effectRadius.Value, 1 << LayerMask.NameToLayer("Unit"));
		
		foreach(Collider each in colliders)
		{
			Unit newTarget = each.GetComponent<UnitTarget>().Unit;
			if(newTarget != a_soucre)
			{
				if(newTarget != null)
				{
					targets.Add(newTarget);
				}
			}
		}
		
		return targets;
	}
	
	internal virtual AttackWrapper Compute(AttackInfos a_attackInfos)
	{
		AttackWrapper attack = new AttackWrapper();
		attack.conf = this;
		attack.attackInfos = a_attackInfos;
		attack.effects = new List<AEffect>();
		
		foreach(EffectConf each in onHitEffects)
		{
			AEffect computed = each.Compute(a_attackInfos);
			attack.effects.Add (computed);
		}
		
		return attack;
	}
	#endregion

	#region Methods
	#endregion
}
