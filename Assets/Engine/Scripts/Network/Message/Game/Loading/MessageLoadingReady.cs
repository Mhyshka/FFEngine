using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    /// <summary>
    /// Sent when a client is ready during loading. ( After the loading is done ( complete ) )
    /// </summary>
    internal class MessageLoadingReady : AMessage
    {
        internal override EMessageType Type
        {
            get
            {
                return EMessageType.LoadingReady;
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
