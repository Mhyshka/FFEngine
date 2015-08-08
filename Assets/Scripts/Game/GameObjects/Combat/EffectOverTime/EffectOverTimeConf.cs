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

public enum EUnitEffectStackMethod
{
	//None, // Hard to implement
	OnePerUnit,
	One
}

public enum EEffectOverTimeTrigger
{
	Apply,
	Refresh,
	Tick,
	Dispel,
	TargetDeath,
	TargetInvalid,
	TimeOut,
	Destroy
}

[System.Serializable]
public class EffectOverTimeConf
{
	public string name = "effectOverTime";
	
	internal string Name
	{
		get
		{
			//TODO LOCALIZATION
			return name;
		}
	}
#region Duration
	[InspectorHeader("Duration")]
	/// <summary>
	/// The duration in second. If 0 or less, won't disappear as time passes by.
	/// </summary>
	public float duration = 10f;
	
	/// <summary>
	/// The time between two ticks. If 0 or less, when be called. Use the onUpdate effects for realtime effects. eg : slow that grow stronger over time.
	/// </summary>
	public float timeBetweenTicks = 0f;
#endregion
	
#region Application
	[InspectorHeader("Application")]
	/// <summary>
	/// If > 1, the effect will be able to be applied multiple times by the same source.
	/// </summary>
	public int maxStack = 1;
	
	/// <summary>
	/// The limit of this effect stacking.
	/// </summary>
	public EUnitEffectStackMethod stackMethod = EUnitEffectStackMethod.OnePerUnit;
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
	//public GameObject fxPrefab = null;//Placeholder
	public IconTitleDescFeedbackConf ui = null;
#endregion

#region Effects
	[InspectorHeader("Effects")]
	public List<EffectConfWrapper> effects = null;
 #endregion

 #region Methods
 	/// <summary>
 	/// Returns an EffectOverTime
  	/// </summary>
	internal virtual EffectOverTime Compute(AttackInfos a_attackInfos)
	{
		EffectOverTime effect = new EffectOverTime();
		effect.attackInfos = a_attackInfos;
		effect.conf = this;
		
		foreach(EffectConfWrapper each in effects)
		{
			effect.effects.Add(each.Compute(a_attackInfos));
		}
		
		return effect;
	}
 #endregion
 
	[System.Serializable]
	public class EffectConfWrapper
	{
		public EffectConf effect = null;
		
		/// <summary>
		/// Wether this effect should be nullify when the EffectOvertime fades. ( Stats modifications NEEDS this to be checked or the modification will last forever. )
		/// </summary>
		public bool isRevert = false;
		
		/// <summary>
		/// A mask that defines where the effect triggers.
		/// </summary>
		public EffectTriggerConf trigger = null;
		
		public bool doesStack = true;
		
		public IntModifierConf perStackModifier = null;
		
		internal EffectOverTime.EffectWrapper Compute(AttackInfos a_attackInfos)
		{
			EffectOverTime.EffectWrapper wrapper = new EffectOverTime.EffectWrapper();
			wrapper.trigger = trigger;
			wrapper.effect = effect.Compute(a_attackInfos);
			wrapper.doesStack = doesStack;
			wrapper.perStackModifier = perStackModifier.Compute();
			return wrapper;
		}
	}
}


[System.Serializable]
public class EffectTriggerConf
{
	public bool OnApply = false;
	public bool OnRefresh = false;
	public bool OnTick = false;
	public bool OnTimeOut = false;
	public bool OnDispel = false;
	public bool OnTargetDeath = false;
	public bool OnTargetInvalid = false;
}
