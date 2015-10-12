using UnityEngine;
using System.Collections;
using System;

using UnityEngine.UI;

namespace FF.UI
{
    internal class FFConnectionLostPopupData : FFPopupData
    {
        internal string titleContent = null;
        internal SimpleCallback onCancelPressed = null;
    }

    internal class FFConnectionLostPopup : FFPopup
    {
        #region Inspector Properties
        public Text timeLabel = null;

        protected SimpleCallback _onCancelPressed;
        protected float _timeElapsed;
        #endregion

        internal override void SetContent(FFPopupData a_data)
        {
            base.SetContent(a_data);
            FFConnectionLostPopupData data = a_data as FFConnectionLostPopupData;

            _onCancelPressed = data.onCancelPressed;

            _timeElapsed = 0f;
        }

        public void OnCancelPressed()
        {
            if (_onCancelPressed != null)
                _onCancelPressed();
            else
                FFEngine.UI.DismissPopup(currentData.id);
        }

        internal void Update()
        {
            _timeElapsed += Time.deltaTime;

            TimeSpan span = TimeSpan.FromSeconds(_timeElapsed);
            timeLabel.text = string.Format("{0}:{1}", span.Minutes.ToString("00"), span.Seconds.ToString("00"));
        }

        internal static int RequestDisplay(SimpleCallback a_callback)
        {
            FFConnectionLostPopupData data = new FFConnectionLostPopupData();
            data.popupName = "ConnectionLostPopup";
            data.onCancelPressed = a_callback;

            FFEngine.UI.PushPopup(data);

            return data.id;
        }

        protected override void OnBackPressed()
        {
            base.OnBackPressed();
            OnCancelPressed();
        }
    }
}