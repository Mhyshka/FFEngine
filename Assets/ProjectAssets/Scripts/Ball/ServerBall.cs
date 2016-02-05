using UnityEngine;
using System.Collections;
using FF.Network;

namespace FF.Pong
{
    internal class ServerBall : ABall
    {
        #region Inspector Properties
        public SynchronizedServerBallMovement synchronizedMovement = null;
        #endregion

        #region Properties

        #endregion

        protected void OnTriggerEnter(Collider a_other)
        {
            IBallContact contact = a_other.GetComponent<IBallContact>();
            if (contact != null)
            {
                color.RandomizeNewColor();
                hitFx.Play(color.CurrentColor,
                            transform.position,
                            a_other.transform.forward);
                lightManager.OnHit();

                Vector3 velocity = contact.BounceOff(transform.position, ballRigidbody.velocity);
                SetVelocity(velocity);

                Engine.Network.Server.BroadcastMessage(new FF.Network.Message.MessageBallCollisionData(transform.position, a_other.transform.forward));

                contact.OnCollision(this);
            }
        }


        internal override void SetVelocity(Vector3 a_velocity)
        {
            base.SetVelocity(a_velocity);
            synchronizedMovement.UpdateNow(transform.position, a_velocity);
        }

        internal override void OnRacketHit(RacketMotor a_motor)
        {
            base.OnRacketHit(a_motor);
            Network.Message.MessageRacketHitData message = new Network.Message.MessageRacketHitData(a_motor.clientId);
            Engine.Network.Server.BroadcastMessage(message);
        }

        internal override void OnGoal(ESide a_side)
        {
            base.OnGoal(a_side);
            Network.Message.MessageGoalHit message = new Network.Message.MessageGoalHit(a_side);
            Engine.Network.Server.BroadcastMessage(message);
        }

        internal override void Smash()
        {
            base.Smash();

            _smashCount++;
            SetVelocity(ballRigidbody.velocity * (1 + _smashCount * smashSpeedMultiplier));
            Network.Message.MessageDidSmash message = new Network.Message.MessageDidSmash(_smashCount);
            Engine.Network.Server.BroadcastMessage(message);
        }
    }
}