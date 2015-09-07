using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace FF
{
	internal class FFGameModePickerPanel : FFPanel
	{
		#region Inspector Properties
		public InputField playerNameInputField = null;
		#endregion


		internal void setPlayerNameInputField (string playerName)
		{
			playerNameInputField.text = playerName;
		}

		internal string getPlayerNameInputField ()
		{
			return playerNameInputField.text;
		}

			// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}
	}
}
