﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloatModified
{
	#region To Set Properties
	protected float _baseValue = 0f;
	internal float BaseValue
	{
		get
		{
			return _baseValue;
		}
		set
		{
			_baseValue = value;
			_needsToBeCompute = true;
		}
	}
	internal bool canGoUnderZero = false;
	internal EReducPerStack reducStackMethod = EReducPerStack.ReduceWhatsLeft;
	internal bool bonusIsFlatFirst = false;
	internal bool malusIsFlatFirst = true;
	#endregion
	
	#region Properties
	protected List<FloatModifierFromEffect> _bonusList = new List<FloatModifierFromEffect>();
	protected List<FloatModifierFromEffect> _malusList = new List<FloatModifierFromEffect>();
	
	protected FloatModifier _bonus = new FloatModifier();
	protected FloatModifier _malus = new FloatModifier();
	
	protected bool _needsToBeCompute = true;
	protected float _value = 0;
	
	internal float Value
	{
		get
		{
			if(_needsToBeCompute)
			{
				ComputeValue();
			}
			return _value;
		}
	}
	#endregion
	
	#region Computing
	internal void ComputeValue()
	{
		_value = _baseValue;
		
		SumUpBonuses ();
		_value = _bonus.Compute(_value, bonusIsFlatFirst);
		
		SumUpReductions ();
		
		_value = _malus.Compute(_value, malusIsFlatFirst);
			
		if(reducStackMethod == EReducPerStack.ReduceWhatsLeft)
			_value = Mathf.Max(_value, 0f);
		
		_needsToBeCompute = false;
	}

	protected void SumUpBonuses ()
	{
		_bonus.flat = 0f;
		_bonus.percent = 0f;
		foreach (FloatModifierFromEffect each in _bonusList)
		{
			_bonus.flat += each.modifier.flat;
			_bonus.percent += each.modifier.percent;
		}
	}

	protected void SumUpReductions ()
	{
		_malus.flat = 0f;
		_malus.percent = 0f;
		foreach (FloatModifierFromEffect each in _malusList)
		{
			_malus.flat += each.modifier.flat;
			if (reducStackMethod == EReducPerStack.ReduceWhatsLeft)
			{
				_malus.percent += Mathf.Clamp01(1f + _malus.percent) * each.modifier.percent;
			}
			else if (reducStackMethod == EReducPerStack.WorstOfAll)
			{
				_malus.percent = _malus.percent > each.modifier.percent ? each.modifier.percent : _malus.percent;
			}
			else if (reducStackMethod == EReducPerStack.Additive)
			{
				_malus.percent += each.modifier.percent;
			}
		}
	}

	#endregion
	
	#region Effects Management
	internal void NotifyEffectDestroy(EffectOverTime a_effect)
	{
		List<FloatModifierFromEffect> modifiersToRemove = new List<FloatModifierFromEffect>();
		foreach(FloatModifierFromEffect each in _bonusList)
		{
			if(each.source == a_effect)
				modifiersToRemove.Add(each);
		}
		
		List<FloatModifierFromEffect> reductionsToRemove = new List<FloatModifierFromEffect>();
		foreach(FloatModifierFromEffect each in _malusList)
		{
			if(each.source == a_effect)
				reductionsToRemove.Add(each);
		}
		
		foreach(FloatModifierFromEffect each in modifiersToRemove)
		{
			_bonusList.Remove(each);
			_needsToBeCompute = true;
		}
		
		foreach(FloatModifierFromEffect each in reductionsToRemove)
		{
			_malusList.Remove(each);
			_needsToBeCompute = true;
		}
	}
	
	internal void AddModifierFromEffect(FloatModifierFromEffect a_modifier)
	{
		if(a_modifier.modifier.IsBonus)
		{
			_bonusList.Add(a_modifier);
		}
		else
		{
			_malusList.Add(a_modifier);	
		}
		_needsToBeCompute = true;
	}
	#endregion
}
