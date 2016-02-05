using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    internal class MessageEmptyData : MessageData
    {
        public MessageEmptyData()
        {
        }

        #region Types
        internal override EDataType Type
        {
            get
            {
                return EDataType.Empty;
            }
        }
        #endregion
    }
}