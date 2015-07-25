using UnityEngine;
using System.Collections;

public abstract class Effect
{
	internal bool isStackable;
	internal AttackInfos attackInfos;
	
	internal abstract AEffectReport Apply(Unit a_target);
	
	internal abstract AEffectReport Revert(Unit a_target);
	
	internal abstract bool IsRevertOnDestroy
	{
		get;
	}
	
	internal abstract int MetaStrength
	{
		get;
	}
}