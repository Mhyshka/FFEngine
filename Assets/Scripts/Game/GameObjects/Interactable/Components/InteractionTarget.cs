using UnityEngine;
using System.Collections;

public class InteractionTarget : AInteractableComponent
{
	internal AInteractable Interactable = null;
	
	internal override void Init (AInteractable a_interactable)
	{
		Interactable = a_interactable;
		base.Init (a_interactable);
	}
}
