using UnityEngine;
using System.Collections;

namespace FF.Pong
{
    internal class GoalWall : MonoBehaviour, IBallContact
    {
        public ESide side = ESide.Left;

        public Vector3 BounceOff(Vector3 a_position, Vector3 a_velocity)
        {
            return Vector3.zero;
        }

        public void OnCollision(ServerBall a_ball)
        {
            a_ball.OnGoal(side);
        }
    }
}