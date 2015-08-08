using UnityEngine;
using System.Collections;

public class EffectDamageModificationConf : EffectConf
{
	#region Inspector Properties
	public IntModifierInspectorConf general = null;
	public IntModifierInspectorConf physical = null;
	public IntModifierInspectorConf magic = null;
	#endregion
	
	internal override AEffect Compute (AttackInfos a_attackInfos)
	{
		EffectDamageModification effect = new EffectDamageModification();
		
		effect.attackInfos = a_attackInfos;
		effect.general = general.Compute();
		effect.physical = physical.Compute();
		effect.magic = magic.Compute();
		
		return effect;
	}
}
