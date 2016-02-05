using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    internal class MessageFloatData : MessageData
    {
        #region Properties
        internal float _data;
        internal float Data
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
                return EDataType.Float;
            }
        }
        #endregion

        #region Constructors
        public MessageFloatData()
        {
            _data = 0f;
        }

        internal MessageFloatData(float a_value)
        {
            _data = a_value;
        }

        #endregion

        #region
        public override void LoadFromData(FFByteReader stream)
        {
            _data = stream.TryReadFloat();
        }

        public override void SerializeData(FFByteWriter stream)
        {
            stream.Write(_data);
        }
        #endregion
    }
}