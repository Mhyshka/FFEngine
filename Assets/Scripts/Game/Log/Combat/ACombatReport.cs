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
	internal int indentLevel = 0;
	internal abstract EReportLevel Level
	{
		get;
	}
	
	internal abstract EReportAlignement Alignement
	{
		get;
	}
}