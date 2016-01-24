using UnityEngine;
using System.Collections;
using System;

namespace FF.Pong
{
    internal class RacketCollider : ARacketComponent, IBallContact
    {
        #region inspector Properties
        public new BoxCollider collider = null;
        #endregion

        public Vector3 BounceOff(Vector3 a_position, Vector3 a_velocity)
        {
            float offsetX = a_position.x - transform.position.x;
            float ratio = offsetX / (collider.bounds.size.x/2f);
            ratio *= motor.maxBounceFactorX;

            Vector3 result = new Vector3(ratio,
                                        0f,
                                        Mathf.Sign(-a_velocity.z) * (1f - Mathf.Abs(ratio)));
            result = result.normalized * a_velocity.magnitude;

            return result;
        }

        public void OnCollision(ServerBall a_ball)
        {
            a_ball.OnRacketHit(motor);
        }

        internal override void Activate()
        {
        }

        internal override void TearDown()
        {
        }
    }
}