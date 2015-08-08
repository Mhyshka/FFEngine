using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ECriticalType
{
	Normal,
	Penetration,
	Crititcal
}

public class AttackWrapper
{

	#region Inspector Properties
	#endregion

	#region Properties
	internal Attack conf = null;
	internal AttackInfos attackInfos = null;
	internal List<AEffect> effects = null;
	#endregion

	#region Methods
	internal AttackReport Apply(Unit a_target)
	{
		AttackReport report = new AttackReport();
		report.attack = conf;
		report.attackInfos = attackInfos;
		report.target = a_target;
		
		foreach(AEffect each in effects)
		{
			report.effects.Add(each.Apply(a_target));
		}
		return report;
	}
	#endregion
}