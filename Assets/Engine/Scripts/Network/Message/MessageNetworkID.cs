using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    internal class MessageNetworkID : AMessage
    {
        internal int id;

        internal override EMessageType Type
        {
            get
            {
                return EMessageType.NetworkID;
            }
        }

        public MessageNetworkID()
        {
            id = -1;
        }

        internal MessageNetworkID(int a_id)
        {
            id = a_id;
        }

        #region Serialization
        public override void LoadFromData(FFByteReader stream)
        {
            id = stream.TryReadInt();
        }

        public override void SerializeData(FFByteWriter stream)
        {
            stream.Write(id);
        }
        #endregion
    }
}