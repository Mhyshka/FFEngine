using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    internal class MessageVersionData : MessageData
    {
        protected FFVersion _data;
        internal FFVersion Data
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
                return EDataType.Version;
            }
        }

        public MessageVersionData()
        {
            _data = null;
        }

        internal MessageVersionData(FFVersion a_data)
        {
            _data = a_data;
        }

        #region Serialization
        public override void LoadFromData(FFByteReader stream)
        {
            _data = stream.TryReadObject<FFVersion>();
        }

        public override void SerializeData(FFByteWriter stream)
        {
            stream.Write(_data);
        }
        #endregion
    }
}