using UnityEngine;
using System.Collections;

using FF.Network.Receiver;
using FF.Network.Message;

namespace FF.Pong
{
    internal class ClientBall : ABall
    {
        #region Inspector Properties
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
            ballRigidbody.MovePosition(a_position);
            SetVelocity(a_velocity);
        }

        #region Network
        protected GenericMessageReceiver<MessagePongBallCollision, MessageHeader> _collisionReceiver;
        protected GenericMessageReceiver<MessageRacketHit, MessageHeader> _racketHitReceiver;
        protected GenericMessageReceiver<MessageGoalHit, MessageHeader> _goalHitReceiver;

        internal void NetworkInit()
        {
            _collisionReceiver = new GenericMessageReceiver<MessagePongBallCollision, MessageHeader> (OnBallCollisionReceived);
            _racketHitReceiver = new GenericMessageReceiver<MessageRacketHit, MessageHeader> (OnRacketHitReceived);
            _goalHitReceiver = new GenericMessageReceiver<MessageGoalHit, MessageHeader> (OnGoalHitReceived);

            Engine.Receiver.RegisterReceiver(EDataType.M_PongBallCollision, _collisionReceiver);
            Engine.Receiver.RegisterReceiver(EDataType.M_RacketHit, _racketHitReceiver);
            Engine.Receiver.RegisterReceiver(EDataType.M_GoalHit, _goalHitReceiver);
        }

        internal void NetworkTearDown()
        {
            Engine.Receiver.UnregisterReceiver(EDataType.M_PongBallCollision, _collisionReceiver);
            Engine.Receiver.UnregisterReceiver(EDataType.M_RacketHit, _racketHitReceiver);
            Engine.Receiver.UnregisterReceiver(EDataType.M_GoalHit, _goalHitReceiver);
        }

        protected void OnBallCollisionReceived(MessageHeader a_header, MessagePongBallCollision a_message)
        {
            OnCollision(a_message.position, a_message.normal);
        }

        protected void OnRacketHitReceived(MessageHeader a_header, MessageRacketHit a_message)
        {
            RacketMotor motor = _pongGm.Board.RacketForId(a_message.racketId);
            OnRacketHit(motor);
        }

        protected void OnGoalHitReceived(MessageHeader a_header, MessageGoalHit a_message)
        {
            OnGoal(a_message.side);
        }
        #endregion
    }
}