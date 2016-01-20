using UnityEngine;
using System.Collections;
using System;

namespace FF.Pong
{
    internal class RacketNetworkController : ARacketComponent
    {
        #region Inspector Properties
        #endregion

        #region Properties
        protected Network.Receiver.GenericReceiver<Network.Message.MessagePongTargetRatio> _targetRatioReceiver = null;
        #endregion

        #region Init & Destroy
        internal override void Activate()
        {
            _targetRatioReceiver = new Network.Receiver.GenericReceiver<Network.Message.MessagePongTargetRatio>(OnTargetRatioReceived);
            Engine.Receiver.RegisterReceiver(Network.Message.EMessageType.PongTargetRatio, _targetRatioReceiver);
        }

        internal override void TearDown()
        {
            Engine.Receiver.UnregisterReceiver(Network.Message.EMessageType.PongTargetRatio, _targetRatioReceiver);
        }

        protected void OnTargetRatioReceived(Network.Message.MessagePongTargetRatio a_message)
        {
            if (a_message.clientId == motor.clientId)
            {
                motor.TargetRatio = a_message.ratio;
            }
        }
        #endregion
    }
}