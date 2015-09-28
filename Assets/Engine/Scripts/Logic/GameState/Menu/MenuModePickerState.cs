using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using FF.UI;

namespace FF
{
	internal class MenuModePickerState : ANavigationMenuState
	{
		#region Inspector Properties
		#endregion

		#region Properties
		protected FFMenuModePickerPanel _gameModePickerPanel;
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
            FFLog.Log(EDbgCat.Logic, "Game Mode Picker state enter.");

            if (_gameModePickerPanel == null)
				_gameModePickerPanel = FFEngine.UI.GetPanel ("MenuModePickerPanel") as FFMenuModePickerPanel;
				
			_gameModePickerPanel.setPlayerNameInputField (FFEngine.Game.player.username);
            _gameModePickerPanel.playerNameInputField.onEndEdit.AddListener(delegate { OnEndEdit(_gameModePickerPanel.playerNameInputField); });

            _navigationPanel.setTitle ("Get Ready.");
		}

		internal override int Manage ()
		{
			return base.Manage ();
		}

		internal override void Exit ()
		{
			base.Exit ();
            _gameModePickerPanel.playerNameInputField.onEndEdit.RemoveAllListeners();
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
            if (FFEngine.NetworkStatus.IsConnectedToLan)
            {
                RequestState((int)EMenuStateID.GameRoomHost);
            }
            else
            {
                MenuWifiCheckState checkState = _gameMode.StateForId((int)EMenuStateID.WifiCheck) as MenuWifiCheckState;
                checkState.outState = _gameMode.StateForId((int)EMenuStateID.GameRoomHost);
                RequestState((int)EMenuStateID.WifiCheck);
            }
		}
		
		
		internal void OnJoinPressed(FFEventParameter a_args)
		{
			FFLog.Log(EDbgCat.Logic,"Game Mode Picker - OnJoinPressed");
            if(FFEngine.NetworkStatus.IsConnectedToLan)
            {
                RequestState((int)EMenuStateID.SearchForRooms);
            }
            else
            {
                MenuWifiCheckState checkState = _gameMode.StateForId((int)EMenuStateID.WifiCheck) as MenuWifiCheckState;
                checkState.outState = _gameMode.StateForId((int)EMenuStateID.SearchForRooms);
                RequestState((int)EMenuStateID.WifiCheck);
            }
			
		}

        internal void OnEndEdit(InputField a_inputField)
        {
            FFEngine.Game.player.username = a_inputField.text;
            a_inputField.Select();
        }
        #endregion

        #region focus
        internal override void OnGetFocus()
        {
            base.OnGetFocus();
            if (FFEngine.Inputs.ShouldUseNavigation)
            {
                _gameModePickerPanel.TrySelectWidget();
            }
        }
        #endregion
    }
}