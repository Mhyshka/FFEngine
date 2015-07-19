using UnityEngine;
using System.Collections;

public abstract class AInteractableComponent : FullInspector.BaseBehavior
{
	#region Inspector Properties
	#endregion
	
	#region Properties
	protected AInteractable _interactable;
	#endregion
	
	#region Methods
	protected override void Awake ()
	{
		base.Awake ();
		enabled = false;
	}
	
	internal virtual void Init(AInteractable a_interactable)
	{
		_interactable = a_interactable;
		enabled = true;
	}
	
	protected virtual void OnDestroy()
	{
		UnregisterForEvents();
	}
	
	internal virtual void RegisterForEvents()
	{
	
	}
	
	protected virtual void UnregisterForEvents()
	{
	
	}
	#endregion
}