using UnityEngine;
using System.Collections;

using FF.Logic;
using System;

namespace FF.Pong
{
    internal class PongGameplayState : APongState
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
            _pongGm.ball.Stop();
        }
        #endregion

        #region Events
        protected override void RegisterForEvent()
        {
            base.RegisterForEvent();
            _pongGm.ball.NetworkInit();
            _pongGm.ball.onGoal += OnGoal;
            _pongGm.ball.onRacketHit += OnRacketHit;

            /*_pongGm.Board.blueRacket.onSmash += OnSmash;
            _pongGm.Board.purpleRacket.onSmash += OnSmash;*/
        }

        protected override void UnregisterForEvent()
        {
            base.UnregisterForEvent();
            _pongGm.ball.NetworkTearDown();
            _pongGm.ball.onGoal -= OnGoal;
            _pongGm.ball.onRacketHit -= OnRacketHit;

            /*_pongGm.Board.blueRacket.onSmash -= OnSmash;
            _pongGm.Board.purpleRacket.onSmash -= OnSmash;*/
        }

        protected virtual void OnGoal(ESide a_side)
        {
            _pongGm.CurrentRound.goalSide = a_side;
            if (a_side == ESide.Left)
                _pongGm.Board.blueLifeLights.TakeRandomLife();
            else if (a_side == ESide.Right)
                _pongGm.Board.purpleLifeLights.TakeRandomLife();

            RequestState((int)EPongGameState.Score);
        }

        protected virtual void OnRacketHit(RacketMotor a_racket)
        {
            _pongGm.CurrentRound.rallyCount++;
            _pongGm.CurrentRound.strikerId = a_racket.PlayerId;
        }

        protected virtual void OnSmash()
        {
            _pongGm.CurrentRound.isSmash = true;
            _pongGm.CurrentRound.smashCount++;

            _pongGm.ball.PostSmash();
        }
        #endregion
    }
}