using UnityEngine;
using System.Collections;

using FF.Logic;
using System;

namespace FF.Pong
{
    internal abstract class PongServiceState : APongState
    {
        #region Inspector Properties
        public AnimationCurve ratioPositionCurve = null;
        public float ratioLoopTime = 1f;
        #endregion

        #region properties
        protected RacketMotor _serviceRacket;
        protected float _ratio = 0f;
        #endregion

        internal override int ID
        {
            get
            {
                return (int)EPongGameState.Service;
            }
        }

        protected RacketMotor ServiceRacket
        {
            get
            {
                return _serviceRacket;
            }
        }

        #region State Methods
        internal override void Enter()
        {
            base.Enter();
            _serviceRacket = _pongGm.Board.RacketForId(_pongGm.serviceClientId);
            _pongGm.CurrentRound.strikerId = ServiceRacket.clientId;
            _ratio = 0f;
        }

        internal override int Manage()
        {
            int toReturn = base.Manage();
            if (toReturn == ID)
            {
                _ratio = ratioPositionCurve.Evaluate(_timeElapsedSinceEnter % ratioLoopTime);
                UpdateBallPosition();
            }
            return toReturn;
        }

        internal override void Exit()
        {
            base.Exit();
        }
        #endregion

        #region EventManagement
        protected override void RegisterForEvent()
        {
            base.RegisterForEvent();
            if (ServiceRacket == _pongGm.LocalRacket)
            {
                ServiceRacket.onTrySmash += OnServicePlayerSmash;
            }
        }

        protected override void UnregisterForEvent()
        {
            base.UnregisterForEvent();
            if (ServiceRacket == _pongGm.LocalRacket)
            {
                ServiceRacket.onTrySmash -= OnServicePlayerSmash;
            }
        }
        #endregion

        protected abstract void OnServicePlayerSmash();

        protected void UpdateBallPosition()
        {
            float offset = 0f;
            if (ServiceRacket.side == ESide.Left)
            {
                offset = _ratio * ServiceRacket.maxOffsetX * 2f - ServiceRacket.maxOffsetX;
            }
            else if (ServiceRacket.side == ESide.Right)
            {
                offset = (1 - _ratio) * ServiceRacket.maxOffsetX * 2f - ServiceRacket.maxOffsetX;
            }
            _pongGm.ball.SetServiceOffsetX(offset);
        }
    }
}