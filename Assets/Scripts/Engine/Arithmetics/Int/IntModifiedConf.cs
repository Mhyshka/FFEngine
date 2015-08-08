using UnityEngine;
using System.Collections;

[System.Serializable]
public class IntModifiedConf
{
	#region Inspector Properties
	public IntValue baseValue = null;
	public EReducPerStack reductionStackMethod = EReducPerStack.ReduceWhatsLeft;
	public bool bonusIsFlatFirst = false;
	public bool malusIsFlatFirst = true;
	#endregion
	
	#region Methods
	internal IntModified Compute()
	{
		IntModified modified = new IntModified();
		
		modified.BaseValue = baseValue.Value;
		modified.reducStackMethod = reductionStackMethod;
		modified.bonusIsFlatFirst = bonusIsFlatFirst;
		modified.malusIsFlatFirst = malusIsFlatFirst;
		
		return modified;
	}
	#endregion
}