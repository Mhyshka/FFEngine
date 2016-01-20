using UnityEngine;
using System.Collections;
using FF.Network;

namespace FF.Pong
{
    internal class ServerBall : MonoBehaviour
    {
        #region Inspector Properties
        public BallColorManager color = null;
        public BallHitFx hitFx = null;
        public BallLightManager lightManager = null;

        public SynchronizedServerBallMovement synchronizedMovement = null;

        public Rigidbody ballRigidbody = null;
        #endregion

        #region Properties
        #endregion

        protected void Update()
        {
        }

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
            }
        }

        internal void SetVelocity(Vector3 a_velocity)
        {
            ballRigidbody.velocity = a_velocity;
            synchronizedMovement.UpdateNow(transform.position, a_velocity);
        }
    }
}