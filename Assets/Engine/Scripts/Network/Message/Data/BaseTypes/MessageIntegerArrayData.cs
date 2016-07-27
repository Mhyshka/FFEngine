using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    internal class MessageIntegerArrayData : MessageData
    {
        protected int[] _data;
        internal int[] Data
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
                return EDataType.IntegerArray;
            }
        }

        public MessageIntegerArrayData()
        {
            _data = null;
        }

        internal MessageIntegerArrayData(int[] a_data)
        {
            _data = a_data;
        }

        #region Serialization
        public override void LoadFromData(FFByteReader stream)
        {
            _data = stream.TryReadIntArray();
        }

        public override void SerializeData(FFByteWriter stream)
        {
            stream.Write(_data);
        }
        #endregion
    }
}