using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloatModifiedModifier
{
	#region Inspector Properties
	internal FloatModified percent = null;
	internal FloatModified flat = null;
	#endregion
	
	#region Properties
	#endregion
	
	#region Computing
	internal FloatModifier Value
	{
		get
		{
			FloatModifier mod = new FloatModifier();
			mod.percent = percent.Value;
			mod.flat = flat.Value;
			return mod;
		}
	}	
	#endregion
	
	#region Effects Management
	#endregion
}
