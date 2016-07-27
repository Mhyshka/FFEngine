using UnityEngine;
using System.Collections;

namespace FF.UI
{
	internal class FFIpPortWidget : MonoBehaviour
	{
        public FFIpPortInputField portInputField = null;
        public FFToggle autoToggle = null;

        void Start()
        {
            portInputField.enabled = !autoToggle.IsSelected;
        }

        public void ToggleAutoMode()
        {
            autoToggle.Toggle();
            portInputField.enabled = !autoToggle.IsSelected;
        }

        internal int Port
        {
            get
            {
                if (portInputField.IsValid)
                    return int.Parse(portInputField.Value);

                return 0;
            }
        }

        internal void Init(bool a_isAutoPort, int a_targetPort)
        {
            autoToggle.SetSelected(a_isAutoPort);
            portInputField.enabled = !autoToggle.IsSelected;
            portInputField.inputField.value = a_targetPort.ToString();
        }
	}
}