using UnityEngine;
using System.Collections;

using FF.UI;

namespace FF
{
	internal abstract class ANavigationMenuState : ANetworkMenuState
	{
		#region Properties
		protected FFNavigationBarPanel _navigationPanel = null;
		#endregion
		
		internal override void Enter ()
		{
			base.Enter ();
			if(_navigationPanel == null)
				_navigationPanel = Engine.UI.GetPanel("NavigationBarPanel") as FFNavigationBarPanel;
		}
	}
}