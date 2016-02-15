using UnityEngine;
using System.Collections;

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
        public UILabel buttonNoLabel = null;

        protected SimpleCallback _onNoPressed;
        #endregion

        internal override void SetContent(FFPopupData a_data)
        {
            base.SetContent(a_data);
            FFYesNoPopupData data = a_data as FFYesNoPopupData;

            buttonNoLabel.text = data.buttonNoContent;
            buttonNoLabel.MarkAsChanged();

            _onNoPressed = data.onNoPressed;
        }

        public void OnNoPressed()
        {
            if (_onNoPressed != null)
                _onNoPressed();
            else
                Engine.UI.DismissPopup(currentData.id);
        }

        internal static int RequestDisplay(string a_message, string a_buttonYesContent, string a_buttonNoContent, SimpleCallback a_yesCallback, SimpleCallback a_noCallback, int a_priority = 0)
        {
            FFYesNoPopupData data = new FFYesNoPopupData();
            data.popupName = "YesNoPopup";
            data.priority = a_priority;
            data.messageContent = a_message;

            data.buttonContent = a_buttonYesContent;
            data.onOkayPressed = a_yesCallback;

            data.buttonNoContent = a_buttonNoContent;
            data.onNoPressed = a_noCallback;

            Engine.UI.PushPopup(data);

            return data.id;
        }

        protected override void OnBackPressed()
        {
            OnNoPressed();
        }
    }
}