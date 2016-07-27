using UnityEngine;
using System.Collections;
using FF.Network;

using FF.UI;

namespace FF
{
    internal class MainMenuState : AMenuGameState
    {
        #region Inspector Properties
        #endregion

        #region Properties
        protected FFTitlePanel _titlePanel;
        #endregion

        #region States Methods
        internal override int ID
        {
            get
            {
                return (int)EMenuStateID.Main;
            }
        }

        internal override void Enter()
        {
            base.Enter();
            Engine.UI.HideSpecificPanel("WifiWarningPanel");
            _titlePanel = Engine.UI.GetPanel("MenuTitlePanel") as FFTitlePanel;
            _titlePanel.nicknameField.onSubmit.Add(new EventDelegate(OnNicknameSubmit));
            _titlePanel.nicknameField.onChange.Add(new EventDelegate(OnNicknameSubmit));
        }

        internal override int Manage()
        {
            return base.Manage();
        }

        internal override void Exit()
        {
            base.Exit();

            _titlePanel.nicknameField.onChange.Clear();
            _titlePanel.nicknameField.onSubmit.Clear();
        }
        #endregion

        #region Event Management
        protected override void RegisterForEvent()
        {
            base.RegisterForEvent();
            Engine.Events.RegisterForEvent("Host", OnHostPressed);
            Engine.Events.RegisterForEvent("Join", OnJoinPressed);
            Engine.Events.RegisterForEvent("Settings", OnSettingsPressed);
            Engine.Events.RegisterForEvent("Buy", OnBuyPressed);
        }

        protected override void UnregisterForEvent()
        {
            base.UnregisterForEvent();
            Engine.Events.UnregisterForEvent("Host", OnHostPressed);
            Engine.Events.UnregisterForEvent("Join", OnJoinPressed);
            Engine.Events.UnregisterForEvent("Settings", OnSettingsPressed);
            Engine.Events.UnregisterForEvent("Buy", OnBuyPressed);
        }

        protected void OnHostPressed(FFEventParameter a_args)
        {
            FFLog.Log(EDbgCat.Logic, "Main menu state - OnHostPressed");
            FFLog.Log(EDbgCat.Logic, "Game Mode Selection - OnHostPressed");
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

        protected void OnJoinPressed(FFEventParameter a_args)
        {
            FFLog.Log(EDbgCat.Logic, "Main menu state - OnJoinPressed");
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

        protected void OnBuyPressed(FFEventParameter a_args)
        {
            FFLog.Log(EDbgCat.Logic, "Main menu state - OnBuyPressed");
        }

        protected void OnSettingsPressed(FFEventParameter a_args)
        {
            RequestState((int)EMenuStateID.Settings);
        }

        internal void OnNicknameSubmit()
        {
            Engine.Game.Player.username = _titlePanel.nicknameField.value;
        }
        #endregion

    }
}