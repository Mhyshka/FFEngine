using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPlayerFeedbackOutline : AInteractableComponent
{
	public enum EState
	{
		Highlighted = 1 << 1,
		Hovered = 1 << 2,
		Selected = 1 << 3
	}
	
	#region Inspector Properties
	public PlayerColorFeedbackSettings colorSettings = null;
	#endregion
	
	#region Properties
	protected EState _state;
	
	protected List<InteractableRenderer> _renderers;
	#endregion
	
	#region Methods
	internal override void Init (AInteractable a_interactable)
	{
		base.Init (a_interactable);
		_renderers = new List<InteractableRenderer>();
		_state = 0;
		UpdateRenderersList();
	}
	
	protected void UpdateRenderersList()
	{
		_renderers.Clear();
		foreach(InteractableRenderer each in _interactable.GetComponentsInChildren<InteractableRenderer>(true))
		{
			_renderers.Add(each);
		}
	}
	
	protected void UpdateRenderersStatus()
	{
		UpdateOutline();
	}
	#endregion
	
	#region Events
	internal override void RegisterForEvents ()
	{
		base.RegisterForEvents ();
		_interactable.onHighlightStart += OnHighlightStart;
		_interactable.onHighlightStop += OnHighlightStop;
		
		_interactable.onHoverStart += OnHoverStart;
		_interactable.onHoverStop += OnHoverStop;
		
		_interactable.onSelection += OnSelection;
		_interactable.onDeselection += OnDeselection;
	}
	
	protected override void UnregisterForEvents ()
	{
		base.UnregisterForEvents ();
		_interactable.onHighlightStart -= OnHighlightStart;
		_interactable.onHighlightStop -= OnHighlightStop;
		
		_interactable.onHoverStart -= OnHoverStart;
		_interactable.onHoverStop -= OnHoverStop;
		
		_interactable.onSelection -= OnSelection;
		_interactable.onDeselection -= OnDeselection;
	}
	#endregion

	#region Outline
	protected void UpdateOutline()
	{
		foreach(InteractableRenderer each in _renderers)
		{
			if(_state.Has(EState.Selected))
			{
				each.EnableOutline(colorSettings.selected.ennemi);
			}
			else if(_state.Has(EState.Hovered))
			{
				each.EnableOutline(colorSettings.hovered.ennemi);
			}
			else if(_state.Has(EState.Highlighted))
			{
				each.EnableOutline(colorSettings.highlighted.ennemi);
			}
			else
			{
				each.DisableOutline();
			}
		}
	}
	#endregion
	
	#region Callbacks
	protected void OnHighlightStart ()
	{
		_state = _state.Include(EState.Highlighted);
		UpdateOutline();
	}

	protected void OnHighlightStop ()
	{
		_state = _state.Remove(EState.Highlighted);
		UpdateOutline();
	}

	protected void OnSelection ()
	{
		_state = _state.Include(EState.Selected);
		UpdateOutline();
	}

	protected void OnDeselection ()
	{
		_state = _state.Remove(EState.Selected);
		UpdateOutline();
	}

	protected void OnHoverStart ()
	{
		_state = _state.Include(EState.Hovered);
		UpdateOutline();
	}

	protected void OnHoverStop ()
	{
		_state = _state.Remove(EState.Hovered);
		UpdateOutline();
	}
	#endregion
}
