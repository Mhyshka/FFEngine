using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

internal class IPEndpointInput : MonoBehaviour
{
    #region Inspector Properties
    public UIInput inputField = null;

    public Color validColor = Color.green;
    public Color invalidColor = Color.red;
    #endregion

    #region Proeprties
    protected Regex _endpointRegex = null;
    protected string _endpointPattern = @"^(\d{1,3}\.){3}\d{1,3}:\d{4,5}$";

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
        _endpointRegex = new Regex(_endpointPattern, RegexOptions.Singleline);
    }

    public void OnDirectConnectValueChanged()
    {
        string value = inputField.value;
        _isValid = false;
        if (_endpointRegex.IsMatch(value))
        {
            string[] split = value.Split(new char[] { '.', ':' });
            _isValid = split.Length == 5;

            if (split.Length == 5)
            {
                int[] ipGroups = new int[5];
                for (int i = 0; i < 5; i++)
                {
                    ipGroups[i] = int.Parse(split[i]);
                }

                for (int i = 0; i < 4; i++)
                {
                    _isValid = _isValid && ipGroups[i] <= 255;
                }

                _isValid = _isValid && ipGroups[4] <= 65535;
            }
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
}
