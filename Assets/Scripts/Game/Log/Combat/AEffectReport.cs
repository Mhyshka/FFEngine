using UnityEngine;
using System.Collections;

public abstract class AEffectReport : ACombatReport
{
	internal AttackInfos attackInfos = null;
	internal EffectOverTimeInfos effectInfos = null;
	internal Unit target = null;
	
	internal virtual bool IsKillingBlow
	{
		get
		{
			return false;
		}
	}
	
	internal virtual int FinalDamages
	{
		get
		{
			return 0;
		}
	}
	
	internal virtual int UnreducedDamages
	{
		get
		{
			return 0;
		}
	}
	
	internal virtual int ReducedByArmor
	{
		get
		{
			return 0;
		}
	}
}
