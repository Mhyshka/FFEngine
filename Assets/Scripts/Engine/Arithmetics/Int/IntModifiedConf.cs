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
		
		modified.baseValue = baseValue.Value;
		modified.reducStackMethod = reductionStackMethod;
		modified.bonusIsFlatFirst = bonusIsFlatFirst;
		modified.malusIsFlatFirst = malusIsFlatFirst;
		
		return modified;
	}
	#endregion
}

[System.Serializable]
public class IntModifiedCustomConf
{
	#region Inspector Properties
	public IntValue baseValue = null;
	internal EReducPerStack reductionStackMethod = EReducPerStack.ReduceWhatsLeft;
	internal bool bonusIsFlatFirst = false;
	internal bool malusIsFlatFirst = true;
	#endregion
	
	#region Methods
	internal IntModified Compute()
	{
		IntModified modified = new IntModified();
		
		modified.baseValue = baseValue.Value;
		modified.reducStackMethod = reductionStackMethod;
		modified.bonusIsFlatFirst = bonusIsFlatFirst;
		modified.malusIsFlatFirst = malusIsFlatFirst;
		
		return modified;
	}
	#endregion
}