using UnityEngine;
using System.Collections;

using FF.UI;

namespace FF
{
	internal class GameModePickerState : ANavigationMenuState
	{
		#region Inspector Properties
		#endregion

		#region Properties
		protected FFGameModePickerPanel _gameModePickerPanel;
		#endregion

		#region States Methods
		internal override int ID {
			get {
				return (int)EMenuStateID.GameModePicker;
			}
		}

		internal override void Enter ()
		{
			base.Enter ();
			FFLog.Log(EDbgCat.Logic,"Game Mode Picker state enter.");

			if(_gameModePickerPanel == null)
				_gameModePickerPanel = FFEngine.UI.GetPanel ("GameModePickerPanel") as FFGameModePickerPanel;
				
			_gameModePickerPanel.setPlayerNameInputField (SystemInfo.deviceName);

			_navigationPanel.setTitle ("Get Ready.");
		}

		internal override int Manage ()
		{
			return base.Manage ();
		}

		internal override void Exit ()
		{
			base.Exit ();
			SavePlayerUsername ();
		}
		#endregion

		#region Internal Methods
		protected void SavePlayerUsername ()
		{	
			_networkGameMode.playerName = _gameModePickerPanel.GetPlayerNameInputField ();
		}
		#endregion

		#region Event Management
		protected override void RegisterForEvent ()
		{
			base.RegisterForEvent ();
			FFEngine.Events.RegisterForEvent ("OnHostPressed", OnHostPressed);
			FFEngine.Events.RegisterForEvent ("OnJoinPressed", OnJoinPressed);
		}

		protected override void UnregisterForEvent ()
		{
			base.UnregisterForEvent ();
			FFEngine.Events.UnregisterForEvent ("OnHostPressed", OnHostPressed);
			FFEngine.Events.UnregisterForEvent ("OnJoinPressed", OnJoinPressed);
		}


		internal void OnHostPressed(FFEventParameter a_args)
		{
			FFLog.Log(EDbgCat.Logic,"Game Mode Picker - OnHostPressed");
			RequestState ((int)EMenuStateID.GameRoomHost);
		}
		
		
		internal void OnJoinPressed(FFEventParameter a_args)
		{
			FFLog.Log(EDbgCat.Logic,"Game Mode Picker - OnJoinPressed");
			RequestState ((int)EMenuStateID.HostList);
		}
		#endregion

	}
}