using UnityEngine;
using System.Collections;

namespace FF.UI
{
	internal class FFNavigationBarPanel : FFPanel
	{
		#region Inspector Properties
		public GameObject backButton = null;
		public UILabel title = null;
		#endregion

		internal void SetTitle (string newTitle)
		{
			title.text = newTitle;
		}
		
		internal void FocusBackButton()
		{
            UICamera.selectedObject = backButton;
		}
    }
}
