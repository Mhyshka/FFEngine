using UnityEngine;
using System.Collections;

namespace FF
{
	internal class FFRandomizeLightIntensity : MonoBehaviour
	{
        #region Inspector Properties
        public Light targetLight = null;
        public float lerpDuration = 1f;

        public float minIntensity = 0f;
        public float maxIntensity = 1f;
        #endregion

        #region Properties
        protected float _timeElapsed = 0f;
        protected float _previousIntensity = 0f;
        protected float _targetIntensity = 1f;
        #endregion

        protected float Factor
        {
            get
            {
                return Mathf.Clamp01(_timeElapsed / lerpDuration);
            }
        }

        void Awake()
        {
            if (targetLight == null)
                targetLight = GetComponent<Light>();
            _previousIntensity = minIntensity;
            RandomizeNewIntensity();
        }

        void Update()
        {
            _timeElapsed += Time.deltaTime;
            targetLight.intensity = Mathf.Lerp(_previousIntensity, _targetIntensity, Factor);
            if (_timeElapsed > lerpDuration)
            {
                RandomizeNewIntensity();
                _timeElapsed = 0f;
            }
        }

        void RandomizeNewIntensity()
        {
            _previousIntensity = _targetIntensity;
            _targetIntensity = Random.Range(minIntensity, maxIntensity);
        }
    }
}