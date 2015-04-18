using UnityEngine;
using System.Collections;

internal class ThreeStatesWidget : MonoBehaviour
{
	#region Inspector Properties
	public GameObject activeSprite = null;

	public UITweener[] 	activeTweens = null,
						newlyTweens = null;

	public UITweener idleTween = null;
	#endregion

	#region Properties

	#endregion

	internal void SetInactive()
	{
		activeSprite.SetActive (false);
		if (activeTweens.Length > 0 && activeTweens[0] != null)
		{
			activeTweens [0].Sample (0f, true);
		}
		if (idleTween != null) {
						idleTween.Sample (0.5f, true);
						idleTween.enabled = false;
				}
	}

	internal void SetActive()
	{
		activeSprite.SetActive (true);
		foreach (UITweener each in activeTweens)
		{
			each.ResetToBeginning();
			each.PlayForward();
		}
		if (idleTween != null) {
						idleTween.Sample (0.5f, true);
						idleTween.enabled = false;
				}
	}

	internal void SetNewly()
	{
		activeSprite.SetActive (true);
		foreach (UITweener each in newlyTweens)
		{
			each.ResetToBeginning();
			each.PlayForward();
		}
		if (idleTween != null) {
						idleTween.PlayForward ();
						idleTween.enabled = true;
				}
	}
}
