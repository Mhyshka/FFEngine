using UnityEngine;
using System.Collections;

internal class LoseInteraction : Interactable
{
	internal override void OnInteraction ()
	{
		base.OnInteraction ();
		FFEngine.Events.FireEvent("TimesUp");
	}
}