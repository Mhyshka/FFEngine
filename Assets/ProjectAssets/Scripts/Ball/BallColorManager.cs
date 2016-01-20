using UnityEngine;
using System.Collections;

namespace FF
{
	internal class BallColorManager : MonoBehaviour
	{
        #region Inspector Properties
        public Material targetMaterial = null;
        public Light targetLight = null;
        public TrailRenderer trail = null;

        public float minColorDistance = 0.3f;

        public float lerpDuration = 0.1f;
        #endregion

        #region Properties
        protected float _timeElapsed = 0f;
        protected Color _previousColor = Color.red;
        protected Color _currentColor = Color.red;
        internal Color CurrentColor
        {
            get
            {
                return _currentColor;
            }
        }
        protected Color _targetColor = Color.red;

        protected float Factor
        {
            get
            {
                return Mathf.Clamp01(_timeElapsed / lerpDuration);
            }
        }
        #endregion

        void Awake()
        {
            if (targetMaterial == null)
                targetMaterial = GetComponent<Renderer>().material;
            if (targetLight == null)
                targetLight = GetComponent<Light>();
            if (trail == null)
                trail = GetComponent<TrailRenderer>();
            RandomizeNewColor();
        }

        void Update()
        {
            _timeElapsed += Time.deltaTime;
            Color curColor = Color.Lerp(_previousColor, _targetColor, Factor);
            SetColor(curColor);
        }

        void SetColor(Color a_color)
        {
            _currentColor = a_color;

            targetMaterial.SetColor("_EmissionColor", _currentColor);
            targetLight.color = _currentColor;

            _currentColor.a = 0.2f;
            trail.material.SetColor("_TintColor", _currentColor);
            _currentColor.a = 1f;
        }

        #region Randomization
        internal void RandomizeNewColor()
        {
            _targetColor = RandomHSV();

            while (ColorDistance(_targetColor, _currentColor) < minColorDistance)
            {
                _targetColor = RandomHSV();
            }

            _previousColor = _currentColor;

            _timeElapsed = 0f;
        }

        protected Color RandomHSV()
        {
            float targetColor = Random.Range(0,3);
            float r = 0f, g = 0f, b = 0f;
            if (targetColor == 0)
            {
                r = 1f;
                if (Random.value > 0.5f)
                {
                    g = Random.value;
                }
                else
                {
                    b = Random.value;
                }
            }
            else if (targetColor == 1)
            {
                g = 1f;
                if (Random.value > 0.5f)
                {
                    r = Random.value;
                }
                else
                {
                    b = Random.value;
                }
            }
            else
            {
                b = 1f;
                if (Random.value > 0.5f)
                {
                    g = Random.value;
                }
                else
                {
                    r = Random.value;
                }
            }

            Color color = new Color(r,g,b);
            return color;
        }

        float ColorDistance(Color a_first, Color a_second)
        {
            float distance = 0f;
            distance += Mathf.Abs(a_first.r - a_second.r);
            distance += Mathf.Abs(a_first.g - a_second.g);
            distance += Mathf.Abs(a_first.b - a_second.b);
            return distance;
        }
        #endregion
    }
}