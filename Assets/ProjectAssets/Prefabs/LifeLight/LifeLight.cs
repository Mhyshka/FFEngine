using UnityEngine;
using System.Collections;

namespace FF.Pong
{
    internal class LifeLight : MonoBehaviour
    {
        #region Inspector Properties
        [Header("Light")]
        public new Light light = null;
        public Color onLightColor = Color.white;
        public Color offLightColor = Color.black;

        [Header("Material")]
        public new Renderer renderer = null;
        public Color onMatColor = Color.white;
        public Color offMatColor = Color.black;

        [Header("Particle System")]
        public ParticleSystem breakFx = null;
        #endregion

        #region Properties
        protected bool _isOn = false;
        internal bool IsOn
        {
            get
            {
                return _isOn;
            }
        }
        #endregion

        internal void SwitchOn()
        {
            _isOn = true;
            light.color = onLightColor;
            renderer.material.SetColor("_EmissionColor", onMatColor);
        }

        internal void SwitchOff()
        {
            _isOn = false;
            light.color = offLightColor;
            renderer.material.SetColor("_EmissionColor", offMatColor);

            breakFx.Play();
        }
    }
}