using UnityEngine;
using System.Collections;

namespace FF
{
	internal class FFTweenPosition : FFTween
	{
		public Vector3 from = Vector3.zero;
		public Vector3 to = Vector3.zero;
		
		// Update is called once per frame
		protected override void Tween (float a_factor)
		{
			/*if(transform is RectTransform)
			{
				RectTransform tr = transform as RectTransform;
				tr.localPosition = Vector3.Lerp(from, to, a_factor);
			}
			else*/
				transform.localPosition = Vector3.Lerp(from, to, a_factor);
		}
	}
}