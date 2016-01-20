using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace FF
{
    internal abstract class ALoadingStep
    {
        internal abstract string Description
        {
            get;
        }

        internal abstract float Factor
        {
            get;
        }

        /// <summary>
        /// returns 1f for it to be complete
        /// </summary>
        internal abstract float Progress
        {
            get;
        }


        internal virtual bool IsComplete
        {
            get
            {
                return Progress == 1;
            }
        }

        internal abstract void Start();
        internal SimpleCallback onStart = null;
    }

    internal abstract class HardcodedLoadingStep : ALoadingStep
    {
        #region Properties
        internal float _factor = 100f;
        internal string _description = "Step one";
        #endregion

        internal HardcodedLoadingStep(string a_description, float a_factor)
        {
            _description = a_description;
            _factor = a_factor;
        }

        internal override float Factor
        {
            get
            {
                return _factor;
            }
        }

        internal override string Description
        {
            get
            {
                return _description;
            }
        }
    }



    internal class MainGameLoadingStep : ALoadingStep
    {
        #region Properties
        #endregion

        internal override string Description
        {
            get
            {
                return "Loading assets";
            }
        }

        internal override float Factor
        {
            get
            {
                return 100f;
            }
        }

        internal override float Progress
        {
            get
            {
                return Engine.Game.MainSceneLoadingProgress;
            }
        }

        //Launch from GameManager
        internal override void Start()
        {
            if (onStart != null)
                onStart();
        }
    }

    internal class AdditionalSceneLoadingStep : ALoadingStep
    {
        #region Properties
        protected LoadingState _loadingState;
        protected List<AsyncOperation> _additionalSceneAO = null;
        #endregion

        internal AdditionalSceneLoadingStep(LoadingState a_state)
        {
            _loadingState = a_state;
            _additionalSceneAO = new List<AsyncOperation>();
        }

        internal override string Description
        {
            get
            {
                return "Loading additional assets";
            }
        }

        internal override float Factor
        {
            get
            {
                if (_loadingState.additionalRequiredScenes == null)
                    return 0f;

                return 50f * _loadingState.additionalRequiredScenes.Length;
            }
        }

        internal override float Progress
        {
            get
            {
                if (_additionalSceneAO.Count > 0)
                {
                    float progress = 0f;
                    foreach (AsyncOperation each in _additionalSceneAO)
                    {
                        progress += each.progress;
                    }
                    progress = progress / _additionalSceneAO.Count;
                    return progress;
                }
                else
                    return 1f;
            }
        }

        //Launch from GameManager
        internal override void Start()
        {
            if (_loadingState.additionalRequiredScenes != null)
            {
                foreach (string each in _loadingState.additionalRequiredScenes)
                {
                    _additionalSceneAO.Add(SceneManager.LoadSceneAsync(each, LoadSceneMode.Additive));
                }
            }

            if (onStart != null)
                onStart();
        }
    }

    internal class UiLoadingStep : ALoadingStep
    {
        #region Properties
        protected string[] panelsToLoad;
        #endregion

        internal UiLoadingStep(string[] a_panelsToLoad)
        {
            panelsToLoad = a_panelsToLoad;
        }

        internal override string Description
        {
            get
            {
                return "Loading GUI";
            }
        }

        internal override float Factor
        {
            get
            {
                return 5f + Engine.UI.UILoadingFactor;
            }
        }

        internal override float Progress
        {
            get
            {
                return Engine.UI.UILoadingProgress;
            }
        }

        internal override void Start()
        {
            if (onStart != null)
                onStart();
            Engine.UI.LoadPanelsSet(panelsToLoad);
        }
    }

    internal class AsyncOperationLoadingStep : HardcodedLoadingStep
    {
        #region Properties
        AsyncOperation _ao;
        #endregion

        internal AsyncOperationLoadingStep(AsyncOperation a_ao, string a_description, float a_factor) : base(a_description, a_factor)
        {
            _ao = a_ao;
        }

        internal override float Progress
        {
            get
            {
                return _ao.progress;
            }
        }

        internal override void Start()
        {
            if (onStart != null)
                onStart();
        }
    }

    internal class AsyncOperationsLoadingStep : HardcodedLoadingStep
    {
        #region Properties
        AsyncOperation[] _aos;
        #endregion

        internal AsyncOperationsLoadingStep(AsyncOperation[] a_aos, string a_description, float a_factor) : base(a_description, a_factor)
        {
            _aos = a_aos;
        }

        internal override float Progress
        {
            get
            {
                float progress = 0f;
                foreach (AsyncOperation each in _aos)
                {
                    progress += each.progress;
                }

                if (_aos.Length > 0)
                    progress /= _aos.Length;
                else
                    progress = 1f;

                return progress;
            }
        }

        internal override void Start()
        {
            if (onStart != null)
                onStart();
        }
    }

    internal class ManualLoadingStep : HardcodedLoadingStep
    {
        #region Properties
        protected bool _isComplete;
        #endregion

        internal ManualLoadingStep(string a_description, float a_factor) : base(a_description, a_factor)
        {
            _isComplete = false;
        }

        internal void SetComplete()
        {
            _isComplete = true;
        }

        internal override float Progress
        {
            get
            {
                if (_isComplete)
                    return 1f;

                return 0f;
            }
        }

        internal override void Start()
        {
            if (onStart != null)
                onStart();
        }
    }
}
