using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    internal class MessageIntegerData : MessageData
    {
        protected int _data;
        internal int Data
        {
            get
            {
                return _data;
            }
        }

        internal override EDataType Type
        {
            get
            {
                return EDataType.Integer;
            }
        }

        public MessageIntegerData()
        {
            _data = -1;
        }

        internal MessageIntegerData(int a_id)
        {
            _data = a_id;
        }

        #region Serialization
        public override void LoadFromData(FFByteReader stream)
        {
            _data = stream.TryReadInt();
        }

        public override void SerializeData(FFByteWriter stream)
        {
            stream.Write(_data);
        }
        #endregion
    }
}