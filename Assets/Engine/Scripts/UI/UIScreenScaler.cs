using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace FF.UI
{
	[RequireComponent(typeof(RectTransform))]
	internal class UIScreenScaler : MonoBehaviour
	{
		#region Inspector properties
		public bool adaptToScreenRatio = true;
		#endregion
		
		protected RectTransform _rect;
		internal RectTransform RectTransform
		{
			get
			{
				return _rect;
			}
		}
		
		protected float _baseWidth;
		
		void Awake()
		{
			_rect = GetComponent<RectTransform>();
			
			_baseWidth = _rect.sizeDelta.x;
			
			if(adaptToScreenRatio)
			{
				float ratio = (float)Screen.width / (float)Screen.height;
				float multiplier = ratio * 3f / 4f;
				SetWidthMultiplier(multiplier);
			}
		}
		
		internal void SetWidthMultiplier(float a_multiplier)
		{
			Vector2 size = _rect.sizeDelta;
			size.x = _baseWidth * a_multiplier;
			_rect.sizeDelta = size;
		}
	}
}