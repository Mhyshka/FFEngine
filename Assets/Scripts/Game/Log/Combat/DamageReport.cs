using UnityEngine;
using System.Collections;

public class DamageReport : AEffectReport
{
	internal int 	unreduced = 0,
					reducedByArmor = 0,
					final = 0;
	internal EDamageType type = EDamageType.True;
	internal bool didScratch = false;
	internal bool isKillingBlow = false;
	
	internal override EReportAlignement Alignement
	{
		get
		{
			return EReportAlignement.Neutral;
		}
	}
	
	internal override EReportLevel Level
	{
		get
		{
			return EReportLevel.Reduced;
		}
	}
	
	/*public static DamageReport operator + (DamageReport x, DamageReport y)
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
	}*/
	
	public override string ToString ()
	{
		//TODO LOCALIZATION
		string scratch = didScratch ? " (Scratch)" : "";
		string report = "";
		if(attackInfos != null && attackInfos.source != null)
		{
			report = string.Format("{0} inflicts {1} - {2} = {3} {4} damage(s) to {5}. {6}", attackInfos.source.Name, 
																								unreduced.ToString(), 
																								reducedByArmor.ToString(), 
																								final.ToString(), 
																								type.ToString(), 
																								target.Name, 
																								scratch); 
		}
		else
		{
			report = string.Format("{0} takes {1} - {2} = {3} {4} damage(s). {5}", target.Name, 
													                              unreduced.ToString(), 
													                              reducedByArmor.ToString(), 
													                              final.ToString(), 
													                              type.ToString(), 
													                              scratch); 
		}
		
		return report;
	}
}
