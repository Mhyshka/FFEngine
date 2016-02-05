using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    internal class MessageRequestGameMode : MessageData
    {
        internal override EDataType Type
        {
            get
            {
                return EDataType.M_RequestGameMode;
            }
        }
    }
}
