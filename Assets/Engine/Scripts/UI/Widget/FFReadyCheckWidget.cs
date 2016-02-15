using UnityEngine;

namespace FF.UI
{
    internal class FFReadyCheckWidget : MonoBehaviour
    {
        #region Inspector Properties
        public UITweener[] tweeners = null;
        public GameObject pending = null;
        public GameObject fail = null;
        public GameObject success = null;
        #endregion

        internal void OnEnable()
        {
            foreach (UITweener each in tweeners)
            {
                each.ResetToBeginning();
                each.enabled = false;
            }
        }

        #region Show & Hide
        internal void Show()
        {
            foreach (UITweener each in tweeners)
            {
                each.PlayForward();
            }
        }

        internal void Hide()
        {
            foreach (UITweener each in tweeners)
            {
                each.PlayReverse();
            }
        }
        #endregion

        #region State
        internal void SetPending()
        {
            pending.SetActive(true);
            fail.SetActive(false);
            success.SetActive(false);
        }

        internal void SetFail()
        {
            pending.SetActive(false);
            fail.SetActive(true);
            success.SetActive(false);
        }

        internal void SetSuccess()
        {
            pending.SetActive(false);
            fail.SetActive(false);
            success.SetActive(true);
        }
        #endregion
    }
}
