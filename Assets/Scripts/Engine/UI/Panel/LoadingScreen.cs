using UnityEngine;
using System.Collections;

internal class LoadingScreen : FFPanel
{
	protected override void Awake ()
	{
		FFEngine.UI.RegisterLoadingScreen(this);
		if (hideOnLoad)
			OnHidden ();
		else
			OnShown ();
	}
	
	protected override void OnDestroy ()
	{
	
	}

	/*protected override void PlayShowTransition()
	{
		tweens.SampleForward ();
		state = EState.Showing;
		OnTransitionComplete ();
	}*/
	
	/*internal bool ShouldStayAlive
	{
		get
		{
			return true;
		}
	}*/
}
