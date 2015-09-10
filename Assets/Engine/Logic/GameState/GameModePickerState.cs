using UnityEngine;
using System.Collections;

namespace FF
{
	internal class GameModePickerState : AMenuGameState
	{
		#region Inspector Properties
		#endregion

		#region Properties
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

			FFGameModePickerPanel lGameModePickerPanel = FFEngine.UI.GetPanel ("GameModePickerPanel") as FFGameModePickerPanel;
			lGameModePickerPanel.setPlayerNameInputField (SystemInfo.deviceName);

			FFNavigationBarPanel lNavigationBarPanel = FFEngine.UI.GetPanel ("NavigationBarPanel") as FFNavigationBarPanel;
			lNavigationBarPanel.setTitle ("Alex est un blaireaudoudou");
		}

		internal override int Manage ()
		{
			return base.Manage ();
		}

		internal override void Exit ()
		{
			base.Exit ();
		}
		#endregion

		#region Internal Methods
		protected void savePlayerUsername ()
		{	
			FFGameModePickerPanel lGameModePickerPanel = FFEngine.UI.GetPanel ("GameModePickerPanel") as FFGameModePickerPanel;
			NetworkMenuGameMode lGameMode = _gameMode as NetworkMenuGameMode;
			lGameMode.playerName = lGameModePickerPanel.getPlayerNameInputField ();
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

		
		internal void OnEvent(FFEventParameter a_args)
		{
			Debug.Log ("test event");
			Debug.Log (a_args);
		}


		internal void OnHostPressed(FFEventParameter a_args)
		{
			FFLog.Log(EDbgCat.Logic,"Game Mode Picker - OnHostPressed");

			savePlayerUsername ();

			RequestState ((int)EMenuStateID.GameRoomHost);
		}
		
		
		internal void OnJoinPressed(FFEventParameter a_args)
		{
			FFLog.Log(EDbgCat.Logic,"Game Mode Picker - OnJoinPressed");

			savePlayerUsername ();

			RequestState ((int)EMenuStateID.HostList);
		}
		#endregion

	}
}