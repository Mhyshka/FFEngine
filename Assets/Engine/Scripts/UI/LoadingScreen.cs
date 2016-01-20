using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace FF.UI
{
    internal class LoadingScreen : FFPanel
    {
        #region Inspector Properties
        public Slider loadingProgress = null;
        public Text loadingDescription = null;
        #endregion
        protected override void Awake()
        {
            if (!debug)
                Engine.UI.RegisterLoadingScreen(this);

            base.Awake();
        }

        protected override void OnDestroy()
        {
            Debug.LogError("Destroyed loading");
        }

        protected override bool NeedsTobeRegister
        {
            get
            {
                return false;
            }
        }

        internal void SetLoadingDescription(string a_text)
        {
            loadingDescription.text = a_text;
        }

        internal void SetProgress(float a_value)
        {
            loadingProgress.value = a_value;
        }

        void Update()
        {
            SetProgress(Engine.Game.Loading.MainSceneLoadingProgress);
        }
    }
}