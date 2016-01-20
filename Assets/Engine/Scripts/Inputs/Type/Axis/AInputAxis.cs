using UnityEngine;
using System.Collections;

namespace FF.Input
{
	internal abstract class AInputAxis
	{
		#region Properties
		internal string eventKeyName;
		internal abstract float Value{get;}
		#endregion
		
		internal AInputAxis(EInputAxisKey a_eventKey,
								bool a_isInverted = false) : this(a_eventKey.ToString(), a_isInverted)
		{
		}
		
		internal AInputAxis(string a_eventKeyName,
		                    bool a_isInverted = false)
		{
			eventKeyName = a_eventKeyName;
            Engine.Inputs.RegisterInputAxis(this);
		}
		
		internal bool IsTriggering()
		{
			return Mathf.Abs(Value) > 0.4f;
		}
		
		internal virtual void DoUpdate (){}
	}
}