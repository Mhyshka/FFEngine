using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace FF
{
	internal class FFEventParameter
	{
		internal System.Object data = null;
	}
	
	internal delegate void EventCallback(FFEventParameter a_eventParam);
	
	internal class EventManager
	{
		#region Properties
		private Dictionary<string, EventCallback> _mapping;
		#endregion
	
		internal EventManager()
		{
			_mapping = new Dictionary<string, EventCallback> ();
		}
		
		#region Register
		internal void RegisterForEvent(EEventType a_type, EventCallback a_callback)
		{
			RegisterForEvent(a_type.ToString(), a_callback);
		}
		
		internal void RegisterForEvent(string a_eventKey, EventCallback a_callback)
		{
			if (!_mapping.ContainsKey (a_eventKey))
			{
				_mapping.Add (a_eventKey, a_callback);
			}
			else
			{
				_mapping[a_eventKey] += a_callback;
			}
		}
		
		internal void UnregisterForEvent(EEventType a_type, EventCallback a_callback)
		{
			UnregisterForEvent(a_type.ToString(), a_callback);
		}
		
		internal void UnregisterForEvent(string a_eventKey, EventCallback a_callback)
		{
			if (_mapping.ContainsKey (a_eventKey))
			{
				_mapping[a_eventKey] -= a_callback;
			}
		}
		#endregion
	
	
		#region Fire
		internal void FireEvent(EEventType a_type, FFEventParameter a_eventParam = null)
		{
			FireEvent(a_type.ToString());
		}
		
		internal void FireEvent(string a_eventKey, FFEventParameter a_eventParam = null)
		{
			if (_mapping.ContainsKey (a_eventKey) && _mapping[a_eventKey] != null)
			{
				_mapping[a_eventKey].Invoke(a_eventParam);
			}
			else
			{
				Debug.LogWarning("No event register on : " + a_eventKey);
			}
		}
		#endregion
	}
}
