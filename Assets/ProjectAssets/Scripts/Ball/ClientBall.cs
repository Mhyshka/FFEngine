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
        protected GenericMessageReceiver _collisionReceiver;
        protected GenericMessageReceiver _racketHitReceiver;
        protected GenericMessageReceiver _goalHitReceiver;

        internal void NetworkInit()
        {
            _collisionReceiver = new GenericMessageReceiver (OnBallCollisionReceived);
            _racketHitReceiver = new GenericMessageReceiver (OnRacketHitReceived);
            _goalHitReceiver = new GenericMessageReceiver (OnGoalHitReceived);

            Engine.Receiver.RegisterReceiver(EMessageChannel.BallCollision.ToString(), _collisionReceiver);
            Engine.Receiver.RegisterReceiver(EMessageChannel.RacketHit.ToString(), _racketHitReceiver);
            Engine.Receiver.RegisterReceiver(EMessageChannel.GoalHit.ToString(), _goalHitReceiver);
        }

        internal void NetworkTearDown()
        {
            Engine.Receiver.UnregisterReceiver(EMessageChannel.BallCollision.ToString(), _collisionReceiver);
            Engine.Receiver.UnregisterReceiver(EMessageChannel.RacketHit.ToString(), _racketHitReceiver);
            Engine.Receiver.UnregisterReceiver(EMessageChannel.GoalHit.ToString(), _goalHitReceiver);
        }

        protected void OnBallCollisionReceived(ReadMessage a_message)
        {
            MessageBallCollisionData data = a_message.Data as MessageBallCollisionData;
            OnCollision(data.position, data.normal);
        }

        protected void OnRacketHitReceived(ReadMessage a_message)
        {
            MessageIntegerData data = a_message.Data as MessageIntegerData;//RacketID
            RacketMotor motor = _pongGm.Board.RacketForId(data.Data);
            OnRacketHit(motor);
        }

        protected void OnGoalHitReceived(ReadMessage a_message)
        {
            MessageIntegerData data = a_message.Data as MessageIntegerData;//ESide
            ESide side = (ESide)data.Data;
            OnGoal(side);
        }
        #endregion
    }
}