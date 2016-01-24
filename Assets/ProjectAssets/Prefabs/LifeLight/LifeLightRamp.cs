using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace FF.Pong
{
    internal class LifeLightRamp : MonoBehaviour
    {
        #region Inspector Properties
        public GameObject lifeLightPrefab = null;
        public float spaceBetweenLights = 1f;
        #endregion

        #region Properties
        protected List<LifeLight> _lights;

        internal List<LifeLight> Lights
        {
            get
            {
                return _lights;
            }
        }

        internal int RemainingLives
        {
            get
            {
                int lives = 0;
                for (int i = 0; i < _lights.Count; i++)
                {
                    if (_lights[i].IsOn)
                        lives++;
                }
                return lives;
            }
        }
        #endregion

        internal void Init(int a_lifeCount)
        {
            _lights = new List<LifeLight>();

            for (int i = 0; i < a_lifeCount; i++)
            {
                GameObject newLightGo = GameObject.Instantiate(lifeLightPrefab,
                                                            transform.position,
                                                            transform.rotation) as GameObject;

                LifeLight lightScript = newLightGo.GetComponent<LifeLight>();
                newLightGo.transform.parent = transform;
                newLightGo.transform.localPosition = lifeLightPrefab.transform.localPosition;
                newLightGo.transform.localRotation = lifeLightPrefab.transform.localRotation;

                Vector3 localPos = newLightGo.transform.localPosition;
                localPos.x = (a_lifeCount  - Mathf.CeilToInt(a_lifeCount / 2f) - i) * spaceBetweenLights;
                newLightGo.transform.localPosition = localPos;
                _lights.Add(lightScript);
            }
        }


        internal void TakeRandomLife()
        {
            int value = Random.Range(0, RemainingLives);
            for (int i = 0; i < _lights.Count; i++)
            {
                if (_lights[i].IsOn)
                {
                    if (value == 0)
                    {
                        _lights[i].SwitchOff();
                        break;
                    }
                    else
                        value--;
                }
            }
        }

        internal void TakeLife()
        {
            foreach (LifeLight each in _lights)
            {
                if (each.IsOn)
                {
                    each.SwitchOff();
                    break;
                }
            }
        }

        internal void ResetLives()
        {
            foreach (LifeLight each in _lights)
            {
                each.SwitchOn();
            }
        }
    }
}