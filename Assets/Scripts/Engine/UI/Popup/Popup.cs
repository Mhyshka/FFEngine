using UnityEngine;
using System.Collections;



internal class Popup : FFPanel
{
	#region Inspector Properties
	public UILabel titleLabel = null;
	public UILabel contentLabel = null;

	public ClickEventButton closeButton = null;
	public ClickEventButton cancelButton = null;
	public ClickEventButton validButton = null;

	public UILabel closeLabel = null;
	public UILabel cancelLabel = null;
	public UILabel validLabel = null;
	#endregion

	#region Properties
	#endregion

	#region Popup Setting
	internal void SetValidData(string a_content)
	{
		closeButton.gameObject.SetActive (false);
		cancelButton.gameObject.SetActive (true);
		validButton.gameObject.SetActive (true);

		contentLabel.text = a_content;
	}


	internal void SetValidData(string a_title, string a_text)
	{
		closeButton.gameObject.SetActive (false);
		cancelButton.gameObject.SetActive (true);
		validButton.gameObject.SetActive (true);
		
		titleLabel.text = a_title;
		contentLabel.text = a_text;
	}

	internal void SetCloseData(string a_content)
	{
		closeButton.gameObject.SetActive (true);
		cancelButton.gameObject.SetActive (false);
		validButton.gameObject.SetActive (false);
		
		contentLabel.text = a_content;
	}

	internal void SetCloseData(string a_title, string a_text)
	{
		closeButton.gameObject.SetActive (true);
		cancelButton.gameObject.SetActive (false);
		validButton.gameObject.SetActive (false);
		
		titleLabel.text = a_title;
		contentLabel.text = a_text;
	}
	#endregion

	#region Buttons Text
	internal void SetButtonsText(string a_validText, string a_cancelText)
	{
		validLabel.text = a_validText;
		cancelLabel.text = a_cancelText;
	}

	internal void SetButtonText(string a_closeText)
	{
		closeLabel.text = a_closeText;
	}
	#endregion

	internal override void Show ()
	{
		FFEngine.Events.FireEvent("DisableInput");
		base.Show ();
	}

	internal override void Hide ()
	{
		FFEngine.Events.FireEvent("EnableInput");
		base.Hide ();
	}
}
