using UnityEngine;
using System.Collections;

namespace FF.UI
{/// <summary>
/// Custom button to place over a Unity UI button. Called FFEngine.Event.FireEvent with the set event type or event key.
/// </summary>
    [RequireComponent(typeof(BoxCollider))]
	public class FFButtonEvent : MonoBehaviour
	{
        #region Inspector Properties
		public bool debug = false;
		
		public bool canTriggerWhileTransitionning = false;
		
		[HideInInspector]
		public FFEventType eventType = FFEventType.Next;
		
		[HideInInspector]
		public string eventKey = "";
		
		internal object Data = null;
        #endregion

        void Awake()
        {
            /*UIButton button = GetComponent<UIButton>();
            button.onClick.Add(new EventDelegate(OnButtonClicked));*/
        }

		public void OnClick()
		{
#if !RELEASE
			if(debug)
			{
				FFLog.LogError("Button clicked : " + gameObject.name + " with event : " + eventKey + " / " + eventType.ToString() );
			}
#endif
			if(canTriggerWhileTransitionning || !Engine.UI.IsTransitionning)
			{
				FFEventParameter args = new FFEventParameter();
				if(Data != null)
				{
					args.data = Data;
				}
				
				if(eventType == FFEventType.Custom)
					Engine.Events.FireEvent(eventKey, args);
				else
					Engine.Events.FireEvent(eventType, args);
			}
		}
	}
}