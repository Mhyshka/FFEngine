using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FullInspector;

public enum EEffectAlignement
{
	Neutral,
	Negative,
	Positive
}

public enum EEffectDispellClass
{
	None,
	Physic,
	Bleed,
	Magic,
	Disease,
	Poison
}

public enum EImparementType
{
	None,
	Movement,
	AttackSpeed,
	Stun,
	Sleep
}

[CreateAssetMenu()]
public class EffectOverTimeConf : EffectConf
{
#region Duration
	/// <summary>
	/// The duration in second. If 0 or less, won't disappear as time passes by.
	/// </summary>
	[InspectorHeader("Duration")]
	public float duration = 10f;
	
	/// <summary>
	/// The time between two ticks. If 0 or less, when be called. Use the onUpdate effects for realtime effects. eg : slow that grow stronger over time.
	/// </summary>
	public float timeBetweenTicks = 0f;
#endregion
	
#region Application
	/// <summary>
	/// If > 1, the effect will be able to be applied multiple times by the same source.
	/// </summary>
	[InspectorHeader("Application")]
	public int maxStack = 1;
	
	/// <summary>
	/// If true, duration of the effect will be reset when refreshed.
	/// </summary>
	public bool resetDurationOnRefresh = true;
	
	/// <summary>
	/// If true, the target can't be affect by this effect from two sources simultaneously. The previous effect will be override.
	/// </summary>
	public bool unique = false;
#endregion

#region Classification
	[InspectorHeader("Classfication")]
	/// <summary>
	/// If the buff can be dispelled by other effects.
	/// </summary>
	public bool canBeDispelled = true;
	
	/// <summary>
	/// Used to group effects. Some effect could for example, dispell all magical negative effects.
	/// </summary>
	public EEffectDispellClass dispell = EEffectDispellClass.None;
	
	/// <summary>
	/// Used to group effects. Some effect could for example, dispell all magical negative effects.
	/// </summary>
	
	public EImparementType imparement = EImparementType.None;
	
	/// <summary>
	/// Used to group effects. Some effect could for example, dispell all magical negative effects.
	/// </summary>
	public EEffectAlignement alignement = EEffectAlignement.Neutral;
#endregion

#region Feedback
	[InspectorHeader("Feedback")]
	public GameObject fxPrefab = null;//Placeholder
	public IconTitleDescFeedbackConf ui = null;
#endregion

#region Effects
	
	[InspectorHeader("Effects")]
	/// <summary>
	/// The list of effect to trigger when this effect get applied on a target.
	/// </summary>
	public List<EffectConf> onApplied = null;
	
	/// <summary>
	/// The list of effect to trigger when this effect get refreshed on a target.
	/// </summary>
	public List<EffectConf> onRefresh = null;
	public List<EffectConf> onUpdate = null;
	public List<EffectConf> onTick = null;
	
	[InspectorIndent]
	public List<EffectConf> onTimeout = null;
	public List<EffectConf> onDispelled = null;
	public List<EffectConf> onTargetDeath = null;
	public List<EffectConf> onTargetInvalid = null;
	public List<EffectConf> onFade = null;
 #endregion

 #region Methods
	internal EffectOverTimeWrapper Compute(Unit a_source)
	{
		EffectOverTimeWrapper effect = new EffectOverTimeWrapper();
		effect.a_source = a_source;
		effect.baseEffect = this;
		
		
		
		return effect;
	}
 #endregion
}
