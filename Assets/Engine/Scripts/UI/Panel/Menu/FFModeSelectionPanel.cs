using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace FF.UI
{
	internal class FFModeSelectionPanel : FFPanel
	{
		#region Inspector Properties
		public InputField playerNameInputField = null;
        #endregion

        internal void SetPlayerName (string playerName)
		{
			playerNameInputField.text = playerName;
		}

		internal string PlayerName
		{
            get
            {
                return playerNameInputField.text;
            }
		}
	}
}
