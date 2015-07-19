using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AInteractable : FullInspector.BaseBehavior
{
	#region Inspector Properties
	public string nameLocKey = "Selectable";
	#endregion

	#region Properties
	#endregion
	
	#region Interface Lists
	#endregion

	#region Starts Methods
	protected override void Awake()
	{
		base.Awake();
		PreparesReferences();
	}
	
	private void PreparesReferences()
	{
		foreach(AInteractableComponent each in GetComponentsInChildren<AInteractableComponent>(true))
		{
			ParseComponent(each);
		}
	}
	
	protected virtual void ParseComponent(AInteractableComponent each)
	{		
		each.Init(this);
		each.RegisterForEvents();
	}
	#endregion
	
	#region Selection Callbacks
	internal SimpleCallback onSelection;
	internal SimpleCallback onDeselection;
	#endregion

	#region Hover Callbacks
	internal SimpleCallback onHoverStart;
	internal SimpleCallback onHoverStop;
	#endregion
	
	#region Highligh Callbacks
	internal SimpleCallback onHighlightStart;
	internal SimpleCallback onHighlightStop;
	#endregion

	#region Interaction Callbacks
	internal SimpleCallback onInteraction;
	#endregion
}