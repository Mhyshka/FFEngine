using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

namespace FF
{
    //Custom inspector
    public class LoadingState : AGameState
    {
        #region Inspector Properties
        //public bool needsLoadingScreen = false;

        public string[] additionalRequiredScenes = null;

        public string[] panelsToLoad = null;
        #endregion

        #region Properties
        protected float POST_LOADING_DURATION = 0.1f;
        protected float _postLoadTimeElapsed = 0f;

        protected bool _didFinishLoading = false;
        protected bool _exitLoadingState = false;

        //Access issue
        internal List<ALoadingStep> _loadingSteps;
        protected int _currentStepIndex;
        #endregion

        #region States Methods
        internal override int ID
        {
            get
            {
                return (int)EStateID.Loading;
            }
        }

        internal override void Enter()
        {
            base.Enter();

            _loadingSteps = new List<ALoadingStep>();
            SetupLoadingSteps();
            _currentStepIndex = 0;

            _didFinishLoading = false;
            _exitLoadingState = false;

            _postLoadTimeElapsed = 0f;
            System.GC.Collect();

            DisplayStep();
        }
	
		internal override int Manage ()
		{
            if(CurrentStep != null && CurrentStep.IsComplete)
            {
                NextLoadingStep();
            }

            if (IsLoadingComplete)
            {
                if (_postLoadTimeElapsed < POST_LOADING_DURATION)
                {
                    _postLoadTimeElapsed += Time.deltaTime;
                }
                else if (!_exitLoadingState)
                {
                    _exitLoadingState = true;
                    OnAllLoadingStepsComplete();
                }
            }

            if (Engine.UI.HasLoadingScreen)
            {
                Engine.UI.LoadingScreen.SetProgress(LoadingProgress);
            }
			return base.Manage ();
		}
	
		internal override void Exit ()
		{
			if(Engine.UI.HasLoadingScreen && Engine.UI.LoadingScreenState != UI.FFPanel.EState.Hidden)
				Engine.UI.HideLoading();
            _gameMode.OnLoadingComplete();
			base.Exit ();
		}

        protected bool IsLoadingComplete
        {
            get
            {
                return LoadingProgress == 1f &&
                       _currentStepIndex == (_loadingSteps.Count);
            }
        }
	#endregion
	
	#region Event Management
		protected override void RegisterForEvent ()
		{
			base.RegisterForEvent ();
		}
	
		protected override void UnregisterForEvent ()
		{
			base.UnregisterForEvent ();
		}
        #endregion

        #region Complete Callbacks
        /// <summary>
        /// Called when the state will exit.
        /// </summary>
        private void OnAllLoadingStepsComplete()
        {
            RequestState(outState.ID);
        }

        /// <summary>
        /// Called when the loading progress == 1f. Not necessarily when the loading state will be skipped.
        /// </summary>
        protected virtual void OnLoadingComplete()
        {
            
        }
        #endregion

        #region Progress
        protected float LoadingProgress
        {
            get
            {
                return ProgressForIndex(_currentStepIndex);
            }
        }

        protected float ProgressForIndex(int a_index)
        {
            float currentFactor = 0f;
            for (int i = 0; i < a_index; i++)
            {
                currentFactor += _loadingSteps[i].Factor;
            }

            if (CurrentStep != null)
                currentFactor += CurrentStep.Progress * CurrentStep.Factor;

            return currentFactor / TotalFactor;
        }

        protected float TotalFactor
        {
            get
            {
                float totalFactor = 0f;
                foreach (ALoadingStep each in _loadingSteps)
                {
                    totalFactor += each.Factor;
                }
                return totalFactor;
            }
        }
        #endregion

        #region Steps
        protected virtual void SetupLoadingSteps()
        {
            _loadingSteps.Add(new MainGameLoadingStep());
            _loadingSteps.Add(new AdditionalSceneLoadingStep(this));
            _loadingSteps.Add(new UiLoadingStep(panelsToLoad));
        }

        internal ALoadingStep CurrentStep
        {
            get
            {
                ALoadingStep step = null;
                if(_loadingSteps.Count > _currentStepIndex)
                    step = _loadingSteps[_currentStepIndex];
                return step;
            }
        }

        protected void NextLoadingStep()
        {
            _currentStepIndex++;
            if (CurrentStep != null)
            {
                CurrentStep.Start();
                DisplayStep();
            }

            if (LoadingProgress == 1f && !_didFinishLoading)
            {
                _didFinishLoading = true;
                OnLoadingComplete();
            }
        }

        protected void DisplayStep()
        {
            if (Engine.UI.HasLoadingScreen && CurrentStep != null)
            {
                Engine.UI.LoadingScreen.SetLoadingDescription(CurrentStep.Description);
            }
        }
        #endregion
    }
}