using UnityEngine;

namespace FF.UI
{
    internal class PlayerSlotStateWidget : MonoBehaviour
    {
        #region Inspector Properties
        [Header("Widgets references")]
        public UISprite backgroundImage = null;

        [Header("Configuration")]
        public bool needsTvIcon = false;
        public bool needsHostIcon = false;
        public bool needsRating = false;
        public Color usernameColor = Color.white;
        #endregion
    }
}