using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public abstract class EffectConf
{	
	public bool canCrit = true;
	public bool revertOnDestroy = false;
	
	internal abstract Effect Compute(AttackInfos a_attackInfos);
}

public class AttackInfos
{
	internal Unit source;
	internal ECriticalType critType;
	internal Vector3 targetPosition;
	internal List<Unit> affectedTargets;
}