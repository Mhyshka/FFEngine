using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPlayerFeedbackOutline : AInteractableComponent, IHighlighCallback, ISelectionCallback, IHoverCallback
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
	public void OnHighlightStart ()
	{
		_state = _state.Include(EState.Highlighted);
		UpdateOutline();
	}

	public void OnHighlightStop ()
	{
		_state = _state.Remove(EState.Highlighted);
		UpdateOutline();
	}

	public void OnSelection ()
	{
		_state = _state.Include(EState.Selected);
		UpdateOutline();
	}

	public void OnDeselection ()
	{
		_state = _state.Remove(EState.Selected);
		UpdateOutline();
	}

	public void OnHoverStart ()
	{
		_state = _state.Include(EState.Hovered);
		UpdateOutline();
	}

	public void OnHoverStop ()
	{
		_state = _state.Remove(EState.Hovered);
		UpdateOutline();
	}
	#endregion
}
