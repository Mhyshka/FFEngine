using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using FF.Network;
using FF.Network.Message;
using FF.Network.Receiver;
using FF.Multiplayer;
using System;

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
            Engine.Receiver.RegisterReceiver(Network.Message.EHeaderType.LoadingComplete, _loadingCompleteReceiver);
            Engine.Receiver.RegisterReceiver(Network.Message.EHeaderType.LoadingReady, _loadingReadyReceiver);
        }

        internal void UnregisterLoadingComplete()
        {
            _playersLoadingState.TearDown();
            Engine.Receiver.UnregisterReceiver(Network.Message.EHeaderType.LoadingComplete, _loadingCompleteReceiver);
            Engine.Receiver.UnregisterReceiver(Network.Message.EHeaderType.LoadingReady, _loadingReadyReceiver);
            _loadingCompleteReceiver = null;
        }

        internal void OnLoadingCompleteReceived(FFTcpClient a_client)
        {
            _finishedCount++;
            _playersLoadingState[a_client.NetworkID].state = UI.ELoadingState.NotReady;
            _playersLoadingState[a_client.NetworkID].rank = _finishedCount;

            Network.Message.MessageLoadingProgressData loadingProgressMessage = new Network.Message.MessageLoadingProgressData(_playersLoadingState);
            Engine.Network.Server.BroadcastMessage(loadingProgressMessage);

            if (onLoadingCompleteReceived != null)
                onLoadingCompleteReceived();
        }

        internal void OnPlayerReadyReceived(FFTcpClient a_client)
        {
            _playersLoadingState[a_client.NetworkID].state = UI.ELoadingState.Ready;

            Network.Message.MessageLoadingProgressData loadingProgressMessage = new Network.Message.MessageLoadingProgressData(_playersLoadingState);
            Engine.Network.Server.BroadcastMessage(loadingProgressMessage);

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
            Engine.Receiver.RegisterReceiver(EHeaderType.LoadingProgress, _loadingProgressReceiver);
        }

        internal void UnregisterLoadingStarted()
        {
            _playersLoadingState.TearDown();
            Engine.Receiver.UnregisterReceiver(EHeaderType.LoadingProgress, _loadingProgressReceiver);
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