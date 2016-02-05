using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using FF.Network.Message;
using FF.Network.Receiver;
using FF.Pong;

namespace FF.Logic
{
    internal class MultiClientLoadingState : PongLoadingState
    {
        #region Properties
        protected ManualLoadingStep _serverReadyStep;

        protected GenericMessageReceiver _receiver;
        #endregion

        #region State Methods
        #endregion

        #region Events
        protected override void RegisterForEvent()
        {
            base.RegisterForEvent();
            Engine.Game.Loading.onLoadingProgressReceived += OnLoadingProgressReceived;
            _receiver = new GenericMessageReceiver(OnEveryoneReady);
            Engine.Receiver.RegisterReceiver(EMessageChannel.LoadingComplete.ToString(),
                                            _receiver);
        }

        protected override void UnregisterForEvent()
        {
            base.UnregisterForEvent();
            Engine.Game.Loading.UnregisterLoadingStarted();
            
            Engine.Game.Loading.onLoadingProgressReceived -= OnLoadingProgressReceived;
            Engine.Receiver.UnregisterReceiver(EMessageChannel.LoadingComplete.ToString(),
                                                _receiver);
        }

        protected void OnLoadingProgressReceived()
        {
            OnRoomUpdate(Engine.Game.CurrentRoom);
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