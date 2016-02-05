using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    internal class MessageLeavingRoom : MessageData
    {
        #region Properties
        #endregion

        internal override EDataType Type
        {
            get
            {
                return EDataType.M_LeavingRoom;
            }
        }
    }
}