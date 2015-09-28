using UnityEngine;
using System.Collections;

namespace FF
{
	internal class FFTweenScale : FFTween
	{
		public Vector3 from = Vector3.one;
		public Vector3 to = Vector3.one;
		
		protected override void Tween (float a_factor)
		{
			transform.localScale = Vector3.Lerp(from, to, a_factor);
		}
	}
}