using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AInteractable : FullInspector.BaseBehavior, ISelectionCallback, IHoverCallback, IInteractionCallback
{
	#region Inspector Properties
	public string nameLocKey = "Selectable";
	#endregion

	#region Properties
	#endregion
	
	#region Interface Lists
	List<AInteractableComponent> _selectionCallbacks 	= new List<AInteractableComponent>();
	List<AInteractableComponent> _hoverCallbacks 		= new List<AInteractableComponent>();
	List<AInteractableComponent> _highlightCallbacks	= new List<AInteractableComponent>();
	List<AInteractableComponent> _interactionCallbacks	= new List<AInteractableComponent>();
	#endregion

	#region Starts Methods
	protected override void Awake()
	{
		base.Awake();
		PreparesReferences();
	}
	
	private void PreparesReferences()
	{
		/*foreach(AInteractableComponent each in GetComponents<AInteractableComponent>())
		{
			ParseComponent(each);
		}*/
		
		foreach(AInteractableComponent each in GetComponentsInChildren<AInteractableComponent>(true))
		{
			ParseComponent(each);
		}
	}
	
	protected virtual void ParseComponent(AInteractableComponent each)
	{		
		//INTERFACE PARSING
		if(each is ISelectionCallback)
			_selectionCallbacks.Add(each);
		if(each is IHoverCallback)
			_hoverCallbacks.Add(each);
		if(each is IHighlighCallback)
			_highlightCallbacks.Add(each);
		if(each is IInteractionCallback)
			_interactionCallbacks.Add(each);
		
		each.Init(this);
	}
	#endregion
	
	#region Selection Callbacks
	public void OnSelection ()
	{
		foreach(AInteractableComponent each in _selectionCallbacks)
		{
			if(each.enabled)
			{
				((ISelectionCallback)each).OnSelection();
			}
		}
	}

	public void OnDeselection ()
	{
		foreach(AInteractableComponent each in _selectionCallbacks)
		{
			if(each.enabled)
			{
				((ISelectionCallback)each).OnDeselection();
			}
		}
	}
	#endregion

	#region Hover Callbacks
	public void OnHoverStart ()
	{
		foreach(AInteractableComponent each in _hoverCallbacks)
		{
			if(each.enabled)
			{
				((IHoverCallback)each).OnHoverStart();
			}
		}
	}

	public void OnHoverStop ()
	{
		foreach(AInteractableComponent each in _hoverCallbacks)
		{
			if(each.enabled)
			{
				((IHoverCallback)each).OnHoverStop();
			}
		}
	}
	#endregion
	
	#region Highligh Callbacks
	public void OnHighlightStart ()
	{
		foreach(AInteractableComponent each in _highlightCallbacks)
		{
			if(each.enabled)
			{
				((IHighlighCallback)each).OnHighlightStart();
			}
		}
	}
	
	public void OnHighlightStop ()
	{
		foreach(AInteractableComponent each in _highlightCallbacks)
		{
			if(each.enabled)
			{
				((IHighlighCallback)each).OnHighlightStop();
			}
		}
	}
	#endregion

	#region Interaction Callbacks
	public void OnInteraction ()
	{
		foreach(AInteractableComponent each in _interactionCallbacks)
		{
			if(each.enabled)
			{
				((IInteractionCallback)each).OnInteraction();
			}
		}
	}
	#endregion
}