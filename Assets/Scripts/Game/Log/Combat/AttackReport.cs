using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackReport : AEffectGroupReport
{
	#region Properties
	internal AttackConf attack = null;
	
	internal override EReportAlignement Alignement
	{
		get
		{
			if(target == FFEngine.Game.Players.Main.hero)// Player takes Damages
			{
				return EReportAlignement.Negative;
			}
			else if(target == FFEngine.Game.Players.Main.hero)
			{
				return EReportAlignement.Positive;
			}
			else
			{
				return EReportAlignement.Neutral;
			}
		}
	}
	
	internal override EReportLevel Level
	{
		get
		{
			return EReportLevel.Reduced;
		}
	}
	#endregion
	
	#region Methods
	//TODO LOCALIZATION
	public override string ToString ()
	{
		string critical = "";
		if(attackInfos.critType == ECriticalType.Crititcal)
			critical = " (Critical)";
		else if(attackInfos.critType == ECriticalType.Penetration)
			critical = " (Penetrating)";
			
		string report = "";
		for(int i = 0 ; i < indentLevel ; i++)
			report += "\t";
		
		report += string.Format("{0}'s {1} hits {2} for {3} damage(s).{4}",attackInfos.source.Name,
												                                 attack.Name,
												                                 target.Name,
														                         FinalDamages.ToString(),
				                             									 critical); 
		foreach(AEffectReport each in effects)
		{
			each.indentLevel = indentLevel + 1;
			report += "\n" + each.ToString();
		}

		return report;
	}
	
	
	#endregion
}
