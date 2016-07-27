using UnityEngine;
using System.Collections;
using FF.Network;
using FF.Network.Receiver;
using FF.Network.Message;
using FF.Handler;

namespace FF.Pong
{
    internal class RacketBallHitInfo
    {
        internal int playerId;
        internal long timestamp;
        internal Vector3 position;
        internal Vector3 velocity;

        internal RacketBallHitInfo()
        {
        }
    }

    [System.Serializable]
    internal class BallSettings
    {
        public float baseSpeed = 12f;
        public float smashSpeedMultiplier = 1.5f;
    }

    internal abstract class ABall : MonoBehaviour
    {
        internal static BallSettings Settings = null;
        #region Inspector Properties
        [Header("Components")]
        public BallColorManager color = null;
        public BallHitFx hitFx = null;
        public BallLightManager lightManager = null;
        public Rigidbody ballRigidbody = null;
        public BallCollider ballCollider = null;
        #endregion

        #region Properties
        protected PongGameMode _pongGm;

        protected int _smashCount = 0;
        internal int SmashCount
        {
            get
            {
                return _smashCount;
            }
        }

        protected RacketBallHitInfo _lastNetworkHitInfo = new RacketBallHitInfo();
        protected RacketBallHitInfo _lastLocalHitInfo = new RacketBallHitInfo();
        internal RacketBallHitInfo LastLocalHitInfo
        {
            get
            {
                return _lastLocalHitInfo;
            }
        }
        #region Callbacks
        internal SideCallback onGoal = null;
        internal RacketMotorCallback onRacketHit = null;
        #endregion

        #endregion

        #region Unity Main
        internal abstract void OnTriggerEnter(Collider a_other);
        /*void Awake()
        {
            NetworkInit();
        }

        void OnDestroy()
        {
            NetworkTearDown();
        }*/
        #endregion

        protected virtual void SetVelocity(Vector3 a_velocity)
        {
            transform.parent = null;
            ballRigidbody.velocity = a_velocity;
        }

        #region Interface
        internal void Init(PongGameMode a_gameMode)
        {
            ballCollider.ball = this;
            _pongGm = a_gameMode;
        }

        internal virtual void PostSmash()
        {
            ballRigidbody.velocity *= Settings.smashSpeedMultiplier;
        }

        internal void SnapToLocator(Transform a_locator)
        {
            transform.parent = a_locator;

            Vector3 currentPosition = transform.position;
            transform.localPosition = Vector3.zero;

            currentPosition.x = transform.position.x;
            currentPosition.z = transform.position.z;

            transform.position = currentPosition;

            transform.localRotation = Quaternion.identity;
        }

        #region Launch
        internal void LocalLaunch(float a_ratio, ESide a_serverSide)
        {
            float angle = a_ratio * RacketMotor.Settings.maxBounceAngle;
            float factorX = angle / 90f;
            float factorZ = 1f - Mathf.Abs(factorX);
            if (a_serverSide == ESide.Right)
                factorZ = -factorZ;

            Vector3 result = new Vector3(factorX,
                                        0f,
                                        factorZ);
            result = result.normalized * Settings.baseSpeed;
            SetVelocity(result);

            _lastLocalHitInfo.playerId = Engine.Network.NetPlayer.ID;
            _lastLocalHitInfo.position = transform.position;
            _lastLocalHitInfo.velocity = result;
            _lastLocalHitInfo.timestamp = System.DateTime.Now.Ticks;

            _lastNetworkHitInfo.playerId = Engine.Network.NetPlayer.ID;
            _lastNetworkHitInfo.position = transform.position;
            _lastNetworkHitInfo.velocity = result;
            _lastNetworkHitInfo.timestamp = System.DateTime.Now.Ticks;
        }

        internal void NetworkLaunch(ReadMessage a_message)
        {
            MessageBallMovementData data = a_message.Data as MessageBallMovementData;
            float timeOffset = (float)a_message.Client.Clock.TimeOffset(a_message.Timestamp).TotalSeconds;
            long remoteInLocalTimestamp = a_message.Client.Clock.ConvertRemoteToLocalTime(a_message.Timestamp);

            _lastLocalHitInfo.playerId = data.playerId;
            _lastLocalHitInfo.position = data.position;
            _lastLocalHitInfo.velocity = data.velocity;
            _lastLocalHitInfo.timestamp = remoteInLocalTimestamp;

            UpdateMovementWithNetworkData(remoteInLocalTimestamp, timeOffset, data);
        }
        #endregion

        internal void Stop()
        {
            ballRigidbody.velocity = Vector3.zero;
        }

        internal void SetServiceOffsetX(float a_offset)
        {
            Vector3 localPos = transform.localPosition;
            localPos.x = a_offset;
            transform.localPosition = localPos;
        }

        internal void ResetBall()
        {
            _smashCount = 0;
        }
        #endregion

        #region Events
        internal virtual void OnRacketHit(RacketMotor a_motor)
        {
            if (onRacketHit != null)
                onRacketHit(a_motor);
        }
        #endregion

        #region Network
        protected GenericMessageReceiver _ballNetworkMovementReceiver = null;

        internal virtual void NetworkInit()
        {
            if(_ballNetworkMovementReceiver == null)
                _ballNetworkMovementReceiver = new GenericMessageReceiver(OnBallNetworkMovementReceived);

            Engine.Receiver.RegisterReceiver(EMessageChannel.BallMovement.ToString(), _ballNetworkMovementReceiver);
        }

        internal virtual void NetworkTearDown()
        {
            Engine.Receiver.UnregisterReceiver(EMessageChannel.BallMovement.ToString(), _ballNetworkMovementReceiver);
        }

        internal abstract void ForceNetworkMovementSync();

        protected void OnBallNetworkMovementReceived(ReadMessage a_message)
        {
            MessageBallMovementData data = a_message.Data as MessageBallMovementData;

            //Not local racket event or spectator
            if (Engine.Network.NetPlayer.slot.IsSpectator || Engine.Network.NetPlayer.ID != data.playerId)
            {
                //Debug.LogError("Updating movement");
                float timeOffset = (float)a_message.Client.Clock.TimeOffset(a_message.Timestamp).TotalSeconds;
                long remoteInLocalTimestamp = a_message.Client.Clock.ConvertRemoteToLocalTime(a_message.Timestamp);

                UpdateMovementWithNetworkData(remoteInLocalTimestamp, timeOffset, data);

                RacketMotor motor = _pongGm.Board.RacketForId(data.playerId);
                OnRacketHit(motor);
            }
        }

        internal void UpdateMovementWithNetworkData(long a_remoteInLocalTimestamp, float a_timeOffset, MessageBallMovementData a_movementData)
        {
            SetVelocity(a_movementData.velocity);
            Vector3 targetPos = a_movementData.position + a_timeOffset * a_movementData.velocity;
            ballRigidbody.MovePosition(targetPos);

            _lastLocalHitInfo.playerId = a_movementData.playerId;
            _lastLocalHitInfo.position = a_movementData.position;
            _lastLocalHitInfo.velocity = a_movementData.velocity;
            _lastLocalHitInfo.timestamp = a_remoteInLocalTimestamp;

            _lastNetworkHitInfo.playerId = a_movementData.playerId;
            _lastNetworkHitInfo.position = a_movementData.position;
            _lastNetworkHitInfo.velocity = a_movementData.velocity;
            _lastNetworkHitInfo.timestamp = a_remoteInLocalTimestamp;
        }
        #endregion
    }
}