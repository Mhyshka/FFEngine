using UnityEngine;
using System.Collections;

public class UnitTarget : AUnitComponent
{
	internal Unit Unit = null;
	
	internal override void Init (AInteractable a_interactable)
	{
		Unit = a_interactable as Unit;
		base.Init (a_interactable);
	}
}
