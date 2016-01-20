using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    internal class MessageLoadingComplete : AMessage
    {
        internal override EMessageType Type
        {
            get
            {
                return EMessageType.LoadingComplete;
            }
        }

        internal override bool HandleByMock
        {
            get
            {
                return true;
            }
        }

        #region Serialize
        public override void LoadFromData(FFByteReader stream)
        {
        }

        public override void SerializeData(FFByteWriter stream)
        {
        }
        #endregion
    }
}
