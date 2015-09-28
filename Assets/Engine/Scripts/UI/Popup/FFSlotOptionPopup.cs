using UnityEngine;
using System.Collections;

using UnityEngine.UI;

using FF.Networking;

namespace FF.UI
{
    internal delegate void NetPlayerCallback(FFNetworkPlayer a_player);

    internal class FFSlotOptionPopupData : FFPopupData
    {
        internal FFNetworkPlayer player = null;

        internal NetPlayerCallback onKickPressed = null;
        internal NetPlayerCallback onBanPressed = null;
        internal NetPlayerCallback onSwapPressed = null;
        internal SimpleCallback onCancelPressed = null;
    }

    internal class FFSlotOptionPopup : FFPopup
    {
        #region Inspector Properties
        public Text playerLabel = null;

        protected NetPlayerCallback _onKickPressed = null;
        protected NetPlayerCallback _onBanPressed = null;
        protected NetPlayerCallback _onSwapPressed = null;
        protected SimpleCallback _onCancelPressed = null;

        protected FFNetworkPlayer _player;
        #endregion

        internal override void SetContent(FFPopupData a_data)
        {
            base.SetContent(a_data);
            FFSlotOptionPopupData data = a_data as FFSlotOptionPopupData;

            playerLabel.text = data.player.player.username;

            _player = data.player;

            _onKickPressed = data.onKickPressed;
            _onBanPressed = data.onBanPressed;
            _onSwapPressed = data.onSwapPressed;
            _onCancelPressed = data.onCancelPressed;
        }

        public void OnSwapPressed()
        {
            if (_onSwapPressed != null)
                _onSwapPressed(_player);
            else
                FFEngine.UI.DismissCurrentPopup();
        }

        public void OnKickPressed()
        {
            if (_onKickPressed != null)
                _onKickPressed(_player);
            else
                FFEngine.UI.DismissCurrentPopup();
        }

        public void OnBanPressed()
        {
            if (_onBanPressed != null)
                _onBanPressed(_player);
            else
                FFEngine.UI.DismissCurrentPopup();
        }

        public void OnCancelPressed()
        {
            if (_onCancelPressed != null)
                _onCancelPressed();
            else
                FFEngine.UI.DismissCurrentPopup();
        }

        internal static void RequestDisplay(FFNetworkPlayer a_player, NetPlayerCallback a_kickCallback, NetPlayerCallback a_banCallback, NetPlayerCallback a_swapCallback, SimpleCallback a_cancelCallback)
        {
            FFSlotOptionPopupData data = new FFSlotOptionPopupData();
            data.popupName = "SlotOptionPopup";
            data.player = a_player;

            data.onKickPressed = a_kickCallback;
            data.onBanPressed = a_banCallback;
            data.onSwapPressed = a_swapCallback;
            data.onCancelPressed = a_cancelCallback;

            FFEngine.UI.PushPopup(data);
        }
    }
}