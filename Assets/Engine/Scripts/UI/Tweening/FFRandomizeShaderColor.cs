using UnityEngine;
using System.Collections;

namespace FF
{
	internal class FFRandomizeShaderColor : MonoBehaviour
	{
        #region Inspector Properties
        public Material targetMaterial = null;
        public float lerpDuration = 1f;
        #endregion

        #region Properties
        protected float _timeElapsed = 0f;
        protected Color _previousColor = Color.red;
        protected Color _targetColor = Color.yellow;
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
            if (targetMaterial == null)
                targetMaterial = GetComponent<Renderer>().material;
            RandomizeNewColor();
        }

        void Update()
        {
            _timeElapsed += Time.deltaTime;
            Color curColor = Color.Lerp(_previousColor, _targetColor, Factor);
            targetMaterial.SetColor("_Color", curColor);
            if (_timeElapsed > lerpDuration)
            {
                RandomizeNewColor();
                _timeElapsed = 0f;
            }
        }

        void RandomizeNewColor()
        {
            _previousColor = _targetColor;
            _targetColor = RandomHSV();
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
    }
}