using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace FF.UI
{
	[RequireComponent(typeof(RectTransform))]
	internal class UIRoot : MonoBehaviour
	{
		#region Inspector properties
		public bool adaptToScreenRatio = true;
		public TouchInputModule touchModule = null;
		public StandaloneInputModule standaloneModule = null;
		#endregion
		
		protected RectTransform _rect;
		protected float _baseWidth;
		
		void Awake()
		{
			FFEngine.UI.RegisterRoot(this);
			_rect = GetComponent<RectTransform>();
			
			_baseWidth = _rect.sizeDelta.x;
			
			if(adaptToScreenRatio)
			{
				float ratio = (float)Screen.width / (float)Screen.height;
				float multiplier = ratio * 3f / 4f;
				SetWidthMultiplier(multiplier);
			}
			
			ConfigureEnabledInput();
		}
		
		internal void SetWidthMultiplier(float a_multiplier)
		{
			Vector2 size = _rect.sizeDelta;
			size.x = _baseWidth * a_multiplier;
			_rect.sizeDelta = size;
		}
		
		internal void ConfigureEnabledInput()
		{
#if !UNITY_EDITOR
			if(FFEngine.Inputs.HasJoystickConnected || FFEngine.MultiScreen.IsTV)
			{
				touchModule.enabled = false;
				standaloneModule.enabled = true;
			}
			else
			{
				touchModule.enabled = true;
				standaloneModule.enabled = false;
			}
#endif
		}
	}
}