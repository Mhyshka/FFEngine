using UnityEngine;
using System.Collections;

using FF.Logic;
using System;

using FF.Network.Receiver;
using FF.Network.Message;

namespace FF.Pong
{
    internal class PongServerServiceState : PongServiceState
    {
        #region Inspector Properties
        #endregion

        #region properties
        protected GenericMessageReceiver _serviceRatioReceiver = null;
        #endregion

        #region State Methods
        internal override void Enter()
        {
            base.Enter();
            _serviceRatioReceiver = new GenericMessageReceiver(OnServiceRatioReceived);
        }
        #endregion

        #region EventManagement
        protected override void RegisterForEvent()
        {
            base.RegisterForEvent();
            Engine.Receiver.RegisterReceiver(EDataType.Empty, _serviceRatioReceiver);
        }

        protected override void UnregisterForEvent()
        {
            base.UnregisterForEvent();
            Engine.Receiver.UnregisterReceiver(EDataType.Empty, _serviceRatioReceiver);
        }
        #endregion

        protected void OnServiceRatioReceived(ReadMessage a_message)
        {
            MessageFloatData data = a_message.Data as MessageFloatData;
            LaunchBall(data.Data);
            RequestState((int)EPongGameState.Gameplay);
        }

        protected override void OnServicePlayerSmash()
        {
            LaunchBall(_ratio);
            RequestState((int)EPongGameState.Gameplay);
        }

        protected void LaunchBall(float a_ratio)
        {
            float signedRatio = a_ratio * 2f - 1f;

            _pongGm.ball.Launch(signedRatio,
                                ServiceRacket.maxBounceFactorX,
                                ServiceRacket.side);
        }
    }
}