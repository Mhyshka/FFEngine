using UnityEngine;
using System.Collections;

public class AUnitComponent
{
	
	
	#region Inspector Properties
	#endregion

	#region Properties
	protected Unit _unit;
	#endregion

	#region Methods
	internal virtual void Init(Unit a_unit)
	{
		_unit = a_unit;
	}
	#endregion

}
