using UnityEngine;
using System.Collections;

internal class YouWinPopupInteraction : Interactable
{

	#region Inspector Properties
	public string winText = "";
	public string cancelText = "";
	#endregion
	
	internal override void OnInteraction ()
	{
		base.OnInteraction ();
		Popup popup;
		
		if (!string.IsNullOrEmpty (winText))
		{
			popup = FFEngine.UI.GetPanel("WinPopup") as Popup;
			popup.SetCloseData (winText	);

			if(!string.IsNullOrEmpty(cancelText))
			{
				popup.SetButtonText(cancelText);
			}
			
			popup.Show ();
		}
	}
}
