using UnityEngine;
using System.Collections;

public class EffectApplyEffectOverTime : Effect
{
	internal EffectOverTime effectOverTime = null;
	
	internal override int MetaStrength
	{
		get
		{
			return effectOverTime.MetaStrength;
		}
	}

	internal override AEffectReport Apply (Unit a_target)
	{
		EffectOverTimeReport report = a_target.effect.TryApplyEffect(effectOverTime);
		return report;
	}
	
	internal override AEffectReport Revert (Unit a_target)
	{
		return new DamageReport();
	}
}