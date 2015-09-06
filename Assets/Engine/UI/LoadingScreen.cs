using UnityEngine;
using System.Collections;

namespace FFEngine
{
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
	}
}