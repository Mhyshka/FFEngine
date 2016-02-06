using UnityEngine;
using System.Collections;
using FF.Network;

using FF.Network.Message;

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

                SentMessage message = new SentMessage(new MessageBallCollisionData(transform.position, a_other.transform.forward),
                                                        EMessageChannel.BallCollision.ToString(),
                                                        false);
                Engine.Network.Server.BroadcastMessage(message);

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
            MessageIntegerData data = new MessageIntegerData(a_motor.clientId);
            SentMessage message = new SentMessage(data,
                                                  EMessageChannel.RacketHit.ToString(),
                                                  false);
            Engine.Network.Server.BroadcastMessage(message);
        }

        internal override void OnGoal(ESide a_side)
        {
            base.OnGoal(a_side);
            MessageIntegerData data = new MessageIntegerData((int)a_side);
            SentMessage message = new SentMessage(data,
                                                  EMessageChannel.GoalHit.ToString(),
                                                  false);
            Engine.Network.Server.BroadcastMessage(message);
        }

        internal override void Smash()
        {
            base.Smash();

            _smashCount++;
            SetVelocity(ballRigidbody.velocity * (1 + _smashCount * smashSpeedMultiplier));

            MessageIntegerData data = new MessageIntegerData(_smashCount);
            SentMessage message = new SentMessage(data,
                                                  EMessageChannel.Smash.ToString(),
                                                  false);
            Engine.Network.Server.BroadcastMessage(message);
        }
    }
}