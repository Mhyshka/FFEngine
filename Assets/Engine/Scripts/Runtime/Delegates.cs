using UnityEngine;

namespace FF
{
	#region Callbacks
	internal delegate void SimpleCallback();
    internal delegate void IntCallback(int a_val);
    internal delegate void StringCallback(string a_string);
	#endregion
}