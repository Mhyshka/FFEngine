using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using FF.Multiplayer;
using FF.Logic;

namespace FF.Network.Message
{
    internal class MessageLoadingProgress : AMessage
    {
        #region Properties
        internal override EMessageType Type
        {
            get
            {
                return EMessageType.LoadingProgress;
            }
        }

        internal override bool HandleByMock
        {
            get
            {
                return false;
            }
        }

        protected PlayerDictionary<PlayerLoadingWrapper> _playersLoadingState;
        internal PlayerDictionary<PlayerLoadingWrapper> PlayersLoadingState
        {
            get
            {
                return _playersLoadingState;
            }
        }

        internal MessageLoadingProgress()
        {
        }

        internal MessageLoadingProgress(PlayerDictionary<PlayerLoadingWrapper> a_playersLoadingState)
        {
            _playersLoadingState = a_playersLoadingState;
        }
        #endregion

        #region Serialize
        public override void LoadFromData(FFByteReader stream)
        {
            _playersLoadingState = stream.TryReadPlayerDictionary<PlayerLoadingWrapper>();
        }

        public override void SerializeData(FFByteWriter stream)
        {
            stream.Write(_playersLoadingState);
        }
        #endregion
    }
}
