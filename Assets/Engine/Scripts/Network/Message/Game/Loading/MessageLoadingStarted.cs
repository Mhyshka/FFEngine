using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    internal class MessageLoadingStarted : AMessage
    {
        internal override EMessageType Type
        {
            get
            {
                return EMessageType.LoadingStarted;
            }
        }

        internal override bool HandleByMock
        {
            get
            {
                return false;
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
