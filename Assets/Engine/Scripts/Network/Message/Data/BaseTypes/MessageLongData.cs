using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;

namespace FF.Network.Message
{
	internal class MessageLongData : MessageData
	{
        #region Properties
        internal override EDataType Type
        {
            get
            {
                return EDataType.Long;
            }
        }

        protected long _data = 0L;
        internal long Data
        {
            get
            {
                return _data;
            }
        }
        #endregion

        public MessageLongData()
        {
        }

        internal MessageLongData(long a_timestamp)
        {
            _data = a_timestamp;
        }

        #region Serialization
        public override void LoadFromData(FFByteReader stream)
        {
            _data = stream.TryReadLong();
        }

        public override void SerializeData(FFByteWriter stream)
        {
            stream.Write(_data);
        }
        #endregion
    }
}