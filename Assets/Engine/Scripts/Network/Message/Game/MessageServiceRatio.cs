using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    internal class MessageServiceRatio : MessageData
    {
        #region Properties
        internal float ratio = 0f;
        #endregion

        internal override EDataType Type
        {
            get
            {
                return EDataType.M_ServiceRatio;
            }
        }

        internal MessageServiceRatio() : this(0f)
        {
        }

        internal MessageServiceRatio(float a_ratio)
        {
            ratio = a_ratio;
        }

        #region Serialization
        public override void LoadFromData(FFByteReader stream)
        {
            ratio = stream.TryReadFloat();
        }

        public override void SerializeData(FFByteWriter stream)
        {
            stream.Write(ratio);
        }
        #endregion
    }
}