using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace FF.UI
{
    internal class FFLoadingPopupData : FFMessagePopupData
    {
        internal bool needsCancelButton = true;
    }

    internal class FFLoadingPopup : FFMessagePopup
    {
        public GameObject cancelButton = null;

        internal override void SetContent(FFPopupData a_data)
        {
            base.SetContent(a_data);
            FFLoadingPopupData data = a_data as FFLoadingPopupData;
            cancelButton.SetActive(data.needsCancelButton);
        }

        internal static int RequestDisplay(string a_message, string a_buttonContent, SimpleCallback a_callback, bool a_needCancelButton = true)
        {
            FFLoadingPopupData data = new FFLoadingPopupData();
            data.popupName = "LoadingPopup";
            data.needsCancelButton = a_needCancelButton;
            data.messageContent = a_message;
            data.buttonContent = a_buttonContent;
            data.onOkayPressed = a_callback;

            FFEngine.UI.PushPopup(data);

            return data.id;
        }
    }
}