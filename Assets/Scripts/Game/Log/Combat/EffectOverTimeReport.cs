using UnityEngine;
using System.Collections;

public abstract class EffectOverTimeReport : AEffectReport
{
	internal EffectOverTime effect = null;
	
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
	/*
	public override string ToString ()
	{
		//TODO LOCALIZATION
		return string.Format ("[EffectOverTimeReport: ]");
	}*/
}
