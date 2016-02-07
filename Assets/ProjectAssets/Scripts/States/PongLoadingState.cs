using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using FF.Handler;
using FF.Network;
using FF.Network.Message;
using FF.Multiplayer;
using FF.Logic;

namespace FF.Pong
{
    internal abstract class PongLoadingState : AMultiLoadingState
    {
        #region Properties
        protected PongGameMode _pgm;
        #endregion

        #region State Methods
        internal override void Enter()
        {
            base.Enter();
            _pgm = _gameMode as PongGameMode;
        }
        internal override int Manage()
        {
            if (CurrentStep != null && CurrentStep == _unlockStep)
            {
                if (DidReachTarget)
                    OnRacketTargetReached();
            }
            return base.Manage();
        }
        #endregion

        #region Racket Position
        protected virtual bool DidReachTarget
        {
            get
            {
                return _pgm.LocalRacket.TargetRatio > 0.9f;
            }
        }

        protected override void SetupLoadingSteps()
        {
            base.SetupLoadingSteps();
            _unlockStep.onStart += OnUnlockStepStart;
        }

        void OnUnlockStepStart()
        {
            _loadingScreen.SetTip("Take your racket to the bottom of the screen.");
            _unlockStep.onStart -= OnUnlockStepStart;
            PongGameMode pgm = _gameMode as PongGameMode;
            pgm.OnLoadingComplete();
        }

        protected virtual void OnRacketTargetReached()
        {
            _unlockStep.SetComplete();
        }
        #endregion
    }
}