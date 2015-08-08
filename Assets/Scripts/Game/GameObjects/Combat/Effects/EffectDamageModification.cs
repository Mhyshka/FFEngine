using UnityEngine;
using System.Collections;

public class EffectDamageModification : AEffect
{
	#region Properties
	internal IntModifier general = null;
	internal IntModifier physical = null;
	internal IntModifier magic = null;
	#endregion

	internal override int MetaStrength
	{
		get
		{
			int strength = 0;
			//TODO META STRENGTH
			return strength;
		}
	}
	
	#region Compute
	internal override AEffectReport Apply (Unit a_target)
	{
		if(effectInfos == null)
		{
			Debug.LogError("Null effect for this modifier effect");
			return null;
		}
		
		FloatModifier percent;
		IntModifier flat;
		IntModifierFromEffect intMod;
		FloatModifierFromEffect floatMod;
		
		//PHYSICAL
		if(physical.flat > 0)
		{
			flat = new IntModifier();
			flat.flat = ComputeStackModification(physical.flat);
			
			intMod = new IntModifierFromEffect();
			intMod.modifier = flat;
			intMod.source = effectInfos.effectOverTime;
			
			a_target.attack.bonusPhysical.damage.flat.AddModifierFromEffect(intMod);
		}
		if(physical.percent > 0)
		{
			percent = new FloatModifier();
			percent.flat = ComputeStackModification(physical.percent);// Only flat % modification.
			
			floatMod = new FloatModifierFromEffect();
			floatMod.modifier = percent;
			floatMod.source = effectInfos.effectOverTime;
			
			a_target.attack.bonusPhysical.damage.percent.AddModifierFromEffect(floatMod);
		}
		
		
		//MAGIC
		if(magic.flat > 0)
		{
			flat = new IntModifier();
			flat.flat = ComputeStackModification(magic.flat);
			
			intMod = new IntModifierFromEffect();
			intMod.modifier = flat;
			intMod.source = effectInfos.effectOverTime;
			
			a_target.attack.bonusMagical.damage.flat.AddModifierFromEffect(intMod);
		}
		if(magic.percent > 0)
		{
			percent = new FloatModifier();
			percent.flat = ComputeStackModification(magic.percent);// Only flat % modification.
			
			floatMod = new FloatModifierFromEffect();
			floatMod.modifier = percent;
			floatMod.source = effectInfos.effectOverTime;
			
			a_target.attack.bonusMagical.damage.percent.AddModifierFromEffect(floatMod);
		}
		
		return null;
	}
	
	internal override AEffectReport Revert (Unit a_target)
	{
		//PHYSICAL
		if(physical.flat > 0)
		{
			a_target.attack.bonusPhysical.damage.flat.NotifyEffectDestroy(effectInfos.effectOverTime);
		}
		if(physical.percent > 0)
		{
			a_target.attack.bonusPhysical.damage.percent.NotifyEffectDestroy(effectInfos.effectOverTime);
		}
		
		
		//MAGIC
		if(magic.flat > 0)
		{
			a_target.attack.bonusMagical.damage.flat.NotifyEffectDestroy(effectInfos.effectOverTime);
		}
		if(magic.percent > 0)
		{
			a_target.attack.bonusMagical.damage.percent.NotifyEffectDestroy(effectInfos.effectOverTime);
		}
		
		return null;
	}
	
	internal float ComputeStackModification(float a_base)
	{
		float res = a_base;
		if(effectInfos != null)
		{
			if(effectInfos.doesStack && effectInfos.effectOverTime.CurrentStackCount > 1)
			{
				res = effectInfos.perStackModifier.ComputeAdditive(res, 
				                                                    effectInfos.effectOverTime.CurrentStackCount);
			}
		}
		return res;
	}
	
	
	internal int ComputeStackModification(int a_base)
	{
		int res = a_base;
		if(effectInfos != null)
		{
			if(effectInfos.doesStack && effectInfos.effectOverTime.CurrentStackCount > 1)
			{
				res = effectInfos.perStackModifier.ComputeAdditive(res, 
				                                                   effectInfos.effectOverTime.CurrentStackCount);
			}
		}
		return res;
	}
	#endregion	
}
