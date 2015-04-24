using UnityEngine;
using System.Collections;

[System.Serializable]
public class DefensiveStats : AUnitComponent
{
	#region Inspector Properties
	public ResistanceConf general = null;
	
	public ResistanceConf slashing = null;
	public ResistanceConf crushing = null;
	public ResistanceConf piercing = null;
	
	public ResistanceConf magic = null;
	#endregion

	#region Properties
	#endregion

	#region Resitance
	internal Reduction GetResistance(EDamageType type, Reduction a_arpen)
	{
		Resistance reduc;
		
		switch(type)
		{
		case EDamageType.Slashing :
			reduc = general + slashing;
			break;
			
		case EDamageType.Crushing :
			reduc = general + crushing;
			break;
			
		case EDamageType.Piercing :
			reduc = general + piercing;
			break;
			
		case EDamageType.Magic :
			reduc = general + magic;
			break;
			
		case EDamageType.True :
			reduc = new Resistance();
			break;
			
		default: 
			reduc = new Resistance();
			break;
		}
		
		Reduction result = new Reduction();
		result.percent = ComputePercentReduction(reduc.armor, a_arpen);
		result.flat = ComputeFlatReduction(reduc.flat, a_arpen);
		return result;
	}
	
	internal float ComputePercentReduction(int armor, Reduction a_arpen)
	{
		int effectiveArmor = a_arpen.Compute(armor);
		float reduction = 1f + (- 1f / Mathf.Exp(effectiveArmor/50f));
		return reduction;
	}
	
	internal float ComputeFlatReduction(int flat, Reduction a_arpen)
	{
		return flat;
	}
	#endregion
	
	internal DamageReport ApplyDamage(AttackWrapper a_attack)
	{
		DamageReport report = new DamageReport();
		
		foreach(DamageWrapper each in a_attack.damages)
		{
			report += ComputeDamage(each);
		}
		
		Debug.Log("Damages : " + report.ToString());
		return report;
	}
	
	internal DamageReport ComputeDamage(DamageWrapper a_dmg)
	{
		DamageReport report = new DamageReport();
		
		//Armor reduction
		report.applied = a_dmg.amount;
		report.final = GetResistance(a_dmg.type,a_dmg.arpen).Compute(report.applied);
		report.reducedByArmor = report.final - report.applied;
		
		return report;
	}
}