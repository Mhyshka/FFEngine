using UnityEngine;
using System.Collections;
using System;

namespace FF.Pong
{
    internal class RacketSmashTiming : ARacketComponent
    {
        #region Inspector Properties
        public float preTimer = 0.075f;
        public float postTimer = 0.05f;
        public float cooldown = 0.15f;
        #endregion

        #region Properties
        protected float _timeElasped = 0f;
        protected bool _isTriggered = false;
        protected long _hitTimestamp = 0L;

        internal bool CanPreSmash
        {
            get
            {
                return _isTriggered && _timeElasped < preTimer;
            }
        }

        internal bool CanPostSmash
        {
            get
            {
                TimeSpan span = TimeSpan.FromTicks(DateTime.Now.Ticks - _hitTimestamp);
                return _isTriggered && span.TotalMilliseconds < postTimer;
            }
        }
        #endregion

        internal override void Activate()
        {
            OnSmashReady();
            enabled = true;
        }

        internal override void TearDown()
        {
            enabled = false;
        }

        void Update()
        {
            if (_isTriggered && _timeElasped < cooldown)
            {
                _timeElasped += Time.deltaTime;

                if (_timeElasped >= cooldown)
                {
                    OnSmashReady();
                }
            }
        }

        void OnSmashReady()
        {
            _timeElasped = 0f;
            _isTriggered = false;
        }

        internal bool TrySmash()
        {
            if (!_isTriggered)
            {
                _isTriggered = true;
                _timeElasped = 0f;

                if (CanPostSmash)
                {
                    motor.ball.PostSmash();
                }
                return true;
            }
            else
            {
                
                return false;
            }
        }

        internal void OnBallHit()
        {
            _hitTimestamp = DateTime.Now.Ticks;
        }
    }
}