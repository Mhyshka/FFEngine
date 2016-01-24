using UnityEngine;
using System.Collections;

namespace FF.Pong
{
    internal class Wall : MonoBehaviour, IBallContact
    {
        public Vector3 BounceOff(Vector3 a_position, Vector3 a_velocity)
        {
            Vector3 result = Vector3.Reflect(a_velocity, transform.forward);

            return result;
        }

        public void OnCollision(ServerBall a_ball)
        {
        }
    }
}