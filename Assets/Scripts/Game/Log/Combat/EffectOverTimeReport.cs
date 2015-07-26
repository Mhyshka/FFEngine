using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectOverTimeReport : AEffectGroupReport
{
	#region Properties
	internal EffectOverTimeConf effect = null;
	internal EEffectOverTimeTrigger trigger = EEffectOverTimeTrigger.Apply;
	internal bool successfullyApplied = true;
	
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
			return EReportLevel.Normal;
		}
	}
	#endregion
	
	public override string ToString ()
	{
		string report = "";
		for(int i = 0 ; i < indentLevel ; i++)
			report += "\t";
			
		if(successfullyApplied)
		{
			if(trigger == EEffectOverTimeTrigger.Apply)
			{
				report += string.Format("{0} is affected by {1}'s {2}.", target.Name,
																		attackInfos.source.Name,
																		effect.Name);
																		
				foreach(AEffectReport each in effects)
				{
					each.indentLevel = indentLevel + 1;
					report += "\n" + each.ToString();
				}
			}
			else if(trigger == EEffectOverTimeTrigger.Refresh)
			{
				report += string.Format("{0}'s {1} is refreshed on {2}.", attackInfos.source.Name,
				                       									effect.Name,
													                    target.Name);
				foreach(AEffectReport each in effects)
				{
					each.indentLevel = indentLevel + 1;
					report += "\n" + each.ToString();
				}
			}
			else if(trigger == EEffectOverTimeTrigger.Tick)
			{
				report += string.Format("{0}'s {1} ticks on {2}.", attackInfos.source.Name,
											                        effect.Name,
											                        target.Name);
				foreach(AEffectReport each in effects)
				{
					each.indentLevel = indentLevel + 1;
					report += "\n" + each.ToString();
				}
			}
			else if(trigger == EEffectOverTimeTrigger.Dispel)
			{
				report += string.Format("{0}'s {1} was dispelled from {2}.", attackInfos.source.Name,
													                        effect.Name,
													                        target.Name);
				foreach(AEffectReport each in effects)
				{
					each.indentLevel = indentLevel + 1;
					report += "\n" + each.ToString();
				}
			}
			else if(trigger == EEffectOverTimeTrigger.TimeOut || trigger == EEffectOverTimeTrigger.TargetDeath || trigger == EEffectOverTimeTrigger.TargetInvalid)
			{
				report += string.Format("{0}'s {1} faded from {2}.", attackInfos.source.Name,
											                        effect.Name,
											                        target.Name);
				foreach(AEffectReport each in effects)
				{
					each.indentLevel = indentLevel + 1;
					report += "\n" + each.ToString();
				}
			}
		}
		else
		{
			report += string.Format("{0}'s {1} couldn't be applied on {2}.", attackInfos.source.Name,
													                        effect.Name,
													                        target.Name);
		}
		return report;
	}
}
