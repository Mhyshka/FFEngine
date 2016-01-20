using UnityEngine;
using System.Collections;

namespace FF.Network
{
    internal class SynchronizedServerBallPosition : MonoBehaviour
    {
        public int messagePerSeconde = 10;

        protected float _timeElapsed = 0f;

        float TimeBetweenMessage
        {
            get
            {
                if (messagePerSeconde == 0)
                {
                    return 1f;
                }
                else
                {
                    return 1f / messagePerSeconde;
                }
            }
        }

        void FixedUpdate()
        {
            _timeElapsed += Time.fixedDeltaTime;
            if (_timeElapsed >= TimeBetweenMessage)
            {
                _timeElapsed %= TimeBetweenMessage;
                NetworkUpdate();
            }
        }

        void NetworkUpdate()
        {
            Message.MessagePongBallPosition position = new Message.MessagePongBallPosition(transform.position);
            Engine.Network.Server.BroadcastMessage(position);
        }

        internal void UpdateNow()
        {
            NetworkUpdate();
        }
    }
}