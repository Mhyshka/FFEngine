using UnityEngine;
using System.Collections;
using System;

namespace FF.UI
{
    internal class FFTweenCanvasGroupAlpha : FFTween
    {
        public float targetAlpha = 1f;

        public CanvasGroup canvasGroup = null;

        protected override void Awake()
        {
            base.Awake();
            if(canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();
        }

        protected override void Tween(float a_factor)
        {
            canvasGroup.alpha = a_factor * targetAlpha;
        }
    }
}