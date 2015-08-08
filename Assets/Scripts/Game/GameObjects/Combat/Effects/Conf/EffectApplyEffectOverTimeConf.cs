using UnityEngine;
using System.Collections;

[System.Serializable]
public class EffectApplyEffectOverTimeConf : EffectConf
{
	public EffectOverTimeConf effectToApply = null;
	
	internal override AEffect Compute (AttackInfos a_attackInfos)
	{
		EffectApplyEffectOverTime effect = new EffectApplyEffectOverTime();
		effect.attackInfos = a_attackInfos;
		effect.effectOverTime = effectToApply.Compute(a_attackInfos);
		return effect;
	}
}
