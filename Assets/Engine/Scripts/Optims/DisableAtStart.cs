using UnityEngine;
using System.Collections;

namespace FF
{
    internal class DisableAtStart : MonoBehaviour
    {
        public MonoBehaviour[] behaviours = null;

        void Start()
        {
            foreach (MonoBehaviour each in behaviours)
            {
                each.enabled = false;
            }
        }
    }
}
