using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IntModified
{
	#region Inspector Properties
	internal int baseValue = 0;
	internal EReducPerStack reducStackMethod = EReducPerStack.ReduceWhatsLeft;
	internal bool bonusIsFlatFirst = false;
	internal bool malusIsFlatFirst = true;
	#endregion
	
	#region Properties
	protected List<IntModifierFromEffect> _bonusList = new List<IntModifierFromEffect>();
	protected List<IntModifierFromEffect> _malusList = new List<IntModifierFromEffect>();
	
	protected IntModifier _bonus = new IntModifier();
	protected IntModifier _malus = new IntModifier();
	
	protected bool _needsToBeCompute = true;
	protected int value = 0;
	
	internal int Value
	{
		get
		{
			if(_needsToBeCompute)
			{
				ComputeValue();
			}
			return value;
		}
	}
	#endregion
	
	#region Computing
	internal void ComputeValue()
	{
		value = baseValue;
		
		SumUpBonuses ();
		value = _bonus.Compute(value, bonusIsFlatFirst);
		
		SumUpReductions ();
		value = _malus.Compute(value, malusIsFlatFirst);
		
		if(reducStackMethod == EReducPerStack.ReduceWhatsLeft)
			value = Mathf.Max(value, 0);
		
		_needsToBeCompute = false;
	}
	
	protected void SumUpBonuses ()
	{
		_bonus.flat = 0;
		_bonus.percent = 0f;
		foreach (IntModifierFromEffect each in _bonusList)
		{
			_bonus.flat += each.modifier.flat;
			_bonus.percent += each.modifier.percent;
		}
	}
	
	protected void SumUpReductions ()
	{
		_malus.flat = 0;
		_malus.percent = 0f;
		foreach (IntModifierFromEffect each in _malusList)
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
		List<IntModifierFromEffect> modifiersToRemove = new List<IntModifierFromEffect>();
		foreach(IntModifierFromEffect each in _bonusList)
		{
			if(each.source == a_effect)
				modifiersToRemove.Add(each);
		}
		
		List<IntModifierFromEffect> reductionsToRemove = new List<IntModifierFromEffect>();
		foreach(IntModifierFromEffect each in _malusList)
		{
			if(each.source == a_effect)
				reductionsToRemove.Add(each);
		}
		
		foreach(IntModifierFromEffect each in modifiersToRemove)
		{
			_bonusList.Remove(each);
			_needsToBeCompute = true;
		}
		
		foreach(IntModifierFromEffect each in reductionsToRemove)
		{
			_malusList.Remove(each);
			_needsToBeCompute = true;
		}
	}
	
	internal void AddModifierFromEffect(IntModifierFromEffect a_modifier)
	{
		if(a_modifier.modifier.IsBonus)
			_bonusList.Add(a_modifier);
		else
			_malusList.Add(a_modifier);
		_needsToBeCompute = true;
	}
	#endregion
}
