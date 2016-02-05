using UnityEngine;
using System.Collections;
using System;

using FF.Pong;

namespace FF.Network.Message
{
    internal class MessageGoalHit : MessageData
    {
        #region Properties
        internal ESide side = 0;
        #endregion

        internal override EDataType Type
        {
            get
            {
                return EDataType.M_GoalHit;
            }
        }

        internal MessageGoalHit() : this(ESide.None)
        {
        }

        internal MessageGoalHit(ESide a_side)
        {
            side = a_side;
        }

        #region Serialization
        public override void LoadFromData(FFByteReader stream)
        {
            side = (ESide)stream.TryReadInt() ;
        }

        public override void SerializeData(FFByteWriter stream)
        {
            stream.Write((int)side);
        }
        #endregion
    }
}