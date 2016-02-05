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
            Engine.Receiver.RegisterReceiver(EDataType.M_PongBallMovement, _receiver);
        }

        void OnDestroy()
        {
            Engine.Receiver.UnregisterReceiver(EDataType.M_PongBallMovement, _receiver);
        }

        void OnMessageReceived(ReadMessage a_message)
        {
            MessagePongBallMovement data = a_message.Data as MessagePongBallMovement;
            ball.RefreshMovement(data.position, data.velocity);
        }
        #endregion
    }
}