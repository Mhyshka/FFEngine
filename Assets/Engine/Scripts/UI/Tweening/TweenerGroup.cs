using UnityEngine;
using System.Collections;

namespace FF.UI
{
    [System.Serializable]
    internal class TweenPlayConf
    {
        public UITweener tweener = null;
        public bool isPlayForward = true;

        internal void Play()
        {
            tweener.Play(isPlayForward);
            tweener.ResetToBeginning();
        }

        internal void Sample(float a_ratio, bool a_isFinished)
        {
            if (isPlayForward)
                tweener.Sample(a_ratio, a_isFinished);
            else
                tweener.Sample(1 - a_ratio, a_isFinished);
        }
    }

    [System.Serializable]
    internal class TweenerGroup
    {
        #region Inspector Properties
        public TweenPlayConf[] tweeners = null;

        protected int _tweenerFinishCount = 0;
        #endregion
        
        protected void RegisterCallbacks()
        {
            foreach (TweenPlayConf each in tweeners)
            {
                each.tweener.onFinished.Clear();
                UITweener tween = each.tweener;
                each.tweener.onFinished.Add(new EventDelegate(() => OnTransitionFinish(tween)));
            }
        }

        internal void Play()
        {
            RegisterCallbacks();
            _tweenerFinishCount = 0;
            Debug.LogError("Play group");
            foreach (TweenPlayConf each in tweeners)
            {
                each.Play();
            }
        }

        internal void Sample(float a_ratio, bool a_isFinished)
        {
            RegisterCallbacks();
            _tweenerFinishCount = 0;
            foreach (TweenPlayConf each in tweeners)
            {
                each.Sample(a_ratio, a_isFinished);
            }
        }

        internal SimpleCallback onTransitionComplete = null;
        protected void OnTransitionFinish(UITweener a_tweener)
        {
            a_tweener.onFinished.Clear();
            _tweenerFinishCount++;

            Debug.LogError("transition finished");

            if (_tweenerFinishCount >= tweeners.Length)
            {
                if (onTransitionComplete != null)
                    onTransitionComplete();
            }
        }
    }
}