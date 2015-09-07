﻿using UnityEngine;
using System.Collections;

namespace FF
{
	[RequireComponent(typeof(RectTransform))]
	internal class UIRoot : MonoBehaviour
	{
		public bool adaptToScreenRatio = true;
		
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
		}
		
		internal void SetWidthMultiplier(float a_multiplier)
		{
			Vector2 size = _rect.sizeDelta;
			size.x = _baseWidth * a_multiplier;
			_rect.sizeDelta = size;
		}
	}
}