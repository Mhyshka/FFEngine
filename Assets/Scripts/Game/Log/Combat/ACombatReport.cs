using UnityEngine;
using System.Collections;

public enum EReportLevel
{
	Verbose,
	Normal,
	Reduced
}

public enum EReportAlignement
{
	Positive,
	Neutral,
	Negative
}

public abstract class ACombatReport
{
	internal abstract EReportLevel Level
	{
		get;
	}
	
	internal abstract EReportAlignement Alignement
	{
		get;
	}
}

