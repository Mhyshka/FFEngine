using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using FF.Network;
using FF.Multiplayer;
using FF.Network.Message;

namespace FF.Input
{
    internal class ServerInputManager : InputManager
    {
        #region Properties
        protected Dictionary<int, NetworkInputManager> _networkManagers;
        protected bool _isInServerMode;
        protected bool _isInClientMode;

        protected Network.Receiver.InputEventReceiver _networkEventReceiver;
        #endregion

        internal ServerInputManager(bool a_registerForBack = false) : base(a_registerForBack)
        {
            _isInServerMode = false;
            _isInClientMode = false;
        }

        internal override void TearDown()
        {
            base.TearDown();

            DisableServerMode();
        }

        #region Properties
        internal void RegisterNetworkManager(int a_id)
        {
            if (_networkManagers.ContainsKey(a_id))
            {
                FFLog.LogError(EDbgCat.Input,"Can't register NetworkInput - Duplicate id : " + a_id);
            }
            _networkManagers.Add(a_id, new NetworkInputManager());
        }

        internal InputManager ManagerForClient(int a_id)
        {
            //Self Input Manager
            if (a_id == Engine.Network.NetworkID)
                return this;

            //Client network input manager
            NetworkInputManager manager = null;
            _networkManagers.TryGetValue(a_id, out manager);
            return manager;
        }

        internal void UnregisterNetworkManager(int a_id)
        {
            if (!_networkManagers.ContainsKey(a_id))
            {
                FFLog.LogError(EDbgCat.Input, "Can't unregister NetworkInput - id not found : " + a_id);
            }
            else
            {
                _networkManagers.Remove(a_id);
            }
        }
        #endregion

        #region Server Mode
        internal void EnableServerMode()
        {
            _isInServerMode = true;
            Engine.Network.Server.onClientAdded += OnClientConnected;
            Engine.Network.Server.onClientReconnection += OnClientConnected;
            Engine.Network.Server.onClientLost += OnClientLost;

            _networkManagers = new Dictionary<int, NetworkInputManager>();

            _networkEventReceiver = new Network.Receiver.MessageInputEventReceiver();
            Engine.Receiver.RegisterReceiver(EHeaderType.InputEvent,
                                             _networkEventReceiver);
        }

        internal void DisableServerMode()
        {
            if (_isInServerMode)
            {
                _isInServerMode = false;
                Engine.Network.Server.onClientAdded -= OnClientConnected;
                Engine.Network.Server.onClientReconnection -= OnClientConnected;
                Engine.Network.Server.onClientLost -= OnClientLost;

                foreach (NetworkInputManager each in _networkManagers.Values)
                {
                    each.TearDown();
                }
                _networkManagers.Clear();

                Engine.Receiver.UnregisterReceiver(EHeaderType.InputEvent,
                                                   _networkEventReceiver);
            }
        }
        #endregion

        #region Client Mode
        internal void EnableClientMode()
        {
            _isInClientMode = true;
            RegisterClientEventForward();
        }

        internal void DisableClientMode()
        {
            if (_isInClientMode)
            {
                _isInClientMode = false;
                UnregisterClientEventForward();
            }
        }

        protected void RegisterClientEventForward()
        {
            foreach (InputEventKey each in _keys.Values)
            {
                each.EnableNetworkForward();
            }
        }

        protected void UnregisterClientEventForward()
        {
            foreach (InputEventKey each in _keys.Values)
            {
                each.DisableNetworkForward();
            }
        }
        #endregion

        #region Connection
        protected void OnClientConnected(FFTcpClient a_client)
        {
            RegisterNetworkManager(a_client.NetworkID);
        }

        protected void OnClientLost(FFTcpClient a_client)
        {
            UnregisterNetworkManager(a_client.NetworkID);
        }
        #endregion
    }
}