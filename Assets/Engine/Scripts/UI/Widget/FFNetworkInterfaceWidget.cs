using UnityEngine;
using System.Collections.Generic;
using System.Net;

namespace FF.UI
{
	internal class FFNetworkInterfaceWidget : MonoBehaviour
	{
        #region Inspector Properties
        public UIPopupList popupList = null;
        public UILabel label = null;
        #endregion

        #region Properties
        protected EventDelegate _onChanged = null;

        protected string _targetAddress = null;
        internal IPAddress TargetAddress
        {
            get
            {
                IPAddress address = null;
                IPAddress.TryParse(_targetAddress, out address);
                return address;
            }
        }
        #endregion

        void Awake()
        {
            _onChanged = new EventDelegate(OnValueChanged);
            popupList.onChange.Add(_onChanged);
        }

        void OnDestroy()
        {
            popupList.onChange.Remove(_onChanged);
        }

        protected void OnValueChanged()
        {
            _targetAddress = popupList.value;
            label.text = popupList.value;
        }

        internal void Init(List<IPAddress> a_addresses, IPAddress a_selectedAddress)
        {
            if (a_addresses.Count > 0)
            {
                List<string> values = new List<string>();
                foreach (IPAddress each in a_addresses)
                {
                    values.Add(each.ToString());
                }

                popupList.items = values;
                popupList.value = a_selectedAddress.ToString();
                _targetAddress = a_selectedAddress.ToString();
            }
            else
            {
                List<string> values = new List<string>();
                values.Add("None");

                popupList.items = values;
                popupList.value = "None";
                _targetAddress = "None";
            }
        }
	}
}