using UnityEngine;
using System.Collections;

namespace FF.UI
{
    internal class FFPopupData
    {
        internal string popupName = null;
    }

    internal abstract class FFPopup : FFPanel
    {
        internal FFPopupData currentData;

        internal virtual void SetContent(FFPopupData a_data)
        {
            currentData = a_data;
        }
    }
}