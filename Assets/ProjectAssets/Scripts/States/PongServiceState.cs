using UnityEngine;
using System.Collections;

using FF.Logic;
using System;
using FF.Network.Message;
using FF.Network.Receiver;

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
        protected float _timeOffset = 0f;


        protected GenericMessageReceiver _serviceLaunchReceiver = null;
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
            _serviceRacket = _pongGm.Board.RacketForId(_pongGm.serverPlayerId);
            _pongGm.CurrentRound.strikerId = ServiceRacket.PlayerId;
            _ratio = 0f;

            if (_serviceLaunchReceiver == null)
                _serviceLaunchReceiver = new GenericMessageReceiver(OnServiceLaunchReceived);
        }

        internal override int Manage()
        {
            int toReturn = base.Manage();
            if (toReturn == ID)
            {
                _ratio = ratioPositionCurve.Evaluate((_timeElapsedSinceEnter + _timeOffset) % ratioLoopTime);
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
            Engine.Receiver.RegisterReceiver(EMessageChannel.ServiceLaunch.ToString(), _serviceLaunchReceiver);
        }

        protected override void UnregisterForEvent()
        {
            base.UnregisterForEvent();
            if (ServiceRacket == _pongGm.LocalRacket)
            {
                ServiceRacket.onTrySmash -= OnServicePlayerSmash;
            }
            Engine.Receiver.UnregisterReceiver(EMessageChannel.ServiceLaunch.ToString(), _serviceLaunchReceiver);
        }

        protected virtual void OnServiceLaunchReceived(ReadMessage a_message)
        {
            _pongGm.ball.NetworkLaunch(a_message);
            RequestState((int)EPongGameState.Gameplay);
        }

        #endregion

        protected void UpdateBallPosition()
        {
            float offset = 0f;
            if (ServiceRacket.side == ESide.Left)
            {
                offset = _ratio * RacketMotor.Settings.serviceMaxOffsetX * 2f - RacketMotor.Settings.serviceMaxOffsetX;
            }
            else if (ServiceRacket.side == ESide.Right)
            {
                offset = (1 - _ratio) * RacketMotor.Settings.serviceMaxOffsetX * 2f - RacketMotor.Settings.serviceMaxOffsetX;
            }
            _pongGm.ball.SetServiceOffsetX(offset);
        }


        protected virtual void OnServicePlayerSmash()
        {
            float signedRatio = _ratio * 2f - 1f;

            _pongGm.ball.LocalLaunch(signedRatio,
                                    ServiceRacket.side);
            RequestState((int)EPongGameState.Gameplay);
        }
    }
}