using UnityEngine;
using System.Collections;

public enum EDamageType
{
//Physical
	Slashing,
	Piercing,
	Crushing,
	
//Magic
	Magic,
	/*Fire,
	Frost,
	Poison,
	Ligthing,*/
	Spirit,
	
	True
}

public class EffectDamage : Effect
{
	#region Inspector Properties
	internal EDamageType type;
	internal int amount;
	internal Reduction arpen;
	//public bool isRanged = false;
	#endregion

	#region Properties
	#endregion

	#region Methods
	/*internal DamageWrapper(EDamageType a_type, 
					int a_amount,
	                Reduction a_arpen
	                //bool a_isRanged,
					)
	{
		type = a_type;
		amount = a_amount;
		arpen = a_arpen;
		//isRanged = a_isRanged;
	}*/
	
	internal override void Apply (Unit a_target)
	{
		
	}
	#endregion
}

public class DamageReport
{
	internal string attackName;
	internal int applied;
	internal int reducedByArmor;
	internal int final;
	internal EDamageType type;
	internal EAttackStrikeType strikeType;
	internal bool didScratch;
	internal bool isKillingBlow;
	
	public static DamageReport operator + (DamageReport x, DamageReport y)
	{
		DamageReport report = new DamageReport();
		report.attackName = x.attackName;
		report.applied = x.applied + y.applied;
		report.reducedByArmor = x.reducedByArmor + y.reducedByArmor;
		report.final = x.final + y.final;
		report.strikeType = (int)x.strikeType > (int)y.strikeType ? x.strikeType : y.strikeType;
		report.isKillingBlow = x.isKillingBlow || y.isKillingBlow;
		report.didScratch = x.didScratch && y.didScratch;
		return report;
	}
	
	public override string ToString ()
	{
		return attackName + " : " + applied.ToString() + " - " + reducedByArmor.ToString() + " = " + final.ToString();
	}
}