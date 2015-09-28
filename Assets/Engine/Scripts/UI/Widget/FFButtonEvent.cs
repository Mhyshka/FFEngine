﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace FF.UI
{/// <summary>
/// Custom button to place over a Unity UI button. Called FFEngine.Event.FireEvent with the set event type or event key.
/// </summary>
	[RequireComponent(typeof(Button))]
	public class FFButtonEvent : MonoBehaviour
	{
		#region Inspector Properties
		public bool debug = false;
		
		public bool canTriggerWhileTransitionning = false;
		
		[HideInInspector]
		public EEventType eventType = EEventType.Next;
		
		[HideInInspector]
		public string eventKey = "";
		
		internal object Data = null;
		#endregion

		#region Properties
		protected Button _button;
        #endregion

        #region Methods
        protected virtual void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() => OnClick());
        }
		#endregion
		
		public void OnClick()
		{
#if !RELEASE
			if(debug)
			{
				FFLog.LogError("Button clicked : " + gameObject.name + " with event : " + eventKey + " / " + eventType.ToString() );
			}
#endif
			if(canTriggerWhileTransitionning || !FFEngine.UI.IsTransitionning)
			{
				FFEventParameter args = new FFEventParameter();
				if(Data != null)
				{
					args.data = Data;
				}
				
				if(eventType == EEventType.Custom)
					FFEngine.Events.FireEvent(eventKey, args);
				else
					FFEngine.Events.FireEvent(eventType, args);
			}
		}
	}
}