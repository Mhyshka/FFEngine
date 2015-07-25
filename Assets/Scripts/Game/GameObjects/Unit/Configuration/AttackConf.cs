using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AttackConf
{
	#region Inspector Properties
	public string name = "attack";
	//TODO LOCALIZATION
	internal string Name
	{
		get
		{
			return name;
		}
	}
	public FloatModified range = null;
	public FloatModified areaOfEffect = null;
	public FloatModified cooldown = null;
	public FloatModified rechargeRate = null;
	public List<EffectConf> effects = null;
	#endregion

	#region Properties
	internal Vector3 TargetPosition(Unit a_source)
	{
		Vector3 pos = a_source.transform.position + a_source.transform.forward * range.Value;
		return pos;
	}
	
	internal List<Unit> SeekTargets(Unit a_soucre, Vector3 a_attackPosition)
	{
		List<Unit> targets = new List<Unit>();
		
		Collider[] colliders = Physics.OverlapSphere(a_attackPosition, areaOfEffect.Value, 1 << LayerMask.NameToLayer("Unit"));
		
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
	
	internal AttackWrapper Compute(AttackInfos a_attackInfos)
	{
		AttackWrapper attack = new AttackWrapper();
		attack.conf = this;
		attack.attackInfos = a_attackInfos;
		attack.effects = new List<Effect>();
		
		foreach(EffectConf each in effects)
		{
			Effect computed = each.Compute(a_attackInfos);
			attack.effects.Add (computed);
		}
		
		return attack;
	}
	#endregion

	#region Methods
	#endregion
}
