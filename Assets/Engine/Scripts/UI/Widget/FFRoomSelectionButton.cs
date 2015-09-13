using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using FF.Networking;

namespace FF.UI
{/// <summary>
/// Custom button to place over a Unity UI button. Called FFEngine.Event.FireEvent with the set event type or event key.
/// </summary>
	[RequireComponent(typeof(Button))]
	public class FFRoomSelectionButton : MonoBehaviour
	{
		#region Inspector Properties
		public bool debug = false;
		
		public bool canTriggerWhileTransitionning = false;
		#endregion

		#region Properties
		internal FFRoom room;
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
				FFLog.LogError("Button room connect : " + room.roomName );
			}
#endif
			if(canTriggerWhileTransitionning || !FFEngine.UI.IsTransitionning)
			{
				FFEventParameter args = new FFEventParameter();
				args.data = room;
				FFEngine.Events.FireEvent(EEventType.Connect, args);
			}
		}
	}
}