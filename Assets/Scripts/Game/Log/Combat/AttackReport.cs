using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackReport : AEffectReport
{
	#region Properties
	internal AttackConf attack = null;
	internal List<AEffectReport> effects = new List<AEffectReport>();
	
	protected HashSet<EDamageType> _damageTypes;
	internal HashSet<EDamageType> DamageTypes
	{
		get
		{
			return _damageTypes;
		}
	}
	
	protected bool _isKillingBlow;
	internal bool IsKillingBlow
	{
		get
		{
			return _isKillingBlow;
		}
	}

	protected int _totalDamages;
	internal int TotalDamages
	{
		get
		{
			return _totalDamages;
		}
	}
	
	protected int _totalReduction;
	internal int TotalReduction
	{
		get
		{
			return _totalReduction;
		}
	}
	
	protected int _totalUnreduced;
	internal int TotalUnreduced
	{
		get
		{
			return _totalUnreduced;
		}
	}
	#endregion
	
	#region Methods
	/// <summary>
	/// Called this before using the getters from this class. Will compute TotalDamages, IsKillingBlow & Damagetypes
	/// </summary>
	internal AttackReport Prepare()
	{
		_damageTypes = new HashSet<EDamageType>();
		_isKillingBlow = false;
		_totalDamages = 0;
		_totalReduction = 0;
		foreach(AEffectReport each in effects)
		{
			if(each is DamageReport)
			{
				DamageReport damage = each as DamageReport;
				_damageTypes.Add(damage.type);
				_isKillingBlow = _isKillingBlow || damage.isKillingBlow;
				_totalDamages += damage.final;
				_totalReduction += damage.reducedByArmor;
				_totalUnreduced += damage.unreduced;
			}
		}
		
		return this;
	}
	
	//TODO LOCALIZATION
	public override string ToString ()
	{
		string critical = "";
		if(attackInfos.critType == ECriticalType.Crititcal)
			critical = " (Critical)";
		else if(attackInfos.critType == ECriticalType.Penetration)
			critical = " (Penetrating)";
			
		string report = string.Format("{0}'s {1} hits {2} for {3} damage(s).{4}",attackInfos.source.Name,
												                                 attack.Name,
												                                 target.Name,
														                         TotalDamages.ToString(),
				                             									 critical); 
		foreach(AEffectReport each in effects)
		{
			report += "\n\t" + each.ToString();
		}

		return report;
	}
	
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
}
