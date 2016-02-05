using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    internal abstract class MessageData : IByteStreamSerialized
    {
        public MessageData()
        {
        }

        #region Types
        internal abstract EDataType Type
        {
            get;
        }
        #endregion

        #region Serialization
        public virtual void LoadFromData(FFByteReader stream)
        {
            
        }

        public virtual void SerializeData(FFByteWriter stream)
        {
            
        }
        #endregion
    }
}