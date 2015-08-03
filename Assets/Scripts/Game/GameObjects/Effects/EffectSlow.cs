using UnityEngine;
using System.Collections;

public class EffectSlow : AEffect
{
	#region Inspector Properties
	internal EffectSlowConf conf = null;
	internal IntModifier moveSpeedModifier = null;
	internal FloatModifier attackSpeedModifier = null;
	internal FloatModifier castSpeedModifier = null;
	#endregion
	
	#region Properties
	internal override int MetaStrength
	{
		get
		{
			return conf.metalLevel;
		}
	}
	#endregion
	
	#region Methods
	#endregion
	
	#region Effect
	internal override AEffectReport Apply(Unit a_target)
	{
		SlowReport report = new SlowReport();
		
		report.attackInfos = attackInfos;
		report.effectInfos = effectInfos;
		
		return report;
	}
	
	/*internal int ComputeStackModifications()
	{
		if(effectInfos != null)
		{
			if(effectInfos.doesStack && effectInfos.effectOverTime.CurrentStackCount > 1)
			{
				return effectInfos.perStackModifier.ComputeAdditive(amount, 
				                                                    effectInfos.effectOverTime.CurrentStackCount);
			}
		}
		
		return amount;
	}*/
	
	internal override AEffectReport Revert (Unit a_target)
	{
		SlowReport report = new SlowReport();
		
		return report;
	}
	#endregion
}

public class EffectSlowConf : EffectConf
{
	public int metalLevel = 10;
	public IntModifierConf moveSpeedReduction = null;
	public FloatModifierConf attackSpeedReduction = null;
	public FloatModifierConf castSpeedReduction = null;
	
	internal override AEffect Compute (AttackInfos a_attackInfos)
	{
		EffectSlow effect = new EffectSlow();
		
		return effect;
	}
}