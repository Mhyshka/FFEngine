using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    internal class MessageRacketHit : MessageData
    {
        #region Properties
        internal int racketId = 0;
        #endregion

        internal override EDataType Type
        {
            get
            {
                return EDataType.M_RacketHit;
            }
        }

        internal MessageRacketHit() : this(0)
        {
        }

        internal MessageRacketHit(int a_racketId)
        {
            racketId = a_racketId;
        }

        #region Serialization
        public override void LoadFromData(FFByteReader stream)
        {
            racketId = stream.TryReadInt();
        }

        public override void SerializeData(FFByteWriter stream)
        {
            stream.Write(racketId);
        }
        #endregion
    }
}