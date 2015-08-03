using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AttackConf
{
	#region Inspector Properties
	public string name = "attack";
	public FloatModifiedConf range = null;
	public FloatModifiedConf areaOfEffect = null;
	public FloatModifiedConf cooldown = null;
	public FloatModifiedConf rechargeRate = null;
	public List<EffectConf> effects = null;
	#endregion

	#region Properties
	//TODO LOCALIZATION
	internal string Name
	{
		get
		{
			return name;
		}
	}
	
	internal Vector3 TargetPosition(Unit a_source)
	{
		Vector3 pos = a_source.transform.position + a_source.transform.forward * range.Compute().Value;
		return pos;
	}
	
	internal List<Unit> SeekTargets(Unit a_soucre, Vector3 a_attackPosition)
	{
		List<Unit> targets = new List<Unit>();
		
		Collider[] colliders = Physics.OverlapSphere(a_attackPosition, areaOfEffect.Compute().Value, 1 << LayerMask.NameToLayer("Unit"));
		
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
		attack.effects = new List<AEffect>();
		
		foreach(EffectConf each in effects)
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
