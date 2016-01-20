using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    internal class RequestEmpty : ARequest
    {
        #region Properties
        internal override EMessageType Type
        {
            get
            {
                return EMessageType.RequestEmpty;
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