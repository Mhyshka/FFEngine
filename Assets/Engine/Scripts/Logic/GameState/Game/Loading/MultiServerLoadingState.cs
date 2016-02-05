using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using FF.Handler;
using FF.Network;
using FF.Network.Message;
using FF.Multiplayer;

using FF.Pong;

namespace FF.Logic
{
    internal class MultiServerLoadingState : PongLoadingState
    {
        #region Properties
        //protected NetworkCheck _networkCheckHandler;

        protected ManualLoadingStep _allPlayerReadyStep;
        #endregion

        #region State Methods
        internal override void Enter()
        {
            base.Enter();
            //_networkCheckHandler = null;
        }

        internal override int Manage()
        {
            //TODO Network Check
            /*if (_networkCheckHandler == null && AllPlayersReady)
            {
                _networkCheckHandler = new NetworkCheck(OnNetworkCheckSuccess, OnNetworkCheckFailed, null, null);
            }*/
            return base.Manage();
        }
        #endregion

        #region Events
        protected override void RegisterForEvent()
        {
            base.RegisterForEvent();
            
            Engine.Game.Loading.onLoadingCompleteReceived += OnLoadingCompleteReceived;
        }

        protected override void UnregisterForEvent()
        {
            base.UnregisterForEvent();
            Engine.Game.Loading.UnregisterLoadingComplete();
            
            Engine.Game.Loading.onLoadingCompleteReceived -= OnLoadingCompleteReceived;
        }

        protected void OnLoadingCompleteReceived()
        {
            OnRoomUpdate(Engine.Game.CurrentRoom);
        }

        protected override void OnLoadingComplete()
        {
            base.OnLoadingComplete();
            Engine.Network.Server.LoopbackClient.Mirror.QueueMessage(new MessageLoadingComplete());
        }
        #endregion

        protected void OnNetworkCheckSuccess(List<FFTcpClient> a_success, List<FFTcpClient> a_failed)
        {
            _allPlayerReadyStep.SetComplete();
            MessageLoadingEveryoneReady message = new MessageLoadingEveryoneReady();
            Engine.Network.Server.BroadcastMessage(message);
            RequestState(outState.ID);
        }

        protected void OnNetworkCheckFailed(List<FFTcpClient> a_success, List<FFTcpClient> a_failed)
        {
            _networkCheckHandler = null;
        }

        #region LoadingStep
        protected override void SetupLoadingSteps()
        {
            base.SetupLoadingSteps();
            _allPlayerReadyStep = new ManualLoadingStep("Waiting for players", 0f);
            _loadingSteps.Add(_allPlayerReadyStep);
        }

        protected bool AllPlayersReady
        {
            get
            {
                bool isReady = true;
                PlayerDictionary<PlayerLoadingWrapper> loadingWrappers = Engine.Game.Loading.PlayersLoadingState;
                foreach (KeyValuePair<int, PlayerLoadingWrapper> each in loadingWrappers)
                {
                    isReady = isReady && each.Value.state == UI.ELoadingState.Ready;//Ready
                    isReady = isReady && !Engine.Game.CurrentRoom.GetPlayerForId(each.Key).isDced;//Dced
                }

                return isReady;
            }
        }
        #endregion
    }
}