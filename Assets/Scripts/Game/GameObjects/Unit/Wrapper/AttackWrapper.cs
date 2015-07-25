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
	internal AttackConf conf = null;
	internal AttackInfos attackInfos = null;
	internal List<Effect> effects = null;
	#endregion

	#region Methods
	internal AttackReport Apply(Unit a_target)
	{
		//a_target.onAttackReceived(this);
		
		AttackReport report = new AttackReport();
		report.attack = conf;
		report.attackInfos = attackInfos;
		report.target = a_target;
		
		foreach(Effect each in effects)
		{
			report.effects.Add(each.Apply(a_target));
		}
		
		return report.Prepare();
	}
	#endregion
}