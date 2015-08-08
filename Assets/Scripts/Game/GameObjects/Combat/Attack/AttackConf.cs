using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FullInspector;

public abstract class AttackConf
{
	#region Inspector Properties
	#endregion
	
	#region Properties
	internal abstract string Name
	{
		get;
	}
	
	internal abstract List<EffectConf> OnHitEffects
	{
		get;
	}
	#endregion
	
	#region Methods
	internal abstract Attack Compute();
	#endregion
}

public class AttackConfReference : AttackConf
{
	#region Inspector Properties
	public AttackScriptable reference = null;
	#endregion
	
	#region Properties
	internal override string Name
	{
		get
		{
			return reference.conf.Name;
		}
	}
	
	internal override List<EffectConf> OnHitEffects
	{
		get
		{
			return reference.conf.OnHitEffects;
		}
	}
	#endregion
	
	#region Methods
	internal override Attack Compute()
	{
		return reference.conf.Compute();
	}
	#endregion
}

[System.Serializable]
public class AttackHardcodeConf : AttackConf
{
	#region Inspector Properties
	public string name = "attack";
	public float range = 1f;
	public float effectRadius = 1f;
	public float cooldown = 0f;
	public float recharge = 1f;
	public List<EffectConf> onHitEffects = new List<EffectConf>();
	#endregion
	
	#region Properties
	internal override string Name
	{
		get
		{
			return name;
		}
	}
	
	internal override List<EffectConf> OnHitEffects
	{
		get
		{
			return onHitEffects;
		}
	}
	#endregion
	
	#region Methods
	internal override Attack Compute()
	{
		Attack att = new Attack();
		att.conf = this;
		
		att.range = new FloatModified();
		att.range.BaseValue = range;
		
		att.effectRadius = new FloatModified();
		att.effectRadius.BaseValue = effectRadius;
		
		att.cooldown = new FloatModified();
		att.cooldown.BaseValue = cooldown;
		
		att.recharge = new FloatModified();
		att.recharge.BaseValue = recharge;
		
		att.onHitEffects = new List<EffectConf>();
		foreach(EffectConf each in OnHitEffects)
		{
			att.onHitEffects.Add(each);
		}
		
		return att;
	}
	#endregion
}