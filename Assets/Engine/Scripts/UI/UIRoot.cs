using UnityEngine;
using System.Collections;

namespace FF.UI
{
	internal class UIRoot : MonoBehaviour
	{
        #region Inspector properties
        public Camera uiCamera = null;
		#endregion

		
		void Awake()
		{
			Engine.UI.RegisterRoot(this);
			ConfigureEnabledInput();
		}
		
		internal void ConfigureEnabledInput()
		{
		}
	}
}