using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace FF.UI
{
	internal class FFMenuModePickerPanel : FFPanel
	{
		#region Inspector Properties
		public InputField playerNameInputField = null;
		#endregion


		internal void setPlayerNameInputField (string playerName)
		{
			playerNameInputField.text = playerName;
		}

		internal string GetPlayerNameInputField ()
		{
			return playerNameInputField.text;
		}
	}
}
