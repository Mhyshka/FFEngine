using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    internal class MessageDidSmash : MessageData
    {
        #region Properties
        internal int smashCount = 0;
        #endregion

        internal override EDataType Type
        {
            get
            {
                return EDataType.M_DidSmash;
            }
        }

        internal MessageDidSmash() : this(0)
        {
        }

        internal MessageDidSmash(int a_smashCount)
        {
            smashCount = a_smashCount;
        }

        #region Serialization
        public override void LoadFromData(FFByteReader stream)
        {
            smashCount = stream.TryReadInt();
        }

        public override void SerializeData(FFByteWriter stream)
        {
            stream.Write(smashCount);
        }
        #endregion
    }
}