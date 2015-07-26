using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectOverTimeReport : EffectOverTimeReport
{
	internal EEffectApplyResult applyResult = EEffectApplyResult.NotApplied;
	internal List<AEffectReport> appliedEffects = null;

	internal EffectOverTimeConf effect = null;
	
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
			switch(applyResult)
			{
				case EEffectApplyResult.Applied : 
				return EReportLevel.Reduced;
				
				case EEffectApplyResult.Refreshed :
				return EReportLevel.Reduced;
				
				case EEffectApplyResult.NotApplied :
				return EReportLevel.Verbose;
				
				case EEffectApplyResult.Resisted :
				return EReportLevel.Normal;
				
				case EEffectApplyResult.Immune :
				return EReportLevel.Normal;
			}
			
			return EReportLevel.Verbose;
		}
	}
	
	public override string ToString ()
	{
		string report = "";
		for(int i = 0 ; i < indentLevel ; i++)
			report += "\t";
		if(applyResult == EEffectApplyResult.Applied)
		{
			report += string.Format("{0} is affected by {1}'s {2}.", target.Name,
																	attackInfos.source.Name,
																	effect.Name);
			foreach(AEffectReport each in appliedEffects)
			{
				each.indentLevel = indentLevel + 1;
				report += "\n" + each.ToString();
			}
		}
		else if(applyResult == EEffectApplyResult.Refreshed)
		{
			report += string.Format("{0}'s {1} is refreshed on {2}.", attackInfos.source.Name,
			                       									effect.Name,
												                    target.Name);
			foreach(AEffectReport each in appliedEffects)
			{
				each.indentLevel = indentLevel + 1;
				report += "\n" + each.ToString();
			}
		}
		else if(applyResult == EEffectApplyResult.NotApplied)
		{
			report += string.Format("{0}'s {1} couldn't be applied on {2}.", attackInfos.source.Name,
													                        effect.Name,
													                        target.Name);
		}
		else if(applyResult == EEffectApplyResult.Resisted)
		{
			report += string.Format("{0} resisted to {1}'s {2}.", target.Name,
										                        attackInfos.source.Name,
										                        effect.Name);
		}
		else if(applyResult == EEffectApplyResult.Immune)
		{
			report += string.Format("{0} was immune to {1}'s {2}.", target.Name,
											                        attackInfos.source.Name,
											                        effect.Name);
		}
		
		return report;
	}
}
