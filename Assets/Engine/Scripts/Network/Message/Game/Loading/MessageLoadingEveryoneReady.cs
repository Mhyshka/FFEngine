using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    internal class MessageLoadingEveryoneReady : MessageData
    {
        internal override EDataType Type
        {
            get
            {
                return EDataType.M_LoadingEveryoneReady;
            }
        }
    }
}
