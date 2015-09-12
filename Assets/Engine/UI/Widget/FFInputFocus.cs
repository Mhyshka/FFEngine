using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FF.UI
{
	public class FFInputFocus : MonoBehaviour
	{
		void OnEnable()
		{	
			RequestFocus();
		}
		
		public void RequestFocus()
		{
			if(FFEngine.Inputs.HasJoystickConnected || FFEngine.MultiScreen.IsTV)
			{
				Selectable selectable = GetComponent<Selectable>();
				selectable.OnSelect(new BaseEventData(EventSystem.current));
				selectable.Select();
			}
		}
	}
}