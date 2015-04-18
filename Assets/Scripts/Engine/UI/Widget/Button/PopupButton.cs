using UnityEngine;
using System.Collections;

internal class PopupButton : ClickEventButton
{
	#region Inspector Properties
	public Popup popup = null;
	#endregion



	protected override void OnClick()
	{
		
		popup.Hide();

		FFEventParameter param = new FFEventParameter ();
		param.data = popup;
		FFEngine.Events.FireEvent (eventKey,param);
	}
}
