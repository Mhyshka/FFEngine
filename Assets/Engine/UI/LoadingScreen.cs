using UnityEngine;
using System.Collections;

namespace FFEngine
{
	internal class LoadingScreen : FFPanel
	{
		protected override void Awake ()
		{
			if(!debug)
				FFEngine.UI.RegisterLoadingScreen(this);
				
			base.Awake();
		}
		
		protected override void OnDestroy ()
		{
		
		}
	}
}