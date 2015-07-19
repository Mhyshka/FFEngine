using UnityEngine;
using System.Collections;

internal class FFPanel : MonoBehaviour
{
	internal enum EState
	{
		Hidden,
		Showing,
		Shown,
		Hidding
	}

	#region Inspector Properties
	public bool debug = false;
	public bool isUsingTransition = true;
	public TweenerGroup tweens = null;
	public bool hideOnLoad = true;
	public bool hasTweenAlpha = true;
	#endregion

	#region Properties
	protected UIPanel panel;
	internal virtual bool ShouldMoveToRoot
	{
		get
		{
			return true;
		}
	}
	
	internal EState state = EState.Hidden;
	#endregion

	protected virtual void Awake()
	{
		panel = GetComponent<UIPanel>();
		if(!debug)
		{
			FFEngine.UI.Register (gameObject.name, this);
			if (hideOnLoad)
				OnHidden ();
			else
				OnShown ();
		}
	}

	protected virtual void OnDestroy()
	{
		FFEngine.UI.Unregister (gameObject.name);
	}
	
	/*internal bool ShouldStayAlive
	{
		get
		{
			return false;
		}
	}*/
	#region Show
	internal virtual void Show()
	{
		if(!hasTweenAlpha && panel != null)
			panel.alpha = 1f;
		if(isUsingTransition)
			PlayShowTransition ();
		else
			OnShown();
	}
	
	protected virtual void PlayShowTransition ()
	{
		//Debug.Log("Showing : " + gameObject.name);
		if (state == EState.Hidden)
		{
			state = EState.Showing;
			tweens.PlayForward ();
		}
		else if (state == EState.Hidding)
		{
			state = EState.Showing;
			tweens.Toggle ();
		}
	}
	#endregion

	#region Hide
	internal virtual void Hide()
	{
		if(isUsingTransition)
			PlayHideTransition ();
		else
			OnHidden();
	}

	protected virtual void PlayHideTransition ()
	{

		if (state == EState.Shown)
		{
			state = EState.Hidding;
			tweens.PlayReverse ();
		}
		else if (state == EState.Showing)
		{
			state = EState.Hidding;
			tweens.Toggle ();
		}
	}
	#endregion
	
	#region Transition Events
	public void OnTransitionComplete()
	{
		if (state == EState.Showing || state == EState.Hidden)
		{
			OnShown();
		}
		else if (state == EState.Hidding || state == EState.Shown)
		{
			OnHidden();
		}
	}

	internal virtual void OnShown()
	{
		//Debug.Log("On Shown : " + gameObject.name);
		state = EState.Shown;
	}

	internal virtual void OnHidden()
	{
		//Debug.Log("On Hidden : " + gameObject.name);
		state = EState.Hidden;
		if(panel != null)
			panel.alpha = 0f;
	}
	#endregion
}
