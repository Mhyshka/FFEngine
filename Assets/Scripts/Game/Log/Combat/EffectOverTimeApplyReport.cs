using UnityEngine;
using System.Collections;

public class EffectOverTimeApplyReport : EffectOverTimeReport
{
	internal override EReportLevel Level
	{
		get
		{
			return EReportLevel.Verbose;
		}
	}
	
	public override string ToString ()
	{
		string report = "";
		report = string.Format("{0} is affected by {1}'s {2}.", target.Name,
																attackInfos.source.Name,
																effect.conf.Name);
		return report;
	}
}
