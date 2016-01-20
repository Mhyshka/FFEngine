using UnityEngine;
using System.Collections;

namespace FF.UI
{
    internal class FFPopupData
    {
        internal int id = 0;
        internal int priority = 0;
        internal string popupName = null;

        internal FFPopupData()
        {
            id = Engine.UI.NextPopupId;
        }
    }

    internal abstract class FFPopup : FFPanel
    {
        internal FFPopupData currentData;

        internal virtual void SetContent(FFPopupData a_data)
        {
            currentData = a_data;
        }

        private bool _isRegisteredToBack = false;
        internal override void Show(bool a_isForward = true)
        {
            base.Show(a_isForward);
            Engine.Inputs.PushOnBackCallback(OnBackPressed);
            _isRegisteredToBack = true;
        }

        internal override void Hide(bool a_isForward = true)
        {
            base.Hide(a_isForward);
            if (_isRegisteredToBack)
            {
                Engine.Inputs.PopOnBackCallback();
                _isRegisteredToBack = false;
            }
        }

        protected virtual void OnBackPressed()
        {
        }
    }
}