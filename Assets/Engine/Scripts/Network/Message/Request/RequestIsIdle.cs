using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    internal class RequestIsIdle : ARequest
    {
        #region Properties
        internal override EMessageType Type
        {
            get
            {
                return EMessageType.IsIdleRequest;
            }
        }

        internal override bool HandleByMock
        {
            get
            {
                return true;
            }
        }
        #endregion
    }
}