﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace FF.UI
{
	internal class FFNavigationBarPanel : FFPanel
	{
		#region Inspector Properties
		public GameObject backButton = null;
		public Text title = null;
        public Animator wifiWarningAnimator = null;
        public Animator loaderAnimator = null;
        public Text loaderText = null;

        protected bool _loaderIsShown = false;

        protected bool _wifiWarningIsShown = false;
		#endregion

		internal void SetTitle (string newTitle)
		{
			title.text = newTitle;
		}
		
		internal void FocusBackButton()
		{
			EventSystem.current.SetSelectedGameObject(backButton);
		}

        internal void ShowWifiWarning()
        {
            FFLog.LogError("Try Show wifi warning");
            if (!_wifiWarningIsShown)
            {
                FFLog.LogError("Showing wifi warning");
                wifiWarningAnimator.SetTrigger("Show");
                _wifiWarningIsShown = true;
            }
        }

        internal void HideWifiWarning()
        {
            FFLog.LogError("Try Hide wifi warning");
            if (_wifiWarningIsShown)
            {
                FFLog.LogError("Hiding wifi warning");
                wifiWarningAnimator.SetTrigger("Hide");
                _wifiWarningIsShown = false;
            }
        }

        internal void ShowLoader(string a_message)
        {
            if (!_loaderIsShown)
            {
                loaderAnimator.SetTrigger("Show");
                _loaderIsShown = true;
                loaderText.text = a_message;
            }
        }

        internal void HideLoader()
        {
            if (_loaderIsShown)
            {
                loaderAnimator.SetTrigger("Hide");
                _loaderIsShown = false;
            }
        }
    }
}
