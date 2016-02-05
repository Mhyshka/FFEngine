using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    internal class MessagePongTargetRatio : MessageData
    {
        #region Properties
        internal float ratio;
        internal int clientId;

        internal override EDataType Type
        {
            get
            {
                return EDataType.M_PongTargetRatio;
            }
        }
        #endregion

        #region Constructors
        public MessagePongTargetRatio()
        {
            ratio = 0f;
            clientId = 0;
        }

        internal MessagePongTargetRatio(float a_ratio, int a_clientId)
        {
            ratio = a_ratio;
            clientId = a_clientId;
        }

        #endregion

        #region
        public override void LoadFromData(FFByteReader stream)
        {
            ratio = stream.TryReadFloat();
            clientId = stream.TryReadInt();
        }

        public override void SerializeData(FFByteWriter stream)
        {
            stream.Write(ratio);
            stream.Write(clientId);
        }
        #endregion
    }
}