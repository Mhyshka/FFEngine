using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IntModifiedModifier
{
	#region Inspector Properties
	internal FloatModified percent = null;
	internal IntModified flat = null;
	#endregion
	
	#region Properties
	#endregion
	
	#region Computing
	internal IntModifier Value
	{
		get
		{
			IntModifier mod = new IntModifier();
			mod.percent = percent.Value;
			mod.flat = flat.Value;
			return mod;
		}
	}	
	#endregion
	
	#region Effects Management
	#endregion
}
