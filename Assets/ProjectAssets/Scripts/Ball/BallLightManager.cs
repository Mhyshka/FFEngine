using UnityEngine;
using System.Collections;

namespace FF.Pong
{
    internal class BallLightManager : MonoBehaviour
    {
        #region Inspector Properties
        public Light targetLight = null;

        public AnimationCurve onHitCurve = null;
        public float hitRangeFactor = 10f;
        public float hitIntensityFactor = 3f;
        public float hitDuration = 0.3f;

        public AnimationCurve pulseCurve = null;
        public float pulseIntensityFactor = 1f;
        public float pulseDuration = 5f;
        #endregion

        #region Properties
        protected float _baseRange = 0f;
        protected float _baseIntensity = 0f;

        protected float _timeElasped = 0f;
        protected float PulseFactor
        {
            get
            {
                return Mathf.Clamp01(_timeElasped / pulseDuration);
            }
        }

        protected float _timeElapsedSinceHit = 0f;
        protected float HitFactor
        {
            get
            {
                return Mathf.Clamp01(_timeElapsedSinceHit / hitDuration);
            }
        }

        protected float Intensity
        {
            get
            {
                return _baseIntensity +
                    onHitCurve.Evaluate(HitFactor) * hitIntensityFactor +
                    pulseCurve.Evaluate(PulseFactor) * pulseIntensityFactor;
            }
        }

        protected float Range
        {
            get
            {
                return onHitCurve.Evaluate(HitFactor) * hitRangeFactor + _baseRange;
            }
        }
        #endregion

        void Awake()
        {
            _baseRange = targetLight.range;
            _baseIntensity = targetLight.intensity;
            _timeElapsedSinceHit = hitDuration;
        }

        // Update is called once per frame
        void Update()
        {
            _timeElasped += Time.deltaTime;
            _timeElasped %= pulseDuration;

            _timeElapsedSinceHit += Time.deltaTime;

            targetLight.intensity = Intensity;
            targetLight.range = Range;
        }

        internal void OnHit()
        {
            _timeElapsedSinceHit = 0f;
        }
    }
}