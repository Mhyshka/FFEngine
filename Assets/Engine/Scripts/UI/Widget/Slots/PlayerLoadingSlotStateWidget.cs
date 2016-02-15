using UnityEngine;

namespace FF.UI
{
    internal class PlayerLoadingSlotStateWidget : MonoBehaviour
    {
        #region Inspector Properties
        [Header("Widgets references")]
        public UISprite backgroundImage = null;

        [Header("Configuration")]
        public bool needsRating = true;
        public bool needsRanking = true;
        public Color usernameColor = Color.white;
        #endregion
    }
}