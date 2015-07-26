using UnityEngine;
using System.Collections;

public abstract class Effect
{
	internal AttackInfos attackInfos = null;
	internal EffectOverTimeInfos effectInfos = null;
	
	internal abstract AEffectReport Apply(Unit a_target);
	
	internal abstract AEffectReport Revert(Unit a_target);
	
	internal abstract int MetaStrength
	{
		get;
	}
}