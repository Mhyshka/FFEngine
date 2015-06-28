using UnityEngine;
using System.Collections;

internal class MenuInputPanel : FFPanel
{
 #region Inspector Properties
 	public TweenerGroup pollTweeners = null;
 	public UILabel pollLabel = null;
 	public BoxCollider pollCollider = null;
 	
 	public InputBindingWidgetKey[] keyWidgets = null;
 #endregion

 #region Properties
 #endregion

 #region Methods
	internal void DisplayKeyPoll(string a_eventKey)
	{
		pollLabel.text = "Press any key to set a new binding for " + a_eventKey + " or <esc> to cancel.";
		pollTweeners.PlayForward();
		pollCollider.enabled = true;
	}
	
	internal void HidePoll()
	{
		pollTweeners.PlayReverse();
		pollCollider.enabled = false;
	}
 #endregion

}
