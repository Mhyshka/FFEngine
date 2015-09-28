using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace FF.UI
{
	internal class UIRoot : MonoBehaviour
	{
		#region Inspector properties
		public TouchInputModule touchModule = null;
		public StandaloneInputModule standaloneModule = null;
		#endregion

		
		void Awake()
		{
			FFEngine.UI.RegisterRoot(this);
			ConfigureEnabledInput();
		}
		
		internal void ConfigureEnabledInput()
		{
#if !UNITY_EDITOR
			if(FFEngine.Inputs.ShouldUseNavigation)
			{
				touchModule.enabled = false;
				standaloneModule.enabled = true;
			}
			else
			{
				touchModule.enabled = true;
				standaloneModule.enabled = false;
			}
#else
			touchModule.enabled = false;
			standaloneModule.enabled = true;
#endif
		}
	}
}