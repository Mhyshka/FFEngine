using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace FF
{/// <summary>
/// Custom button to place over a Unity UI button. Called FFEngine.Event.FireEvent with the set event type or event key.
/// </summary>
	[RequireComponent(typeof(Button))]
	public class FFButtonEvent : MonoBehaviour
	{
		#region Inspector Properties
		public bool debug = false;
		
		[HideInInspector]
		public EEventType eventType = EEventType.Next;
		
		[HideInInspector]
		public string eventKey = "";
		#endregion

		#region Properties
		protected Button _button;
		#endregion

		#region Methods
		protected virtual void Start ()
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
			if(eventType == EEventType.Custom)
				FFEngine.Events.FireEvent(eventKey);
			else
				FFEngine.Events.FireEvent(eventType);
		}
	}
}