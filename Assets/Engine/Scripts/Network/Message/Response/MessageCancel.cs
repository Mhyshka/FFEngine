using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    internal class MessageCancel : MessageData
    {
        #region properties
        internal int requestId = 0;

        internal override EDataType Type
        {
            get
            {
                return EDataType.M_Cancel;
            }
        }
        #endregion

        #region Constructors
        internal MessageCancel(int a_requestId)
        {
            requestId = a_requestId;
        }
        #endregion

        #region Serialization
        public override void SerializeData(FFByteWriter stream)
        {
            base.SerializeData(stream);
            stream.Write(requestId);
        }

        public override void LoadFromData(FFByteReader stream)
        {
            base.LoadFromData(stream);
            requestId = stream.TryReadInt();
        }
        #endregion
    }
}