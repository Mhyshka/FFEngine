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
	#endregion
}

internal interface IHoverCallback
{
	/// <summary>
	/// Called when the player starts hovering this GO
	/// </summary>
	void OnHoverStart();
	
	/// <summary>
	/// Called when the player stops hovering this GO
	/// </summary>
	void OnHoverStop();
}

internal interface IHighlighCallback
{
	/// <summary>
	/// Called when the GO should be highlighted
	/// </summary>
	void OnHighlightStart();
	
	/// <summary>
	/// Called when the GO shouldn't be highlight anymore
	/// </summary>
	void OnHighlightStop();
}

internal interface ISelectionCallback
{
	/// <summary>
	/// Called when the player select this GO
	/// </summary>
	void OnSelection();
	
	/// <summary>
	/// Called when the player deselect this GO
	/// </summary>
	void OnDeselection();
}

internal interface IInteractionCallback
{
	/// <summary>
	/// Called when the player use the interact action on this GO
	/// </summary>
	void OnInteraction();
	
	/*/// <summary>
	/// Called when the player use the interact action on this GO
	/// </summary>
	void OnInteractionEnd();
	
	/// <summary>
	/// Called when the player use the interact action on this GO
	/// </summary>
	void OnInteractionCanceled();
	
	/// <summary>
	/// Called when the player use the interact action on this GO
	/// </summary>
	void OnInteraction();*/
}