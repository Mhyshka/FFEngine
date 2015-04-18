using UnityEngine;
using System.Collections;

internal class WinInteraction : Interactable
{
	internal override void OnInteraction ()
	{
		base.OnInteraction ();
		FFEngine.Events.FireEvent("LevelCompletion");
	}
}
