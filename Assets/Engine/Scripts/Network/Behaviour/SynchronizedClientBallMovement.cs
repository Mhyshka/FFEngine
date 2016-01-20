using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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
        internal Receiver.GenericReceiver<Message.MessagePongBallMovement> _receiver;

        void Awake()
        {
            _receiver = new Receiver.GenericReceiver<Message.MessagePongBallMovement>(OnMessageReceived);
            Engine.Receiver.RegisterReceiver(Message.EMessageType.PongBallMovement, _receiver);
        }

        void OnDestroy()
        {
            Engine.Receiver.UnregisterReceiver(Message.EMessageType.PongBallMovement, _receiver);
        }

        void OnMessageReceived(Message.MessagePongBallMovement a_message)
        {
            ball.RefreshMovement(a_message.position, a_message.velocity);
        }
        #endregion
    }
}