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
	
	internal override bool IsRevertOnDestroy
	{
		get
		{
			return false;
		}
	}

	internal override AEffectReport Apply (Unit a_target)
	{
		EffectOverTimeApplyReport report = new EffectOverTimeApplyReport();
		report.attackInfos = attackInfos;
		report.target = a_target;
		report.effect = effectOverTime;
		return report;
	}
	
	internal override AEffectReport Revert (Unit a_target)
	{
		return new DamageReport();
	}
}