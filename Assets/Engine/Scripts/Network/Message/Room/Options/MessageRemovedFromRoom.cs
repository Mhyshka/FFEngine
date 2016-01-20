using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    internal class MessageRemovedFromRoom : AMessage
    {
        internal bool isBan = false;

        internal override EMessageType Type
        {
            get
            {
                return EMessageType.RemovedFromRoom;
            }
        }

        public MessageRemovedFromRoom()
        {
        }

        internal MessageRemovedFromRoom(bool a_isBan)
        {
            isBan = a_isBan;
        }

        #region Serialization
        public override void LoadFromData(FFByteReader stream)
        {
            isBan = stream.TryReadBool();
        }

        public override void SerializeData(FFByteWriter stream)
        {
            stream.Write(isBan);
        }
        #endregion
    }
}
