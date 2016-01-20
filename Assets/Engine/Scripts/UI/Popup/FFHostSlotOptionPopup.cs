using UnityEngine;
using System.Collections;

using UnityEngine.UI;

using FF.Network;
using FF.Multiplayer;

namespace FF.UI
{
    internal class FFHostSlotOptionPopupData : FFClientSlotOptionPopupData
    {
        internal NetPlayerCallback onKickPressed = null;
        internal NetPlayerCallback onBanPressed = null;
    }

    internal class FFHostSlotOptionPopup : FFClientSlotOptionPopup
    {
        #region Inspector Properties
        protected NetPlayerCallback _onKickPressed = null;
        protected NetPlayerCallback _onBanPressed = null;
        #endregion

        internal override void SetContent(FFPopupData a_data)
        {
            base.SetContent(a_data);
            FFHostSlotOptionPopupData data = a_data as FFHostSlotOptionPopupData;

            _onKickPressed = data.onKickPressed;
            _onBanPressed = data.onBanPressed;
        }

        #region Callbacks
        public void OnKickPressed()
        {
            if (_onKickPressed != null)
                _onKickPressed(_player);
            else
                Engine.UI.DismissPopup(currentData.id);
        }

        public void OnBanPressed()
        {
            if (_onBanPressed != null)
                _onBanPressed(_player);
            else
                Engine.UI.DismissPopup(currentData.id);
        }
        #endregion

        internal static int RequestDisplay(FFNetworkPlayer a_player, NetPlayerCallback a_kickCallback, NetPlayerCallback a_banCallback, NetPlayerCallback a_swapCallback, SimpleCallback a_cancelCallback, int a_priority = 0)
        {
            FFHostSlotOptionPopupData data = new FFHostSlotOptionPopupData();
            data.popupName = "HostSlotOptionPopup";
            data.player = a_player;
            data.priority = a_priority;

            data.onKickPressed = a_kickCallback;
            data.onBanPressed = a_banCallback;
            data.onSwapPressed = a_swapCallback;
            data.onCancelPressed = a_cancelCallback;

            Engine.UI.PushPopup(data);

            return data.id;
        }

        protected override void OnBackPressed()
        {
            base.OnBackPressed();
            OnCancelPressed();
        }
    }
}