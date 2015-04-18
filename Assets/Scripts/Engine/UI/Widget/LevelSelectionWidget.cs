using UnityEngine;
using System.Collections;

[System.Serializable]
internal class TweenerGroup
{
	#region Inspector Properties
	public UITweener[] inTweeners = null;
	public UITweener[] outTweeners = null;
	public UITweener[] dualTweeners = null;
	public bool isActive = false;
	#endregion

	internal void PlayForward()
	{
		foreach(UITweener each in inTweeners)
		{
			//each.ResetToBeginning();
			each.PlayForward();
		}

		foreach(UITweener each in dualTweeners)
		{
			//each.ResetToBeginning();
			each.PlayForward();
		}
		isActive = true;
	}

	internal void PlayReverse()
	{
		foreach(UITweener each in outTweeners)
		{
			//each.ResetToBeginning();
			each.PlayForward();
		}
		
		foreach(UITweener each in dualTweeners)
		{
			//each.ResetToBeginning();
			each.PlayReverse();
		}
		isActive = false;
	}

	internal void SampleForward()
	{
		foreach(UITweener each in inTweeners)
		{
			each.Sample(1f,true);
		}

		foreach(UITweener each in outTweeners)
		{
			each.Sample(0f,true);
		}
		
		foreach(UITweener each in dualTweeners)
		{
			each.Sample(1f,true);
		}
		isActive = false;
	}

	internal void SampleBackward()
	{
		foreach(UITweener each in inTweeners)
		{
			each.Sample(0f,true);
		}

		foreach(UITweener each in outTweeners)
		{
			each.Sample(1f,true);
		}
		
		foreach(UITweener each in dualTweeners)
		{
			each.Sample(0f,true);
		}
		isActive = false;
	}

	internal void Toggle()
	{
		foreach(UITweener each in inTweeners)
		{
			if(each.enabled)
				each.Toggle ();
		}
		
		foreach(UITweener each in outTweeners)
		{
			if(each.enabled)
				each.Toggle ();
		}
		
		foreach(UITweener each in dualTweeners)
		{
			if(each.enabled)
				each.Toggle ();
		}
		isActive = false;
	}
}

internal class LevelSelectionWidget : MonoBehaviour
{
	#region Inspector Properties
	public UILabel label = null;
	public RequestLevelButton button = null;
	public TweenerGroup selectedTweeners = null;
	public TweenerGroup hoverTweeners = null;
	#endregion
	
	protected void Awake ()
	{
		if (selectedTweeners.isActive)
			selectedTweeners.SampleForward ();
		else
			selectedTweeners.SampleBackward ();

		if (hoverTweeners.isActive)
			hoverTweeners.PlayForward ();
	}

	internal void SetData(GameLevelData a_data, int a_index)
	{
		label.text = a_data.levelName;
		button.levelIndex = a_index;
	}

	internal void SetSelected()
	{
		selectedTweeners.PlayForward ();
	}

	internal void SetUnselected()
	{
		selectedTweeners.PlayReverse ();
	}
}
