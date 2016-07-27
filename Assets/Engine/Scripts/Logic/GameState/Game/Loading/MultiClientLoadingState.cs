using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using FF.Network.Message;
using FF.Network.Receiver;
using FF.Network;
using FF.Pong;
using FF.UI;

namespace FF.Logic
{
    internal class MultiClientLoadingState : PongLoadingState
    {
        #region Properties
        protected ManualLoadingStep _serverReadyStep;

        protected GenericMessageReceiver _receiver;
        #endregion

        #region State Methods
        internal override void Enter()
        {
            base.Enter();
            _connectionLostPopupId = -1;
        }

        internal override void Exit()
        {
            base.Exit();
            if (_connectionLostPopupId != -1)
            {
                Engine.UI.DismissPopup(_connectionLostPopupId);
                _connectionLostPopupId = -1;
            }
        }
        #endregion

        #region Events
        protected override void RegisterForEvent()
        {
            base.RegisterForEvent();
            Engine.Game.Loading.onLoadingProgressReceived += OnLoadingProgressReceived;
            _receiver = new GenericMessageReceiver(OnEveryoneReady);
            Engine.Receiver.RegisterReceiver(EMessageChannel.LoadingComplete.ToString(),
                                            _receiver);

            Engine.Network.CurrentRoom.onRoomUpdated += OnRoomUpdate;
            Engine.Network.MainClient.onReconnection += OnReconnection;
            Engine.Network.MainClient.onConnectionEnded += OnConnectionLost;
            Engine.Network.MainClient.onConnectionEnded += OnConnectionEnded;
        }

        protected override void UnregisterForEvent()
        {
            base.UnregisterForEvent();
            Engine.Game.Loading.UnregisterLoadingStarted();
            
            Engine.Game.Loading.onLoadingProgressReceived -= OnLoadingProgressReceived;
            Engine.Receiver.UnregisterReceiver(EMessageChannel.LoadingComplete.ToString(),
                                                _receiver);

            if (Engine.Network.MainClient != null)
            {
                Engine.Network.CurrentRoom.onRoomUpdated -= OnRoomUpdate;
                Engine.Network.MainClient.onReconnection -= OnReconnection;
                Engine.Network.MainClient.onConnectionLost -= OnConnectionLost;
                Engine.Network.MainClient.onConnectionEnded -= OnConnectionEnded;
            }
        }

        #region Client Callbacks
        protected int _connectionLostPopupId = -1;
        protected void OnConnectionLost(FFNetworkClient a_client)
        {
            _connectionLostPopupId = FFConnectionLostPopup.RequestDisplay(OnConnectionLostPopupCancel, UIManager.POPUP_PRIO_HIGH);
        }

        protected void OnReconnection()
        {
            if (_connectionLostPopupId != -1)
            {
                Engine.UI.DismissPopup(_connectionLostPopupId);
                _connectionLostPopupId = -1;
            }
            _connectionLostPopupId = -1;
        }

        protected void OnConnectionEnded(FFNetworkClient a_client)
        {
            Engine.Network.LeaveCurrentRoom();
            if (_connectionLostPopupId != -1)
            {
                Engine.UI.DismissPopup(_connectionLostPopupId);
                _connectionLostPopupId = -1;
            }
            RequestGameMode("MenuGameMode");
        }

        protected void OnConnectionLostPopupCancel()
        {
            Engine.Network.LeaveCurrentRoom();
            if (_connectionLostPopupId != -1)
            {
                Engine.UI.DismissPopup(_connectionLostPopupId);
                _connectionLostPopupId = -1;
            }
            RequestGameMode("MenuGameMode");
        }
        #endregion

        protected void OnLoadingProgressReceived()
        {
            OnRoomUpdate(Engine.Network.CurrentRoom);
        }


        protected override void OnLoadingComplete()
        {
            base.OnLoadingComplete();
            SentMessage message = new SentMessage(new MessageEmptyData(),
                                                  EMessageChannel.LoadingComplete.ToString());
            Engine.Network.MainClient.QueueMessage(message);
        }
        #endregion

        #region LoadingStep
        protected override void SetupLoadingSteps()
        {
            base.SetupLoadingSteps();
            _serverReadyStep = new ManualLoadingStep("Waiting for players", 0f);
            _loadingSteps.Add(_serverReadyStep);
        }
        #endregion
    }
}