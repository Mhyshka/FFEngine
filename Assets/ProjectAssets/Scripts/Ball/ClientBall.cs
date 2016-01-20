using UnityEngine;
using System.Collections;

namespace FF.Pong
{
    internal class ClientBall : MonoBehaviour
    {
        #region Inspector Properties
        public BallColorManager color = null;
        public BallHitFx hitFx = null;
        public BallLightManager lightManager = null;

        public new Rigidbody rigidbody = null;
        #endregion

        #region Properties
        #endregion

        #region Unity Main
        void Awake()
        {
            NetworkInit();
        }

        void OnDestroy()
        {
            NetworkTearDown();
        }

        void Update()
        {
        }
        #endregion

        internal void OnCollision(Vector3 a_position, Vector3 a_normal)
        {
            color.RandomizeNewColor();
            hitFx.Play(color.CurrentColor,
                a_position,
                a_normal);
            lightManager.OnHit();
        }

        internal void RefreshMovement(Vector3 a_position, Vector3 a_velocity)
        {
            rigidbody.MovePosition(a_position);
            rigidbody.velocity = a_velocity;
        }

        #region Network
        protected Network.Receiver.GenericReceiver<Network.Message.MessagePongBallCollision> _collisionReceiver;
        internal void NetworkInit()
        {
            _collisionReceiver = new Network.Receiver.GenericReceiver<Network.Message.MessagePongBallCollision>(OnBallCollisionReceived);
            Engine.Receiver.RegisterReceiver(Network.Message.EMessageType.PongBallCollision, _collisionReceiver);
        }

        internal void NetworkTearDown()
        {
            Engine.Receiver.UnregisterReceiver(Network.Message.EMessageType.PongBallCollision, _collisionReceiver);
        }

        protected void OnBallCollisionReceived(Network.Message.MessagePongBallCollision a_message)
        {
            OnCollision(a_message.position, a_message.normal);
        }
        #endregion
    }
}