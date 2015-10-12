using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace FF.UI
{
    internal class FFYesNoPopupData : FFMessagePopupData
    {
        internal string buttonNoContent = null;
        internal SimpleCallback onNoPressed = null;
    }

    internal class FFYesNoPopup : FFMessagePopup
    {
        #region Inspector Properties
        public Text buttonNoLabel = null;

        protected SimpleCallback _onNoPressed;
        #endregion

        internal override void SetContent(FFPopupData a_data)
        {
            base.SetContent(a_data);
            FFYesNoPopupData data = a_data as FFYesNoPopupData;
            buttonNoLabel.text = data.buttonNoContent;
            _onNoPressed = data.onNoPressed;
        }

        public void OnNoPressed()
        {
            if (_onNoPressed != null)
                _onNoPressed();
            else
                FFEngine.UI.DismissPopup(currentData.id);
        }

        internal static int RequestDisplay(string a_message, string a_buttonYesContent, string a_buttonNoContent, SimpleCallback a_yesCallback, SimpleCallback a_noCallback)
        {
            FFYesNoPopupData data = new FFYesNoPopupData();
            data.popupName = "YesNoPopup";
            data.messageContent = a_message;

            data.buttonContent = a_buttonYesContent;
            data.onOkayPressed = a_yesCallback;

            data.buttonNoContent = a_buttonNoContent;
            data.onNoPressed = a_noCallback;

            FFEngine.UI.PushPopup(data);

            return data.id;
        }

        protected override void OnBackPressed()
        {
            OnNoPressed();
        }
    }
}