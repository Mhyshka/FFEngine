using UnityEngine;
using System.Collections;

public class SlowReport : AEffectReport
{
	internal EffectSlow slow = null;
		
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
			return EReportLevel.Verbose;
		}
	}
	
	public override string ToString ()
	{
		//TODO LOCALIZATION
		string report = "";
		for(int i = 0 ; i < indentLevel ; i++)
			report += "\t";
			
		string targetValue = "";
		/*if(slow.conf.attackSpeedModifier.flat > 0 )
		
		report += string.Format("{0} inflicts {1} - {2} = {3} {4} damage(s) to {5}. {6}", attackInfos.source.Name, 
																	                        unreduced.ToString(), 
																	                        reducedByArmor.ToString(), 
																	                        final.ToString(), 
																	                        type.ToString(), 
																	                        target.Name, 
																	                        scratch); */
		
		return report;
	}
}
