using UnityEngine;
using System.Collections;

using FF.Network.Message;
using FF.Handler;

namespace FF.Network
{
    internal class SynchronizedServerBallMovement : MonoBehaviour
    {
        /*protected void NetworkUpdate(Vector3 a_position, Vector3 a_velocity)
        {
            MessageBallMovementData data = new MessageBallMovementData(a_position, a_velocity);
            SentBroadcastMessage message = new SentBroadcastMessage(Engine.Network.CurrentRoom.GetPlayersIds(),
                                                                    data,
                                                                    EMessageChannel.BallMovement.ToString());
            message.Broadcast();
        }

        internal void UpdateNow(Vector3 a_position, Vector3 a_velocity)
        {
            NetworkUpdate(a_position, a_velocity);
        }*/
    }
}