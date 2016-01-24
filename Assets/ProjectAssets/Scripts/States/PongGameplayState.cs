using UnityEngine;
using System.Collections;

using FF.Logic;
using System;

namespace FF.Pong
{
    internal class PongGameplayState : APongServerState
    {
        #region Inspector Properties
        #endregion

        #region properties
        #endregion

        internal override int ID
        {
            get
            {
                return (int)EPongGameState.Gameplay;
            }
        }

        #region State Methods
        internal override void Enter()
        {
            base.Enter();
        }

        internal override int Manage()
        {
            return base.Manage();
        }

        internal override void Exit()
        {
            base.Exit();
        }
        #endregion

        #region Events
        protected override void RegisterForEvent()
        {
            base.RegisterForEvent();
            _pongServerGm.ServerBall.onGoal += OnGoal;
            _pongServerGm.ServerBall.onRacketHit += OnRacketHit;

            _pongServerGm.Board.blueRacket.onSmash += OnSmash;
            _pongServerGm.Board.purpleRacket.onSmash += OnSmash;
        }

        protected override void UnregisterForEvent()
        {
            base.UnregisterForEvent();
            _pongServerGm.ServerBall.onGoal -= OnGoal;
            _pongServerGm.ServerBall.onRacketHit += OnRacketHit;

            _pongServerGm.Board.blueRacket.onSmash -= OnSmash;
            _pongServerGm.Board.purpleRacket.onSmash -= OnSmash;
        }

        protected virtual void OnGoal(ESide a_side)
        {
            _pongServerGm.CurrentRound.goalSide = a_side;
            if (a_side == ESide.Left)
                _pongServerGm.Board.blueLifeLights.TakeRandomLife();
            else if (a_side == ESide.Right)
                _pongServerGm.Board.purpleLifeLights.TakeRandomLife();

            RequestState((int)EPongGameState.Score);
        }

        protected virtual void OnRacketHit(RacketMotor a_racket)
        {
            _pongServerGm.CurrentRound.rallyCount++;
            _pongServerGm.CurrentRound.strikerId = a_racket.clientId;
        }

        protected virtual void OnSmash()
        {
            _pongServerGm.CurrentRound.isSmash = true;
            _pongServerGm.CurrentRound.smashCount++;

            _pongServerGm.ball.Smash();
        }
        #endregion
    }
}