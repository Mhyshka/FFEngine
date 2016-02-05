using UnityEngine;
using System.Collections;
using System;

using FF.Network.Receiver;
using FF.Network.Message;

namespace FF.Pong
{
    internal class RacketNetworkController : ARacketComponent
    {
        #region Inspector Properties
        #endregion

        #region Properties
        protected GenericMessageReceiver<MessagePongTargetRatio, MessageHeader> _targetRatioReceiver = null;
        protected GenericMessageReceiver<MessageTrySmash, MessageHeader> _trySmashReceiver = null;
        #endregion

        #region Init & Destroy
        internal override void Activate()
        {
            if(_targetRatioReceiver == null)
                _targetRatioReceiver = new GenericMessageReceiver<MessagePongTargetRatio, MessageHeader>(OnTargetRatioReceived);
            if(_trySmashReceiver == null)
                _trySmashReceiver = new GenericMessageReceiver<MessageTrySmash, MessageHeader>(OnTrySmashReceived);

            Engine.Receiver.RegisterReceiver(EDataType.M_TrySmash, _trySmashReceiver);
            Engine.Receiver.RegisterReceiver(EDataType.M_PongTargetRatio, _targetRatioReceiver);
        }

        internal override void TearDown()
        {
            Engine.Receiver.UnregisterReceiver(EDataType.M_TrySmash, _trySmashReceiver);
            Engine.Receiver.UnregisterReceiver(EDataType.M_PongTargetRatio, _targetRatioReceiver);
        }

        protected void OnTargetRatioReceived(MessageHeader a_header, MessagePongTargetRatio a_message)
        {
            if (a_message.clientId == motor.clientId)
            {
                motor.TargetRatio = a_message.ratio;
            }
        }

        protected void OnTrySmashReceived(MessageHeader a_header, MessageTrySmash a_message)
        {
            if (a_message.racketId == motor.clientId)
            {
                motor.TrySmash();
            }
        }
        #endregion
    }
}