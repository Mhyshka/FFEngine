using UnityEngine;
using System.Collections;
using System;

namespace FF.UI
{
    internal class FFTweenCanvasGroupAlpha : FFTween
    {
        public float targetAlpha = 1f;

        protected CanvasGroup _canvasGroup = null;

        protected override void Awake()
        {
            base.Awake();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        protected override void Tween(float a_factor)
        {
            _canvasGroup.alpha = a_factor * targetAlpha;
        }
    }
}