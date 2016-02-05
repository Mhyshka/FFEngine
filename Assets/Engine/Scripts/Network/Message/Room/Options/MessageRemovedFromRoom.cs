using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    internal class MessageBoolData : MessageData
    {
        internal bool _value = false;
        internal bool Data
        {
            get
            {
                return _value;
            }
        }

        internal override EDataType Type
        {
            get
            {
                return EDataType.Bool;
            }
        }

        public MessageBoolData()
        {
        }

        internal MessageBoolData(bool a_isBan)
        {
            _value = a_isBan;
        }

        #region Serialization
        public override void LoadFromData(FFByteReader stream)
        {
            _value = stream.TryReadBool();
        }

        public override void SerializeData(FFByteWriter stream)
        {
            stream.Write(_value);
        }
        #endregion
    }
}
