using UnityEngine;
using System.Collections;

namespace FF.Pong
{
    internal class BallHitFx : MonoBehaviour
    {
        public ParticleSystem fx = null;

        internal void Play(Color a_color, Vector3 a_position, Vector3 a_normal)
        {
            fx.transform.position = a_position;
            fx.transform.rotation = Quaternion.LookRotation(a_normal);

            fx.startColor = a_color;
            fx.Play();
        }
    }
}