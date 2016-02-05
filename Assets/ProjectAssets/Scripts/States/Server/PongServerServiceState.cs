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
        protected GenericMessageReceiver<MessageServiceRatio, MessageHeader> _serviceRatioReceiver = null;
        #endregion

        #region State Methods
        internal override void Enter()
        {
            base.Enter();
            _serviceRatioReceiver = new GenericMessageReceiver<MessageServiceRatio, MessageHeader>(OnServiceRatioReceived);
        }
        #endregion

        #region EventManagement
        protected override void RegisterForEvent()
        {
            base.RegisterForEvent();
            Engine.Receiver.RegisterReceiver(EDataType.M_ServiceRatio, _serviceRatioReceiver);
        }

        protected override void UnregisterForEvent()
        {
            base.UnregisterForEvent();
            Engine.Receiver.UnregisterReceiver(EDataType.M_ServiceRatio, _serviceRatioReceiver);
        }
        #endregion

        protected void OnServiceRatioReceived(MessageHeader a_header, MessageServiceRatio a_message)
        {
            LaunchBall(a_message.ratio);
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