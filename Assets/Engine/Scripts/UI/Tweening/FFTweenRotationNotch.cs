using UnityEngine;
using System.Collections;

namespace FF
{
	internal class FFTweenRotationNotch : FFTween
	{		
		public Vector3 from = Vector3.zero;
		public Vector3 to = Vector3.zero;
		
		public int notchCount = 12;
		
		// Update is called once per frame
		protected override void Tween (float a_factor)
		{
			float notchSize = 1f / notchCount;
			float currentDelta = Mathf.FloorToInt(a_factor / notchSize);
			float factor = currentDelta / notchCount;
			
			transform.localRotation = Quaternion.Euler(new Vector3(
				Mathf.Lerp(from.x, to.x, factor),
				Mathf.Lerp(from.y, to.y, factor),
				Mathf.Lerp(from.z, to.z, factor)));
		}
	}
}