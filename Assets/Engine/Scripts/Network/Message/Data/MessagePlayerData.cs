using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;

using FF.Multiplayer;

namespace FF.Network.Message
{
	internal class MessagePlayerData : MessageData
	{
        internal enum EErrorCode
        {
            UserCanceled,
            ServerOnly,
            RoomIsFull,
            Banned
        }

        #region Properties
        protected FFNetworkPlayer _player = null;
        internal FFNetworkPlayer Player
        {
            get
            {
                return _player;
            }
        }

        internal override EDataType Type
        {
            get
            {
                return EDataType.Player;
            }
        }
        #endregion

        public MessagePlayerData()
        {
        }

        internal MessagePlayerData(FFNetworkPlayer a_player)
        {
            _player = a_player;
        }

        #region Methods
        #endregion

        #region Serialization
        public override void SerializeData(FFByteWriter stream)
        {
            base.SerializeData(stream);
            stream.Write(_player);
        }

        public override void LoadFromData(FFByteReader stream)
        {
            base.LoadFromData(stream);
            _player = stream.TryReadObject<FFNetworkPlayer>();
        }
        #endregion
    }
}