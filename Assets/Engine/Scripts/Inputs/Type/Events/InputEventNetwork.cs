using UnityEngine;
using System.Collections;

namespace FF.Input
{
	internal class InputEventNetwork : AInputEvent
	{
        #region properties
		#endregion
		
		internal InputEventNetwork(EInputEventKey a_eventKey) : this(a_eventKey.ToString())
		{
		}
		
		internal InputEventNetwork(string a_eventKeyName) : base(a_eventKeyName)
		{
        }
		
		#region Input Methods
		internal override void DoUpdate ()
		{
			base.DoUpdate ();
		}


		#endregion
	}
}