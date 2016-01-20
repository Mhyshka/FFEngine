using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using FF.Network.Message;
using FF.Pong;

namespace FF.Logic
{
    internal class MultiClientLoadingState : PongLoadingState
    {
        #region Properties
        protected ManualLoadingStep _serverReadyStep;
        #endregion

        #region State Methods
        #endregion

        #region Events
        protected override void RegisterForEvent()
        {
            base.RegisterForEvent();
            Engine.Game.Loading.onLoadingProgressReceived += OnLoadingProgressReceived;
            Engine.Receiver.RegisterReceiver(EMessageType.LoadingEveryoneReady, new Network.Receiver.MessageLoadingEveryoneReady(OnEveryoneReady));
        }

        protected override void UnregisterForEvent()
        {
            base.UnregisterForEvent();
            Engine.Game.Loading.UnregisterLoadingStarted();
            
            Engine.Game.Loading.onLoadingProgressReceived -= OnLoadingProgressReceived;
            Engine.Receiver.UnregisterReceiver(EMessageType.LoadingEveryoneReady, new Network.Receiver.MessageLoadingEveryoneReady(OnEveryoneReady));
        }

        protected void OnLoadingProgressReceived()
        {
            OnRoomUpdate(Engine.Game.CurrentRoom);
        }


        protected override void OnLoadingComplete()
        {
            base.OnLoadingComplete();
            Engine.Network.MainClient.QueueMessage(new MessageLoadingComplete());
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