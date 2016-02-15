using UnityEngine;
using System.Collections;


using FF.Multiplayer;

namespace FF.UI
{
/// <summary>
/// Custom button to place over a Unity UI button. Called FFEngine.Event.FireEvent with the set event type or event key.
/// </summary>
	public class FFRoomSelectionButton : MonoBehaviour
	{
		#region Inspector Properties
		public bool debug = false;
		
		public bool canTriggerWhileTransitionning = false;
		#endregion

		#region Properties
		internal Room room;
		protected UIButton _button;
		#endregion

		#region Methods
		protected virtual void Start ()
		{
			_button = GetComponent<UIButton>();
			_button.onClick.Add(new EventDelegate(OnClick));
		}
        #endregion

        public void OnClick()
		{
            if (enabled)
            {
#if !RELEASE
                if (debug)
                {
                    FFLog.LogError("Button room connect : " + room.roomName);
                }
#endif
                if (canTriggerWhileTransitionning || !Engine.UI.IsTransitionning)
                {
                    FFEventParameter args = new FFEventParameter();
                    args.data = room;
                    Engine.Events.FireEvent(FFEventType.Connect, args);
                }
            }
		}
	}
}