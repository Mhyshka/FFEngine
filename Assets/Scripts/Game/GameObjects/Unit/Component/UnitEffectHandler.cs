using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UnitEffectHandler : AUnitComponent
{
	protected List<EffectOverTime> _effects;
	
	#region Init
	internal override void Init (AInteractable a_unit)
	{
		base.Init (a_unit);
		_effects = new List<EffectOverTime>();
	}
	#endregion
	
	#region Applying
	internal EffectApplyResult TryApplyEffect(EffectOverTime a_effect)
	{
		if(a_effect.conf.stackMethod == EUnitEffectStackMethod.OnePerUnit)
		{
			EffectOverTime previousEffect = GetEffectOfType(a_effect);
			if(previousEffect != null)
			{
				ReplaceEffect(previousEffect, a_effect);
				a_effect.Refresh();
				return EffectApplyResult.Refreshed;
			}
			else
			{
				AddNewEffect(a_effect);
				a_effect.Apply();
				return EffectApplyResult.Applied;
			}
		}
		
		return EffectApplyResult.NotApplied;
	}
	
	/// <summary>
	/// Will apply a new effect to this unit. The unit will manage it from here.
	/// </summary>
	protected void AddNewEffect(EffectOverTime a_effect)
	{
		_effects.Add(a_effect);
		a_effect.Prepare(_unit);
	}
	#endregion
	
	#region Management
	protected virtual void Update()
	{
		foreach(EffectOverTime each in _effects)
		{
			if(each.Update(_unit.time.EffectTimeScale * Time.deltaTime))
			{
				EffectTimeOut(each);
			}
		}
	}
	#endregion
	
	#region Assertion
	/// <summary>
	/// Returns null if no effect with the given conf is found in the active effects.
	/// Returns the matching effect otherwise.
	/// </summary>
	internal EffectOverTime GetEffectOfType(EffectOverTimeConf a_effectConf)
	{
		foreach(EffectOverTime each in _effects)
		{
			if(each.conf == a_effectConf)
			{
				return each;
			}
		}
		
		return null;
	}
	
	/// <summary>
	/// Returns null if no effect with the same conf & source is found in the active effects.
	/// Returns the matching effect otherwise.
	/// </summary>
	internal EffectOverTime GetEffectOfType(EffectOverTime a_effect)
	{
		foreach(EffectOverTime each in _effects)
		{
			if(each.conf == a_effect.conf &&
			   each.attackInfos.source == a_effect.attackInfos.source)
			{
				return each;
			}
		}
		
		return null;
	}
	#endregion
	
	#region Removing
	internal void RemoveEffect(EffectOverTime a_effect)
	{
		_effects.Remove(a_effect);
		a_effect.Destroy();
	}
	
	internal void ReplaceEffect(EffectOverTime a_previous, EffectOverTime a_new)
	{
		a_new.Prepare (_unit);
		a_new.timeElapsed = a_previous.timeElapsed;
		RemoveEffect(a_previous);
	}
	
	internal void EffectTimeOut(EffectOverTime a_effect)
	{
		a_effect.Timeout();
		_effects.Remove(a_effect);
		a_effect.Destroy();
	}
	#endregion
}

public enum EffectApplyResult
{
	NotApplied,
	Applied,
	Refreshed
}


/*
	if(a_effect.conf.stackLimit == EEffectStackLimit.None)
		{
			ApplyNewEffect(a_effect);
		}
		else if(a_effect.conf.stackLimit == EEffectStackLimit.One)
		{
			EffectOverTime effect = GetEffectOfType(a_effect.conf);
			if(effect == null)
			{
				ApplyNewEffect(a_effect);
			}
			else
			{
				if(a_effect.MetaStrength > effect)
				{
					effect.Destroy();
					RemoveEffect(effect);
					ApplyNewEffect(effect);
				}
				else
				{
					effect.Refresh();
				}
			}
		}
		else if(a_effect.conf.stackLimit == EEffectStackLimit.OnePerUnit)
		{
		
		}
*/
