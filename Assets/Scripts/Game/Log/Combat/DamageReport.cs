using UnityEngine;
using System.Collections;

public class DamageReport : AEffectReport
{
	#region Properties
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
	
	internal override bool IsKillingBlow
	{
		get
		{
			return isKillingBlow;
		}
	}
	
	internal override int FinalDamages
	{
		get
		{
			return final;
		}
	}
	
	internal override int UnreducedDamages
	{
		get
		{
			return unreduced;
		}
	}
	
	internal override int ReducedByArmor
	{
		get
		{
			return reducedByArmor;
		}
	}
	#endregion
	
	public override string ToString ()
	{
		//TODO LOCALIZATION
		string scratch = didScratch ? " (Scratch)" : "";
		string report = "";
		for(int i = 0 ; i < indentLevel ; i++)
			report += "\t";
			
		if(attackInfos != null && attackInfos.source != null)
		{
			report += string.Format("{0} inflicts {1} - {2} = {3} {4} damage(s) to {5}. {6}", attackInfos.source.Name, 
																								unreduced.ToString(), 
																								reducedByArmor.ToString(), 
																								final.ToString(), 
																								type.ToString(), 
																								target.Name, 
																								scratch); 
		}
		else
		{
			report += string.Format("{0} takes {1} - {2} = {3} {4} damage(s). {5}", target.Name, 
													                              unreduced.ToString(), 
													                              reducedByArmor.ToString(), 
													                              final.ToString(), 
													                              type.ToString(), 
													                              scratch); 
		}
		
		return report;
	}
}
