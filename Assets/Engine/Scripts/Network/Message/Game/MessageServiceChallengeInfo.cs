using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    internal class MessageServiceChallengeInfo : MessageData
    {
        #region Properties
        internal int bounceCount = 0;
        #endregion

        internal override EDataType Type
        {
            get
            {
                return EDataType.M_ServiceChallengeInfo;
            }
        }

        internal MessageServiceChallengeInfo() : this(0)
        {
        }

        internal MessageServiceChallengeInfo(int a_bounceCount)
        {
            bounceCount = a_bounceCount;
        }

        #region Serialization
        public override void LoadFromData(FFByteReader stream)
        {
            bounceCount = stream.TryReadInt();
        }

        public override void SerializeData(FFByteWriter stream)
        {
            stream.Write(bounceCount);
        }
        #endregion
    }
}