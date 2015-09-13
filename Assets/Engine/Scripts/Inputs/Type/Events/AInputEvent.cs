using UnityEngine;
using System.Collections;

namespace FF.Input
{
	internal abstract class AInputEvent
	{
		#region Properties
		internal string eventName;
		internal SimpleCallback onDown = null;
		internal SimpleCallback onUp = null;
		internal SimpleCallback onActive = null;
		#endregion
		
		internal virtual void DoUpdate(){}
		
		internal AInputEvent(EInputEventKey a_eventKey) : this(a_eventKey.ToString())
		{
		}
		
		internal AInputEvent(string a_eventKeyName)
		{
			eventName = a_eventKeyName;
			if(!FFEngine.Inputs.keys.ContainsKey(eventName))
				FFEngine.Inputs.keys.Add(eventName,this);
			else
				FFEngine.Inputs.keys[eventName] = this;
		}
	}
}