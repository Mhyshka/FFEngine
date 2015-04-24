using UnityEngine;
using System.Collections;

[System.Serializable]
public class UnitStats : AUnitComponent
{
	#region Inspector Properties
	public AttributeStats attributes = null;
	public OffensiveStats attack = null;
	public DefensiveStats defense = null;
	public HitPointStats hitpoints = null;
	#endregion

	#region Properties
	#endregion

	#region Methods
	internal override void Init (Unit a_unit)
	{
		base.Init (a_unit);
		attack.Init(a_unit);
		defense.Init(a_unit);
		hitpoints.Init(a_unit);
	}
	#endregion


}
