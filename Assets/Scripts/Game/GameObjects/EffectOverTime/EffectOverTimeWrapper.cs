using UnityEngine;
using System.Collections;
using System.Collections.Generic;

internal class EffectOverTimeWrapper
{
	internal Unit a_source;
	internal EffectOverTimeConf baseEffect;
	
	#region Duration
	//public EffectDurationConf duration = null;
	protected float _timeElapsed = 0f;
	protected float _timeElapsedSinceRefresh = 0f;
	#endregion
	
	#region Application
	//public EffectApplicationConf application = null;
	protected int _currentStackCount = 0;
	#endregion
	
	#region Classification
	//public EffectClassification classification = null;
	#endregion
	
	#region Feedback
	//public IconTitleDescFeedbackConf feedback = null;
	#endregion
	
	#region Effects
	#endregion
	
	#region Effects
	/// <summary>
	/// The list of effect to trigger when this effect get applied on a target.
	/// </summary>
	public List<Effect> onApplied = null;
	
	/// <summary>
	/// The list of effect to trigger when this effect get refreshed on a target.
	/// </summary>
	public List<Effect> onRefresh = null;
	
	public List<Effect> onUpdate = null;
	public List<Effect> onTick = null;
	
	public List<Effect> onTimeout = null;
	public List<Effect> onDispelled = null;
	public List<Effect> onTargetDeath = null;
	public List<Effect> onTargetInvalid = null;
	public List<Effect> onFade = null;
	#endregion
	
	internal EffectOverTimeWrapper()
	{
		_timeElapsed = 0f;
		_timeElapsedSinceRefresh = 0f;
		_currentStackCount = 1;
	}
}
