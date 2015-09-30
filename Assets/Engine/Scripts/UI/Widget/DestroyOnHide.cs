using UnityEngine;
using System.Collections;

namespace FF.UI
{
    internal class DestroyOnHide : MonoBehaviour
    {
        public void OnHidden()
        {
            Destroy(gameObject);
        }
    }
}
