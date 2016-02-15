using UnityEngine;
using System.Collections;

namespace FF.UI
{
    internal class FFMessageToastData : FFToastData
    {
        internal string messageContent = null;
        internal float duration = 0f;
    }

    internal class FFMessageToast : FFToast
    {
        public UILabel messageLabel = null;

        protected override float Duration
        {
            get
            {
                FFMessageToastData data = currentData as FFMessageToastData;
                return data.duration;
            }
        }

        internal override void SetContent(FFToastData a_data)
        {
            base.SetContent(a_data);
            FFMessageToastData data = a_data as FFMessageToastData;
            messageLabel.text = data.messageContent;
            messageLabel.MarkAsChanged();
        }

        internal static void RequestDisplay(string a_message, float a_duration = 3f)
        {
            FFMessageToastData data = new FFMessageToastData();
            data.toastName = "MessageToast";
            data.duration = a_duration;
            data.messageContent = a_message;
            Engine.UI.PushToast(data);
        }
    }
}