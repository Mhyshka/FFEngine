using UnityEngine;
using System.Collections;

namespace FF.Pong
{
    internal class BallCollider : MonoBehaviour
    {
        internal ABall ball = null;
        internal Collider modelCollider = null;

        protected void Awake()
        {
            modelCollider = GetComponent<Collider>();
        }
    }
}