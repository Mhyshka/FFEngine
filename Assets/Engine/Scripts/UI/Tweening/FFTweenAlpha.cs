using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace FF
{
	internal class FFTweenAlpha : FFTween
	{
		public float from = 0f;
		public float to = 1f;
		
		
		protected MaskableGraphic[] uiElements;
		protected Shadow[] shadows;
		
		
		protected override void Awake ()
		{
			base.Awake ();
			uiElements = GetComponents<MaskableGraphic>();
			shadows = GetComponents<Shadow>();
		}
		
		protected override void Tween (float a_factor)
		{
			foreach(MaskableGraphic each in uiElements)
			{
				Color color = each.color;
				color.a = Mathf.Lerp(from, to, a_factor);
				each.color = color;
			}
			
			foreach(Shadow each in shadows)
			{
				Color color = each.effectColor;
				color.a = Mathf.Lerp(from, to, a_factor);
				each.effectColor = color;
			}
		}
	}
}