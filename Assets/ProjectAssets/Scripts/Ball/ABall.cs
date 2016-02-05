using UnityEngine;
using System.Collections;
using FF.Network;

namespace FF.Pong
{
    internal abstract class ABall : MonoBehaviour
    {
        #region Inspector Properties
        [Header("Components")]
        public BallColorManager color = null;
        public BallHitFx hitFx = null;
        public BallLightManager lightManager = null;
        public Rigidbody ballRigidbody = null;
        #endregion

        #region Properties
        internal SideCallback onGoal = null;
        internal RacketMotorCallback onRacketHit = null;

        //Set from game settings
        internal float baseSpeed = 12f;
        internal float smashSpeedMultiplier = 1.5f;

        protected int _smashCount = 0;
        internal int SmashCount
        {
            get
            {
                return _smashCount;
            }
        }
        protected PongGameMode _pongGm;
        #endregion

        internal void Init(PongGameMode a_gameMode)
        {
            _pongGm = a_gameMode;
        }

        internal virtual void SetVelocity(Vector3 a_velocity)
        {
            transform.parent = null;
            ballRigidbody.velocity = a_velocity;
        }

        internal virtual void Smash()
        {
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

        internal void Launch(float a_ratio, float a_maxBounceFactor, ESide a_serverSide)
        {
            float ratio = a_ratio * a_maxBounceFactor;
            float factorZ = (1f - Mathf.Abs(ratio));
            if (a_serverSide == ESide.Right)
                factorZ = -factorZ;

            Vector3 result = new Vector3(ratio,
                                        0f,
                                        factorZ);
            result = result.normalized * baseSpeed;

            SetVelocity(result);
        }

        internal void SetServiceOffsetX(float a_offset)
        {
            Vector3 localPos = transform.localPosition;
            localPos.x = a_offset;
            transform.localPosition = localPos;
        }

        internal virtual void OnGoal(ESide a_side)
        {
            if (onGoal != null)
                onGoal(a_side);
        }

        internal virtual void OnRacketHit(RacketMotor a_motor)
        {
            if (onRacketHit != null)
                onRacketHit(a_motor);
        }

        internal void ResetBall()
        {
            _smashCount = 0;
        }
    }
}