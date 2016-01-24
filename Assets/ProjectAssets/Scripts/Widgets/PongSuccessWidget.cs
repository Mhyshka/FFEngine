using UnityEngine;
using UnityEngine.UI;

namespace FF.Pong
{
    internal class PongSuccessWidget : MonoBehaviour
    {
        #region Inspector Properties
        public Text successLabel = null;
        public Text playerLabel = null;
        public FFTween showTween = null;
        public Color blueSideColor = Color.blue;
        public Color purpleSideColor = Color.cyan;
        #endregion

        #region Properties
        internal SimpleCallback onSuccessDisplayComplete = null;
        #endregion

        void Awake()
        {
            showTween.onTransitionForwardComplete += OnTransitionForwardComplete;
        }

        void OnDestroy()
        {
            showTween.onTransitionForwardComplete += OnTransitionForwardComplete;
        }

        internal void Prepare()
        {
            showTween.Sample(0f);
            showTween.enabled = false;
        }

        internal void DisplaySuccess(SuccessContent a_successContent)
        {
            showTween.PlayForward(true);

            successLabel.text = a_successContent.successTitle;

            //Color color = Color.white;
            if (a_successContent.side == ESide.Left)
            {
                playerLabel.color = blueSideColor;
            }
            else if (a_successContent.side == ESide.Right)
            {
                playerLabel.color = purpleSideColor;
            }
            //string hexCode = ColorUtility.ToHtmlStringRGB(color);

            //playerLabel.text = "<color=\"" + hexCode + "\">" + a_successContent.playerName + "</color>" + " - " + a_successContent.scoreToDisplay;
            playerLabel.text = a_successContent.playerName + " - " + a_successContent.scoreToDisplay;
        }

        protected void OnTransitionForwardComplete()
        {
            if (onSuccessDisplayComplete != null)
                onSuccessDisplayComplete();
        }
    }

    internal class SuccessContent
    {
        internal string successTitle;
        internal string playerName;
        internal string scoreToDisplay;
        internal ESide side;

        internal SuccessContent(string a_successTitle, string a_playerName, string a_scoreToDisplay, ESide a_side)
        {
            successTitle = a_successTitle;
            playerName = a_playerName;
            scoreToDisplay = a_scoreToDisplay;
            side = a_side;
        }
    }
}
