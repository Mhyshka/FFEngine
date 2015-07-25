using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitManager
{
	protected Dictionary<int, Unit> _units = new Dictionary<int, Unit>();
	
	#region Management
	internal void RegisterUnit(Unit a_unit)
	{
		_units.Add(a_unit.GetInstanceID(), a_unit);
	}	
	
	internal void UnregisterUnit(Unit a_unit)
	{
		_units.Remove(a_unit.GetInstanceID());
	}
	#endregion
	
	#region Access
	internal Unit GetUnit(int a_instanceId)
	{
		Unit unit = null;
		
		if(!_units.TryGetValue(a_instanceId, out unit))
		{
			Debug.LogWarning("Unit not found");
		}
		
		return unit;
	}
	#endregion
}
