using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace FF.UI
{
    internal class FFLoadingPopup : FFMessagePopup
    {
        internal static new void RequestDisplay(string a_message, string a_buttonContent, SimpleCallback a_callback)
        {
            FFMessagePopupData data = new FFMessagePopupData();
            data.popupName = "LoadingPopup";
            data.messageContent = a_message;
            data.buttonContent = a_buttonContent;
            data.onOkayPressed = a_callback;

            FFEngine.UI.PushPopup(data);
        }
    }
}