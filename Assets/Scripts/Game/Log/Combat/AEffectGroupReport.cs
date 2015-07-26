using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AEffectGroupReport : AEffectReport
{
	#region Properties
	internal List<AEffectReport> effects = new List<AEffectReport>();
	
	protected HashSet<EDamageType> _damageTypes;
	internal HashSet<EDamageType> DamageTypes
	{
		get
		{
			return _damageTypes;
		}
	}
	
	internal override bool IsKillingBlow
	{
		get
		{
			foreach(AEffectReport each in effects)
			{
				if(each.IsKillingBlow)
					return true;
			}
			return false;
		}
	}
	
	internal override int FinalDamages
	{
		get
		{
			int total = 0;
			foreach(AEffectReport each in effects)
			{
				total += each.FinalDamages;
			}
			return total;
		}
	}
	
	internal override int UnreducedDamages
	{
		get
		{
			int total = 0;
			foreach(AEffectReport each in effects)
			{
				total += each.UnreducedDamages;
			}
			return total;
		}
	}
	
	internal override int ReducedByArmor
	{
		get
		{
			int total = 0;
			foreach(AEffectReport each in effects)
			{
				total += each.ReducedByArmor;
			}
			return total;
		}
	}
	#endregion
}
