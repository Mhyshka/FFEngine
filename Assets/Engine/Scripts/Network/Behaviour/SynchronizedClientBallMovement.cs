using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using FF.Network.Message;
using FF.Network.Receiver;

namespace FF.Network
{
    internal class SynchronizedClientBallMovement : MonoBehaviour
    {
        #region Inspector Properties
        public Pong.ClientBall ball = null;
        #endregion

        #region Properties
        #endregion

        #region Network
        internal GenericMessageReceiver _receiver;

        void Awake()
        {
            _receiver = new GenericMessageReceiver(OnMessageReceived);
            Engine.Receiver.RegisterReceiver(EMessageChannel.BallMovement.ToString(), _receiver);
        }

        void OnDestroy()
        {
            Engine.Receiver.UnregisterReceiver(EMessageChannel.BallMovement.ToString(), _receiver);
        }

        void OnMessageReceived(ReadMessage a_message)
        {
            MessageBallMovementData data = a_message.Data as MessageBallMovementData;
            ball.RefreshMovement(data.position, data.velocity);
        }
        #endregion
    }
}