using UnityEngine;
using System.Collections.Generic;
using System.Net;
using FF.UI;

namespace FF.Logic
{
    internal class MenuSettingsState : AMenuGameState
    {
        protected FFSettingsPanel _settingsPanel = null;

        #region State Methods
        internal override int ID
        {
            get
            {
                return (int)EMenuStateID.Settings;
            }
        }

        internal override void Enter()
        {
            base.Enter();
            _settingsPanel = Engine.UI.GetPanel("MenuSettingsPanel") as FFSettingsPanel;
            PreparePanel();
        }

        internal override void Exit()
        {
            base.Exit();
            SaveModifications();
        }
        #endregion

        protected void SaveModifications()
        {
            Engine.Game.autoPort = _settingsPanel.portWidget.autoToggle.IsSelected;
            Engine.Network.PreferedPort = _settingsPanel.portWidget.Port;
            Engine.Network.PreferedNetworkAddress = _settingsPanel.networkInterfaceWidget.TargetAddress;
        }

        protected void PreparePanel()
        {
            _settingsPanel.portWidget.Init(Engine.Game.autoPort, Engine.Network.PreferedPort);
            PrepareNetworkInterface();
        }

        protected void PrepareNetworkInterface()
        {
            _settingsPanel.networkInterfaceWidget.Init(Engine.Network.NetworkAddresses,
                                                        Engine.Network.PreferedNetworkAddress);
        }
    }
}