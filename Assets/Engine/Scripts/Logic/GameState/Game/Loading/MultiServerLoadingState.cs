using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using FF.Handler;
using FF.Network;
using FF.Network.Message;
using FF.Multiplayer;
using FF.UI;

using FF.Pong;


namespace FF.Logic
{
    internal class MultiServerLoadingState : PongLoadingState
    {
        #region Properties
        protected SentBroadcastRequest _networkCheck;

        protected ManualLoadingStep _allPlayerReadyStep;
        #endregion

        #region State Methods
        internal override void Enter()
        {
            base.Enter();
            _networkCheck = null;
        }

        internal override int Manage()
        {
            if (_networkCheck == null && AllPlayersReady)
            {
                _networkCheck = new SentBroadcastRequest(new MessageEmptyData(),
                                                        EMessageChannel.IsAlive.ToString(),
                                                        Engine.Network.NextRequestId,
                                                        true,
                                                        true,
                                                        2f);
                _networkCheck.onResult += OnNetworkCheckResult;
                _networkCheck.Broadcast();
            }
            return base.Manage();
        }
        #endregion

        #region Events
        protected override void RegisterForEvent()
        {
            base.RegisterForEvent();
            
            Engine.Game.Loading.onLoadingCompleteReceived += OnLoadingCompleteReceived;

            Engine.Events.RegisterForEvent("Kick", OnKickPlayerPressed);
        }

        protected override void UnregisterForEvent()
        {
            base.UnregisterForEvent();
            Engine.Game.Loading.UnregisterLoadingComplete();
            
            Engine.Game.Loading.onLoadingCompleteReceived -= OnLoadingCompleteReceived;

            Engine.Events.UnregisterForEvent("Kick", OnKickPlayerPressed);
        }

        #region Kick
        protected int _kickConfirmPopup = -1;
        protected FFNetworkPlayer _kickTarget = null;
        protected void OnKickPlayerPressed(FFEventParameter a_args)
        {
            _kickTarget = a_args.data as FFNetworkPlayer;
            if (_kickTarget != null)
            {
                _kickConfirmPopup = FFYesNoPopup.RequestDisplay("Kick " + _kickTarget.player.username,
                                            "Kick", "Keep", OnKickConfirm, OnKickCancel);
            }
        }

        protected void OnKickConfirm()
        {
            Engine.Network.Server.RemoveLostClient(_kickTarget.ID);
            Engine.Game.CurrentRoom.RemovePlayer(_kickTarget.ID);
            Engine.UI.DismissPopup(_kickConfirmPopup);
        }

        protected void OnKickCancel()
        {
            Engine.UI.DismissPopup(_kickConfirmPopup);
        }
        #endregion

        protected void OnLoadingCompleteReceived()
        {
            OnRoomUpdate(Engine.Game.CurrentRoom);
        }

        protected override void OnLoadingComplete()
        {
            base.OnLoadingComplete();
            SentMessage message = new SentMessage(new MessageEmptyData(),
                                                 EMessageChannel.LoadingComplete.ToString(),
                                                 true,
                                                 true);
            Engine.Network.Server.LoopbackClient.Mirror.QueueMessage(message);
        }
        #endregion

        protected void OnNetworkCheckResult(Dictionary<FFTcpClient, ReadResponse> a_success, Dictionary<FFTcpClient, ReadResponse> a_failures)
        {
            if (a_failures.Count > 0)
            {
                OnNetworkCheckFailed();
            }
            else
            {
                OnNetworkCheckSuccess();
            }
        }

        protected void OnNetworkCheckSuccess()
        {
            _allPlayerReadyStep.SetComplete();
            SentMessage message = new SentMessage(new MessageEmptyData(),
                                                    EMessageChannel.LoadingComplete.ToString(),
                                                    true,
                                                    false);
            Engine.Network.Server.BroadcastMessage(message);
            RequestState(outState.ID);
        }

        protected void OnNetworkCheckFailed()
        {
            _networkCheck = null;
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
                //Debug.LogError("Count : " + loadingWrappers.Count);
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