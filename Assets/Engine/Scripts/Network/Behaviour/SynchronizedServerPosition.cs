using UnityEngine;
using System.Collections;

namespace FF.Network
{
    internal class SynchronizedServerPosition : MonoBehaviour
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
            NetworkPositionEvent posEvent = new NetworkPositionEvent(Time.time, transform.position, Vector3.zero);
            Message.MessagePositionEvent position = new Message.MessagePositionEvent(posEvent);
            Engine.Network.Server.BroadcastMessage(position);
        }

        internal void UpdateNow()
        {
            NetworkUpdate();
        }
    }
}