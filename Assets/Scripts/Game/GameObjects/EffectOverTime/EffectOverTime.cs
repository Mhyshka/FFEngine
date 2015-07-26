using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectOverTime
{
	internal AttackInfos attackInfos;
	internal Unit target;
	internal EffectOverTimeConf conf;
	
	#region Properties
	internal float timeElapsed = 0f;
	internal float timeElapsedSinceRefresh = 0f;
	
	protected bool _isPrepared = false;
	internal bool IsPrepared
	{
		get
		{
			return _isPrepared;
		}
	}
	
	protected int _currentStackCount = 0;
	internal int CurrentStackCount
	{
		get
		{
			return _currentStackCount;
		}
	}
	
	protected int _tickCount;
	internal int TickCount
	{
		get
		{
			 return _tickCount;
		}
	}
	
	protected int _tickNeeded = 0;

	public List<EffectWrapper> effects = null;
	
	/// <summary>
	/// Returns the time remaining before this effect will Timeout.
	/// </summary>
	internal float TimeRemaining
	{
		get
		{
			float remaining = conf.duration - timeElapsedSinceRefresh;
			return remaining >= 0 ? remaining : 0f;
		}
	}
	
	/// <summary>
	/// Returns the total count of ticks that SHOULD have been handle for the current TimeElapsedSinceRefresh.
	/// </summary>
	internal int TimeToTickCount
	{
		get
		{
			int index = -1;
			
			if(conf.timeBetweenTicks > 0f)
				index = Mathf.FloorToInt(timeElapsedSinceRefresh / conf.timeBetweenTicks);
			
			return index;
		}
	}
	#endregion
	
	#region Standard methods
	internal EffectOverTime()
	{
		timeElapsed = 0f;
		timeElapsedSinceRefresh = 0f;
		_currentStackCount = 1;
		_tickNeeded = 0;
		effects = new List<EffectWrapper>();
	}
	
	internal virtual int MetaStrength
	{
		get
		{
			int strength = 0;
			foreach(EffectWrapper each in effects)
			{
				strength += each.effect.MetaStrength;
			}
			return strength;
		}
	}
	
	internal int TickNeeded
	{
		get
		{
			return _tickNeeded;
		}
	}
	
	internal bool ShouldTimeout
	{
		get
		{
			return timeElapsedSinceRefresh > conf.duration;
		}
	}
	
	internal void Prepare(Unit a_target)
	{
		Prepare (a_target, CurrentStackCount);
	}
	internal void Prepare(Unit a_target, int a_stackCount)
	{
		if(!IsPrepared)
		{
			_isPrepared = true;
			target = a_target;
			_currentStackCount = Mathf.Clamp(a_stackCount,
			                                 0,
			                                 conf.maxStack);
		}
	}
	
	internal virtual void Update(float a_deltatime)
	{
		int tick = TimeToTickCount;
		
		timeElapsed += a_deltatime;
		timeElapsedSinceRefresh += a_deltatime;
		
		_tickNeeded = TimeToTickCount - tick;
	}
	#endregion
	
	#region Effects Methods
	internal virtual EffectOverTimeReport Apply()
	{
		_tickCount = 0;
		timeElapsed = 0f;
		timeElapsedSinceRefresh = 0f;
		_currentStackCount = 1;
		_tickNeeded = 0;
		
		EffectOverTimeInfos effectInfos = new EffectOverTimeInfos();
		effectInfos.effectOverTime = this;
		effectInfos.trigger = EEffectOverTimeTrigger.Apply;
		
		List<AEffectReport> effectsReport = new List<AEffectReport>();
		foreach(EffectWrapper each in effects)
		{
			if(each.trigger.OnApply)
			{
				effectInfos.doesStack = each.doesStack;
				effectInfos.perStackModifier = each.perStackModifier;
				effectInfos.isRevertOnDestroy = each.isRevertOnDestroy;
				each.effect.effectInfos = effectInfos;
				effectsReport.Add(each.effect.Apply(target));
			}
		}
		
		EffectOverTimeReport report = new EffectOverTimeReport();
		report.effectInfos = effectInfos;
		report.target = target;
		report.attackInfos = attackInfos;
		report.effects = effectsReport;
		report.effect = conf;
		report.trigger = EEffectOverTimeTrigger.Apply;
		return report;
	}
	
	internal virtual EffectOverTimeReport Refresh()
	{
		timeElapsedSinceRefresh = 0f;
		_tickNeeded = 0;
		_currentStackCount = Mathf.Clamp(_currentStackCount + 1,
											0,
											conf.maxStack);
											
		EffectOverTimeInfos effectInfos = new EffectOverTimeInfos();
		effectInfos.effectOverTime = this;
		effectInfos.trigger = EEffectOverTimeTrigger.Refresh;
							
		List<AEffectReport> effectsReport = new List<AEffectReport>();				
		foreach(EffectWrapper each in effects)
		{
			if(each.trigger.OnRefresh)
			{
				effectInfos.doesStack = each.doesStack;
				effectInfos.perStackModifier = each.perStackModifier;
				effectInfos.isRevertOnDestroy = each.isRevertOnDestroy;
				each.effect.effectInfos = effectInfos;
				effectsReport.Add(each.effect.Apply(target));
			}
		}
		
		EffectOverTimeReport report = new EffectOverTimeReport();
		report.effectInfos = effectInfos;
		report.target = target;
		report.attackInfos = attackInfos;
		report.effects = effectsReport;
		report.effect = conf;
		report.trigger = EEffectOverTimeTrigger.Refresh;
		return report;
	}
	
	internal virtual EffectOverTimeReport Tick()
	{
		_tickNeeded--;
		_tickCount++;
		
		EffectOverTimeInfos effectInfos = new EffectOverTimeInfos();
		effectInfos.effectOverTime = this;
		effectInfos.trigger = EEffectOverTimeTrigger.Tick;
		
		List<AEffectReport> effectsReport = new List<AEffectReport>();			
		foreach(EffectWrapper each in effects)
		{
			if(each.trigger.OnTick)
			{
				effectInfos.doesStack = each.doesStack;
				effectInfos.perStackModifier = each.perStackModifier;
				effectInfos.isRevertOnDestroy = each.isRevertOnDestroy;
				each.effect.effectInfos = effectInfos;
				effectsReport.Add(each.effect.Apply(target));
			}
		}
		EffectOverTimeReport report = new EffectOverTimeReport();
		report.effectInfos = effectInfos;
		report.target = target;
		report.attackInfos = attackInfos;
		report.effects = effectsReport;
		report.effect = conf;
		report.trigger = EEffectOverTimeTrigger.Tick;
		return report;
	}

	
	internal virtual EffectOverTimeReport Timeout()
	{
		EffectOverTimeInfos effectInfos = new EffectOverTimeInfos();
		effectInfos.effectOverTime = this;
		effectInfos.trigger = EEffectOverTimeTrigger.TimeOut;
		
		List<AEffectReport> effectsReport = new List<AEffectReport>();			
		foreach(EffectWrapper each in effects)
		{
			if(each.trigger.OnTimeOut)
			{
				effectInfos.doesStack = each.doesStack;
				effectInfos.perStackModifier = each.perStackModifier;
				effectInfos.isRevertOnDestroy = each.isRevertOnDestroy;
				each.effect.effectInfos = effectInfos;
				effectsReport.Add(each.effect.Apply(target));
			}
		}
		
		EffectOverTimeReport report = new EffectOverTimeReport();
		report.effectInfos = effectInfos;
		report.target = target;
		report.attackInfos = attackInfos;
		report.effects = effectsReport;
		report.effect = conf;
		report.trigger = EEffectOverTimeTrigger.TimeOut;
		return report;
	}
	
	internal virtual EffectOverTimeReport Destroy()
	{
		EffectOverTimeInfos effectInfos = new EffectOverTimeInfos();
		effectInfos.effectOverTime = this;
		effectInfos.trigger = EEffectOverTimeTrigger.Destroy;

		List<AEffectReport> effectsReport = new List<AEffectReport>();			
		foreach(EffectWrapper each in effects)
		{
			if(each.isRevertOnDestroy)
			{
				effectInfos.doesStack = each.doesStack;
				effectInfos.perStackModifier = each.perStackModifier;
				effectInfos.isRevertOnDestroy = each.isRevertOnDestroy;
				each.effect.effectInfos = effectInfos;
				effectsReport.Add(each.effect.Revert(target));
			}
		}
		
		EffectOverTimeReport report = new EffectOverTimeReport();
		report.effectInfos = effectInfos;
		report.target = target;
		report.attackInfos = attackInfos;
		report.effects = effectsReport;
		report.effect = conf;
		report.trigger = EEffectOverTimeTrigger.Destroy;
		
		attackInfos = null;
		target = null;
		conf = null;
		
		return report;
	}
	#endregion
	
	[System.Serializable]
	public class EffectWrapper
	{
		//EffectInfos start
		internal bool isRevertOnDestroy = true;
		internal bool doesStack = true;
		internal IntModifier perStackModifier = null;
		//EffectInfos end
		
		internal Effect effect;
		internal EffectTriggerConf trigger;
	}
}

internal class EffectOverTimeInfos
{
	//EffectInfos start
	internal bool isRevertOnDestroy = true;
	internal bool doesStack = true;
	internal IntModifier perStackModifier = null;
	//EffectInfos end
	
	internal EEffectOverTimeTrigger trigger = EEffectOverTimeTrigger.Apply;
	internal EffectOverTime effectOverTime = null;
}
