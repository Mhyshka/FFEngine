using UnityEngine;
using System.Collections;

[System.Serializable]
public class FloatModifiedConf
{
	#region Inspector Properties
	public FloatValue baseValue = null;
	public EReducPerStack reductionStackMethod = EReducPerStack.ReduceWhatsLeft;
	public bool bonusIsFlatFirst = false;
	public bool malusIsFlatFirst = true;
	#endregion
	
	#region Properties
	#endregion
	
	#region Methods
	internal FloatModified Compute()
	{
		FloatModified modified = new FloatModified();
		
		modified.baseValue = baseValue.Value;
		modified.reducStackMethod = reductionStackMethod;
		modified.bonusIsFlatFirst = bonusIsFlatFirst;
		modified.malusIsFlatFirst = malusIsFlatFirst;
		
		return modified;
	}
	#endregion
}

[System.Serializable]
public class FloatModifiedCustomConf
{
	#region Inspector Properties
	public FloatValue baseValue = null;
	internal EReducPerStack reductionStackMethod = EReducPerStack.ReduceWhatsLeft;
	internal bool bonusIsFlatFirst = false;
	internal bool malusIsFlatFirst = true;
	#endregion
	
	#region Properties
	#endregion
	
	#region Methods
	internal FloatModified Compute()
	{
		FloatModified modified = new FloatModified();
		
		modified.baseValue = baseValue.Value;
		modified.reducStackMethod = reductionStackMethod;
		modified.bonusIsFlatFirst = bonusIsFlatFirst;
		modified.malusIsFlatFirst = malusIsFlatFirst;
		
		return modified;
	}
	#endregion
}