using UnityEngine;

namespace FF.UI
{
    internal class PlayerLoadingSlotDcedStateWidget : PlayerLoadingSlotStateWidget
    {
        #region Inspector Properties
        [Header("Widgets references")]
        public UILabel dcedTimer = null;
        public GameObject kickGo = null;
        #endregion
    }
}