using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using FF.Network;
using FF.Network.Message;
using FF.Network.Receiver;
using FF.Multiplayer;
using FF.Handler;

namespace FF.Logic
{
    internal class PlayerLoadingWrapper : IByteStreamSerialized
    {
        internal UI.ELoadingState state = UI.ELoadingState.Loading;
        internal int rank = -1;

        public void SerializeData(FFByteWriter stream)
        {
            stream.Write((int)state);
            stream.Write(rank);
        }

        public void LoadFromData(FFByteReader stream)
        {
            state = (UI.ELoadingState)stream.TryReadInt();
            rank = stream.TryReadInt();
        }
    }

	/// <summary>
	/// Basic Game Manager that handles loading of GameModes.
	/// </summary>
	internal class NetworkLoadingManager : GameManager
	{
        #region Properties
        internal bool loadMultiplayerGameMode = false;

        protected PlayerDictionary<PlayerLoadingWrapper> _playersLoadingState = null;
        internal PlayerDictionary<PlayerLoadingWrapper> PlayersLoadingState
        {
            get
            {
                return _playersLoadingState;
            }
        }
        #endregion

        #region Loading Server
        internal SimpleCallback onLoadingCompleteReceived;
        protected LoadingCompleteReceiver _loadingCompleteReceiver;
        protected LoadingReadyReceiver _loadingReadyReceiver;
        protected int _finishedCount;

        internal void RegisterLoadingComplete()
        {
            _finishedCount = 0;
            _playersLoadingState = new PlayerDictionary<PlayerLoadingWrapper>();
            _loadingCompleteReceiver = new LoadingCompleteReceiver();
            _loadingReadyReceiver = new LoadingReadyReceiver();
            Engine.Receiver.RegisterReceiver(EMessageChannel.LoadingComplete.ToString(), _loadingCompleteReceiver);
            Engine.Receiver.RegisterReceiver(EMessageChannel.LoadingReady.ToString(), _loadingReadyReceiver);
        }

        internal void UnregisterLoadingComplete()
        {
            _playersLoadingState.TearDown();
            Engine.Receiver.UnregisterReceiver(EMessageChannel.LoadingComplete.ToString(), _loadingCompleteReceiver);
            Engine.Receiver.UnregisterReceiver(EMessageChannel.LoadingReady.ToString(), _loadingReadyReceiver);
            _loadingCompleteReceiver = null;
        }

        internal void OnLoadingCompleteReceived(FFNetworkClient a_client)
        {
            _finishedCount++;
            _playersLoadingState[a_client.NetworkID].state = UI.ELoadingState.NotReady;
            _playersLoadingState[a_client.NetworkID].rank = _finishedCount;

            MessageLoadingProgressData loadingProgressData = new MessageLoadingProgressData(_playersLoadingState);
            SentBroadcastMessage message = new SentBroadcastMessage(Engine.Network.CurrentRoom.GetPlayersIds(),
                                                                    loadingProgressData,
                                                                    EMessageChannel.LoadingProgress.ToString());
            message.Broadcast();

            if (onLoadingCompleteReceived != null)
                onLoadingCompleteReceived();
        }

        internal void OnPlayerReadyReceived(FFNetworkClient a_client)
        {
            _playersLoadingState[a_client.NetworkID].state = UI.ELoadingState.Ready;

            MessageLoadingProgressData loadingProgressData = new MessageLoadingProgressData(_playersLoadingState);
            SentBroadcastMessage message = new SentBroadcastMessage(Engine.Network.CurrentRoom.GetPlayersIds(),
                                                                    loadingProgressData,
                                                                    EMessageChannel.LoadingProgress.ToString());
            message.Broadcast();

            if (onLoadingCompleteReceived != null)
                onLoadingCompleteReceived();
        }
        #endregion

        #region Loading Client
        protected GenericMessageReceiver _loadingProgressReceiver;

        internal SimpleCallback onLoadingProgressReceived = null;
        internal void RegisterLoadingStarted()
        {
            _playersLoadingState = new PlayerDictionary<PlayerLoadingWrapper>();
            _loadingProgressReceiver = new GenericMessageReceiver(OnLoadingProgressReceived);
            Engine.Receiver.RegisterReceiver(EMessageChannel.LoadingProgress.ToString(), _loadingProgressReceiver);
        }

        internal void UnregisterLoadingStarted()
        {
            _playersLoadingState.TearDown();
            Engine.Receiver.UnregisterReceiver(EMessageChannel.LoadingProgress.ToString(), _loadingProgressReceiver);
            _loadingProgressReceiver = null;
        }

        internal void OnLoadingProgressReceived(ReadMessage a_message)
        {
            if (a_message.Data.Type == EDataType.LoadingProgress)
            {
                MessageLoadingProgressData loadingProgress = a_message.Data as MessageLoadingProgressData;
                _playersLoadingState = loadingProgress.PlayersLoadingState;

                if (onLoadingProgressReceived != null)
                    onLoadingProgressReceived();
            }
            
        }
        #endregion
    }
}