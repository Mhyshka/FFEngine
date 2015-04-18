using UnityEngine;
using System.Collections;
using System;

internal class ResultPanel : FFPanel
{
	#region Inspector Properties
	public UILabel title = null;
	public UILabel timeLeft = null;
	public UILabel timeImprovement = null;
	public UILabel totalTime = null;

	public RatingWidget rating = null;
	
	public ClickEventButton cancelButton = null;
	public ClickEventButton validButton = null;

	public UILabel cancelLabel = null;
	public UILabel validLabel = null;
	#endregion

	internal void SetResult(float a_score, int a_rating)
	{
		TimeSpan span = TimeSpan.FromSeconds (a_score);
		timeLeft.text = "\""  + span.Seconds.ToString ("00") + "' " + (span.Milliseconds / 10).ToString ("00");

		timeImprovement.enabled = false;

		rating.SetRating (a_rating, 0);
	}

	internal void SetResult(float a_score, int a_rating, int a_newStars, float a_timeImprovement)
	{
		TimeSpan span = TimeSpan.FromSeconds (a_score);
		timeLeft.text = span.Seconds.ToString ("00") + "' " + (span.Milliseconds / 10).ToString ("00") + "\"";

		timeImprovement.enabled = true;
		
		rating.SetRating (a_rating, a_newStars);
	}

	internal void SetButtonsText(string a_validText, string a_cancelText)
	{
		validLabel.text = a_validText;
		cancelLabel.text = a_cancelText;
	}

	internal override void Show ()
	{
		base.Show ();
		FFEngine.Events.FireEvent("DisableInput");
	}
	
	internal override void Hide ()
	{
		base.Hide ();
		FFEngine.Events.FireEvent("EnableInput");
	}

	internal void SetTotalTime(float a_time)
	{
		TimeSpan span = TimeSpan.FromSeconds (a_time);
		totalTime.text = span.Seconds.ToString ("00") + "' " + (span.Milliseconds / 10).ToString ("00") + "\"";
	}
}
