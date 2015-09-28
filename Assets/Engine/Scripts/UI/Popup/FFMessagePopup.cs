using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace FF.UI
{
    internal class FFMessagePopupData : FFPopupData
    {
        internal string messageContent = null;
        internal string buttonContent = null;
        internal SimpleCallback onOkayPressed = null;
    }

    internal class FFMessagePopup : FFPopup
    {
        #region Inspector Properties
        public Text messageLabel = null;
        public Text buttonLabel = null;

        protected SimpleCallback _onOkayPressed;
        #endregion

        internal override void SetContent(FFPopupData a_data)
        {
            base.SetContent(a_data);
            FFMessagePopupData data = a_data as FFMessagePopupData;
            
            messageLabel.text = data.messageContent;
            buttonLabel.text = data.buttonContent;

            _onOkayPressed = data.onOkayPressed;
        }

        public void OnOkayPressed()
        {
            if (_onOkayPressed != null)
                _onOkayPressed();
            else
                FFEngine.UI.DismissCurrentPopup();
        }

        internal static void RequestDisplay(string a_message, string a_buttonContent, SimpleCallback a_callback)
        {
            FFMessagePopupData data = new FFMessagePopupData();
            data.popupName = "MessagePopup";
            data.messageContent = a_message;
            data.buttonContent = a_buttonContent;
            data.onOkayPressed = a_callback;

            FFEngine.UI.PushPopup(data);
        }
    }
}