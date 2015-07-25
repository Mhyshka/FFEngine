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
	
	protected int _currentStackCount = 0;

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
	#endregion
	
	#region Effects Methods
	internal void Prepare(Unit a_target)
	{
		target = a_target;
	}
	
	internal virtual void Apply()
	{
		timeElapsed = 0f;
		timeElapsedSinceRefresh = 0f;
		_currentStackCount = 1;
		foreach(EffectWrapper each in effects)
		{
			if(each.trigger.Has(EffectOverTimeTrigger.Apply))
			{
				each.effect.Apply(target);
			}
		}
	}
	
	internal virtual void Refresh()
	{
		timeElapsedSinceRefresh = 0f;
		
		_currentStackCount = Mathf.Clamp(_currentStackCount + 1,
											0,
											conf.maxStack);
											
		foreach(EffectWrapper each in effects)
		{
			if(each.trigger.Has(EffectOverTimeTrigger.Refresh))
			{
				each.effect.Apply(target);
			}
		}
	}
	
	internal virtual void Tick()
	{
		foreach(EffectWrapper each in effects)
		{
			if(each.trigger.Has(EffectOverTimeTrigger.Tick))
			{
				each.effect.Apply(target);
			}
		}
	}
	
	
	internal virtual bool Update(float a_deltatime)
	{
		int tick = TimeToTickCount;
		
		timeElapsed += a_deltatime;
		timeElapsedSinceRefresh += a_deltatime;
		
		int tickNeeded = TimeToTickCount - tick;
		for(int i = 0 ; i < tickNeeded ; i ++)
		{
			Tick();
		}
		
		if(timeElapsedSinceRefresh > conf.duration)
		{
			return true;
		}
		return false;
	}
	
	internal virtual bool Timeout()
	{
		foreach(EffectWrapper each in effects)
		{
			if(each.trigger.Has(EffectOverTimeTrigger.TimeOut))
			{
				each.effect.Apply(target);
			}
		}
		
		return timeElapsedSinceRefresh < conf.duration;
	}
	
	internal virtual void Destroy()
	{
		foreach(EffectWrapper each in effects)
		{
			if(each.effect.IsRevertOnDestroy)
				each.effect.Revert(target);
		}
		
		attackInfos = null;
		target = null;
		conf = null;
	}
	#endregion
	
	[System.Serializable]
	public class EffectWrapper
	{
		internal Effect effect;
		internal EffectOverTimeTrigger trigger;
	}
}
