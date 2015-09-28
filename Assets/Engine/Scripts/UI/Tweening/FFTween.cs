using UnityEngine;
using System.Collections;

namespace FF
{
	internal abstract class FFTween : MonoBehaviour
	{
		internal enum EFFTweenMode
		{
			Once,
			Loop,
			PingPong
		}
		
		public AnimationCurve curve = null;
		public float duration = 1f;
		public EFFTweenMode mode = EFFTweenMode.Loop;
		public bool randomStartOffset = false;
		public float startOffset = 0f;
		
		protected bool _isForward = true;
		
		protected float _timeElapsed = 0f;
		
		protected virtual void Awake()
		{
			if(randomStartOffset)
				_timeElapsed = Random.Range(0, duration);
			else
				_timeElapsed = startOffset;
		}
		
		void Update ()
		{
			if(_isForward)
				_timeElapsed += Time.deltaTime;
			else
				_timeElapsed -= Time.deltaTime;
				
			if(_timeElapsed >= duration && _isForward || 
			   _timeElapsed <= 0 && !_isForward)
			{
				switch(mode)
				{
					case EFFTweenMode.Loop : 
					_timeElapsed = _timeElapsed % duration;
					break;
					
					case EFFTweenMode.PingPong : 
					if(_isForward)
						_timeElapsed = duration - _timeElapsed % duration;
					else
						_timeElapsed = -_timeElapsed;
					_isForward = !_isForward;
					break;
					
					case EFFTweenMode.Once:
					enabled = false;
					break;
				}
				
			}
			float ratio = _timeElapsed / duration;
			float _factor = curve.Evaluate(ratio);
			
			Tween (_factor);
		}
		
		protected abstract void Tween(float a_factor);
	}
}