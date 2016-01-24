using UnityEngine;
using System.Collections;

namespace FF.Pong
{
    internal interface IBallContact
    {
        Vector3 BounceOff(Vector3 a_position, Vector3 a_direction);
        void OnCollision(ServerBall a_ball);
    }
}