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
		
		modified.BaseValue = baseValue.Value;
		modified.reducStackMethod = reductionStackMethod;
		modified.bonusIsFlatFirst = bonusIsFlatFirst;
		modified.malusIsFlatFirst = malusIsFlatFirst;
		
		return modified;
	}
	#endregion
}