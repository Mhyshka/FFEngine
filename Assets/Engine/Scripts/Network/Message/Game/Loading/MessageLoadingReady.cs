using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    /// <summary>
    /// Sent when a client is ready during loading. ( After the loading is done ( complete ) )
    /// </summary>
    internal class MessageLoadingReady : MessageData
    {
        internal override EDataType Type
        {
            get
            {
                return EDataType.M_LoadingReady;
            }
        }
    }
}
