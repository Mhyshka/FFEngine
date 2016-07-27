using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

namespace FF.UI
{
    internal class FFIpPortInputField : MonoBehaviour
    {
        #region Inspector Properties
        public UIInput inputField = null;
        public UIButton button = null;

        public Color validColor = Color.green;
        public Color invalidColor = Color.red;
        #endregion

        #region Proeprties
        protected Regex _portRegex = null;
        protected string _endpointPattern = @"^\d{4,5}$";

        protected EventDelegate _onChange = null;

        protected bool _isValid = false;
        internal bool IsValid
        {
            get
            {
                return _isValid;
            }
        }

        internal string Value
        {
            get
            {
                return inputField.value;
            }
        }
        #endregion

        protected void Awake()
        {
            _portRegex = new Regex(_endpointPattern, RegexOptions.Singleline);
            _onChange = new EventDelegate(OnPortValueChanged);
            inputField.onChange.Add(_onChange);
        }

        protected void OnDestroy()
        {
            inputField.onChange.Remove(_onChange);
        }

        public void OnPortValueChanged()
        {
            string str = inputField.value;
            _isValid = false;
            if (_portRegex.IsMatch(str))
            {
                int val = 0;
                _isValid = int.TryParse(str, out val);
            }

            if (_isValid)
            {
                inputField.activeTextColor = validColor;
            }
            else
            {
                inputField.activeTextColor = invalidColor;
            }
        }

        protected virtual void OnDisable()
        {
            inputField.enabled = false;
            button.isEnabled = false;
        }

        protected virtual void OnEnable()
        {
            inputField.enabled = true;
            button.isEnabled = true;
        }
    }
}
