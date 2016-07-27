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

        protected long hitTimestamp = 0L;

        public Vector3 BounceOff(Vector3 a_position, Vector3 a_velocity)
        {
            float offsetX = a_position.x - transform.position.x;
            float ratio = offsetX / (collider.bounds.size.x/2f);
            float angle = ratio * RacketMotor.Settings.maxBounceAngle;

            float factorX = angle / 90f;

            

            Vector3 result = new Vector3(factorX,
                                        0f,
                                        (motor.side == ESide.Right ? -1f : 1f) * (1f - Mathf.Abs(factorX)));

            float smashMultiplier = 1f;
            if (motor.smashTiming.CanPreSmash)
            {
                smashMultiplier = ABall.Settings.smashSpeedMultiplier;
            }
            result = result.normalized * a_velocity.magnitude * smashMultiplier;

            hitTimestamp = DateTime.Now.Ticks;

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