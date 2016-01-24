using UnityEngine;
using System.Collections;

using FF.Logic;
using System;

namespace FF.Pong
{
    internal class PongServiceState : APongServerState
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

            _ratio = 0f;
            _serviceRacket = _pongServerGm.RacketForPlayer(_pongServerGm.serviceClientId);

            SnapBallToRacket();
            ServiceRacket.onSmash += OnServicePlayerSmash;
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
            ServiceRacket.onSmash -= OnServicePlayerSmash;
        }
        #endregion

        protected void OnServicePlayerSmash()
        {
            LaunchBall();
            RequestState((int)EPongGameState.Gameplay);
        }

        protected void SnapBallToRacket()
        {
            _pongServerGm.ball.SnapToLocator(ServiceRacket.ballSnapLocator);
        }

        protected void LaunchBall()
        {
            bool isLeftSide = Engine.Game.CurrentRoom.GetPlayerForId(_serviceRacket.clientId).slot.team.teamIndex == GameConstants.BLUE_TEAM_INDEX;
            float signedRatio = _ratio * 2f - 1f;

            _pongServerGm.CurrentRound.strikerId = _serviceRacket.clientId;

            _pongServerGm.ball.Launch(signedRatio,
                                        ServiceRacket.maxBounceFactorX,
                                        isLeftSide);
        }

        protected void UpdateBallPosition()
        {
            float offset = _ratio * ServiceRacket.maxOffsetX * 2f - ServiceRacket.maxOffsetX;
            _pongServerGm.ball.SetServiceOffsetX(offset);
        }
    }
}