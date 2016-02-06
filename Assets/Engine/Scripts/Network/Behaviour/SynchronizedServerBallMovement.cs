using UnityEngine;
using System.Collections;

using FF.Network.Message;

namespace FF.Network
{
    internal class SynchronizedServerBallMovement : MonoBehaviour
    {
        protected void NetworkUpdate(Vector3 a_position, Vector3 a_velocity)
        {
            MessageBallMovementData data = new MessageBallMovementData(a_position, a_velocity);
            SentMessage message = new SentMessage(data,
                                                    EMessageChannel.BallMovement.ToString());
            Engine.Network.Server.BroadcastMessage(message);
        }

        internal void UpdateNow(Vector3 a_position, Vector3 a_velocity)
        {
            NetworkUpdate(a_position, a_velocity);
        }
    }
}