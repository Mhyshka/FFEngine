using UnityEngine;
using System.Collections;

using UnityEngine.UI;

using FF.Network;
using FF.Multiplayer;

namespace FF.UI
{
    internal delegate void NetPlayerCallback(FFNetworkPlayer a_player);

    internal class FFClientSlotOptionPopupData : FFPopupData
    {
        internal FFNetworkPlayer player = null;

        internal NetPlayerCallback onSwapPressed = null;
        internal SimpleCallback onCancelPressed = null;
    }

    internal class FFClientSlotOptionPopup : FFPopup
    {
        #region Inspector Properties
        public Button swapButton = null;
        public Text playerLabel = null;
        #endregion

        #region Properties
        protected NetPlayerCallback _onSwapPressed = null;
        protected SimpleCallback _onCancelPressed = null;

        protected FFNetworkPlayer _player;
        #endregion

        internal override void SetContent(FFPopupData a_data)
        {
            base.SetContent(a_data);
            FFClientSlotOptionPopupData data = a_data as FFClientSlotOptionPopupData;

            playerLabel.text = data.player.player.username;

            _player = data.player;

            swapButton.enabled = !data.player.isDced;
            _onSwapPressed = data.onSwapPressed;
            _onCancelPressed = data.onCancelPressed;
        }

        #region Callback
        public void OnSwapPressed()
        {
            if (_onSwapPressed != null)
                _onSwapPressed(_player);
            else
                Engine.UI.DismissPopup(currentData.id);
        }

        public void OnCancelPressed()
        {
            if (_onCancelPressed != null)
                _onCancelPressed();
            else
                Engine.UI.DismissPopup(currentData.id);
        }
        #endregion

        internal static int RequestDisplay(FFNetworkPlayer a_player, NetPlayerCallback a_swapCallback, SimpleCallback a_cancelCallback, int a_priority = 0)
        {
            FFClientSlotOptionPopupData data = new FFClientSlotOptionPopupData();
            data.popupName = "ClientSlotOptionPopup";
            data.player = a_player;
            data.priority = a_priority;

            data.onSwapPressed = a_swapCallback;
            data.onCancelPressed = a_cancelCallback;

            Engine.UI.PushPopup(data);

            return data.id;
        }
    }
}