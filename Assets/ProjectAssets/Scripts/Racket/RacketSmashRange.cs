using UnityEngine;
using System.Collections;
using System;

namespace FF.Pong
{
    internal class RacketSmashRange : ARacketComponent
    {
        internal bool didSmash = false;

        protected bool _isInSmashRange = false;
        internal bool IsInSmashRange
        {
            get
            {
                return _isInSmashRange;
            }
        }

        internal override void Activate()
        {
        }

        internal override void TearDown()
        {
        }

        void OnTriggerEnter(Collider a_collider)
        {
            if (a_collider.GetComponent<BallCollider>() != null)
            {
                Debug.LogError("True");
                _isInSmashRange = true;
            }
        }

        void OnTriggerExit(Collider a_collider)
        {
            if (a_collider.GetComponent<BallCollider>() != null)
            {
                Debug.LogError("False");
                didSmash = false;
                _isInSmashRange = false;
            }
        }
    }
}