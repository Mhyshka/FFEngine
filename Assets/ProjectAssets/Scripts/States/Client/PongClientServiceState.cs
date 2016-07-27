using UnityEngine;
using System.Collections;

using FF.Logic;
using System;

using FF.Network.Receiver;
using FF.Network.Message;

namespace FF.Pong
{
    internal class PongClientServiceState : PongServiceState
    {
        #region Inspector Properties
        #endregion

        #region State Methods
        internal override void Enter()
        {
            base.Enter();
            _timeOffset = (float)new TimeSpan(_pongGm.ServiceTimestamp).TotalSeconds;
        }
        #endregion
        
        protected override void OnServicePlayerSmash()
        {
            base.OnServicePlayerSmash();
            MessageBallMovementData data = new MessageBallMovementData(Engine.Network.NetPlayer.ID,
                                                                        _pongGm.ball.transform.position,
                                                                        _pongGm.ball.ballRigidbody.velocity);
            SentMessage message = new SentMessage(data,
                                                  EMessageChannel.ServiceLaunch.ToString());
            Engine.Network.MainClient.QueueMessage(message);
        }
    }
}