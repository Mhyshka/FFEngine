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
        internal SideCallback onGoal = null;
        internal RacketMotorCallback onRacketHit = null;
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

                Engine.Network.Server.BroadcastMessage(new FF.Network.Message.MessagePongBallCollision(transform.position, a_other.transform.forward));

                contact.OnCollision(this);
            }
        }

        internal void OnGoal(ESide a_side)
        {
            if (onGoal != null)
                onGoal(a_side);
        }

        internal void OnRacketHit(RacketMotor a_motor)
        {
            if (onRacketHit != null)
                onRacketHit(a_motor);
        }

        internal override void SetVelocity(Vector3 a_velocity)
        {
            base.SetVelocity(a_velocity);
            synchronizedMovement.UpdateNow(transform.position, a_velocity);
        }
    }
}