using UnityEngine;
using System.Collections;

namespace FF
{
	internal class FFTweenRotation : FFTween
	{		
		public Vector3 from = Vector3.zero;
		public Vector3 to = Vector3.zero;
		
		// Update is called once per frame
		protected override void Tween (float a_factor)
		{
			transform.localRotation = Quaternion.Euler(new Vector3(
				Mathf.Lerp(from.x, to.x, a_factor),
				Mathf.Lerp(from.y, to.y, a_factor),
				Mathf.Lerp(from.z, to.z, a_factor)));
		}
	}
}