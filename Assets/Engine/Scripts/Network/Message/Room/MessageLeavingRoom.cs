using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    internal class MessageLeavingRoom : AMessage
    {
        #region Properties
        #endregion

        internal override EMessageType Type
        {
            get
            {
                return EMessageType.LeavingRoom;
            }
        }

        public MessageLeavingRoom()
        {
        }

        internal override bool IsMandatory
        {
            get
            {
                return false;
            }
        }

        #region Serialization
        public override void SerializeData(FFByteWriter stream)
        {
        }

        public override void LoadFromData(FFByteReader stream)
        {
        }
        #endregion
    }
}