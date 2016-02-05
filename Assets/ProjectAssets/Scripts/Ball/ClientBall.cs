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

            Engine.Receiver.RegisterReceiver(EDataType.BallCollision, _collisionReceiver);
            Engine.Receiver.RegisterReceiver(EDataType.RacketHit, _racketHitReceiver);
            Engine.Receiver.RegisterReceiver(EDataType.GoalHit, _goalHitReceiver);
        }

        internal void NetworkTearDown()
        {
            Engine.Receiver.UnregisterReceiver(EDataType.BallCollision, _collisionReceiver);
            Engine.Receiver.UnregisterReceiver(EDataType.RacketHit, _racketHitReceiver);
            Engine.Receiver.UnregisterReceiver(EDataType.GoalHit, _goalHitReceiver);
        }

        protected void OnBallCollisionReceived(ReadMessage a_message)
        {
            MessageBallCollisionData data = a_message.Data as MessageBallCollisionData;
            OnCollision(data.position, data.normal);
        }

        protected void OnRacketHitReceived(ReadMessage a_message)
        {
            MessageRacketHitData data = a_message.Data as MessageRacketHitData;
            RacketMotor motor = _pongGm.Board.RacketForId(data.racketId);
            OnRacketHit(motor);
        }

        protected void OnGoalHitReceived(ReadMessage a_message)
        {
            MessageGoalHit data = a_message.Data as MessageGoalHit;
            OnGoal(data.side);
        }
        #endregion
    }
}