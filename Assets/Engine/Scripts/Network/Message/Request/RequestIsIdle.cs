using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    internal class RequestIsIdle : MessageData
    {
        #region Properties
        internal override EDataType Type
        {
            get
            {
                return EDataType.IsIdleRequest;
            }
        }
        #endregion
    }
}