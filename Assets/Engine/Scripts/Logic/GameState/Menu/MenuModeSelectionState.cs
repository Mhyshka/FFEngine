using UnityEngine;
using System.Collections;

using FF.UI;

namespace FF
{
	internal class MenuModeSelectionState : ANavigationMenuState
	{
		#region Inspector Properties
		#endregion

		#region Properties
		protected FFModeSelectionPanel _gameModeSelectionPanel;
        #endregion

        #region States Methods
        internal override int ID {
			get {
				return (int)EMenuStateID.ModeSelection;
			}
		}

		internal override void Enter ()
		{
			base.Enter ();
            FFLog.Log(EDbgCat.Logic, "Game Mode Selection state enter.");

            if (_gameModeSelectionPanel == null)
				_gameModeSelectionPanel = Engine.UI.GetPanel ("MenuModeSelectionPanel") as FFModeSelectionPanel;
				
			_gameModeSelectionPanel.SetPlayerName (Engine.Game.Player.username);
            //_gameModeSelectionPanel.playerNameInputField.onEndEdit.AddListener(delegate { OnEndEdit(_gameModeSelectionPanel.playerNameInputField); });

            _navigationPanel.SetTitle ("Get Ready.");
            Engine.UI.HideSpecificPanel("WifiWarningPanel");
        }

		internal override int Manage ()
		{
			return base.Manage ();
		}

		internal override void Exit ()
		{
			base.Exit ();
            _gameModeSelectionPanel.playerNameInputField.onEndEdit.RemoveAllListeners();
        }
		#endregion

		#region Event Management
		protected override void RegisterForEvent ()
		{
			base.RegisterForEvent ();
			Engine.Events.RegisterForEvent ("OnHostPressed", OnHostPressed);
			Engine.Events.RegisterForEvent ("OnJoinPressed", OnJoinPressed);
		}

		protected override void UnregisterForEvent ()
		{
			base.UnregisterForEvent ();
			Engine.Events.UnregisterForEvent ("OnHostPressed", OnHostPressed);
			Engine.Events.UnregisterForEvent ("OnJoinPressed", OnJoinPressed);
        }


		internal void OnHostPressed(FFEventParameter a_args)
		{
            FFLog.Log(EDbgCat.Logic,"Game Mode Selection - OnHostPressed");
            Engine.Game.NetPlayer.isHost = true;
            if (Engine.NetworkStatus.IsConnectedToLan)
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
			FFLog.Log(EDbgCat.Logic, "Game Mode Selection - OnJoinPressed");
            Engine.Game.NetPlayer.isHost = false;
            if (Engine.NetworkStatus.IsConnectedToLan)
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

        /*internal void OnEndEdit(InputField a_inputField)
        {
            Engine.Game.Player.username = _gameModeSelectionPanel.PlayerName;
        }*/
        #endregion

        #region focus
        #endregion
    }
}