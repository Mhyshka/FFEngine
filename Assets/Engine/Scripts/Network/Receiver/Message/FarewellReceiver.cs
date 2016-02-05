using UnityEngine;
using System;

using FF.Network.Message;

namespace FF.Network.Receiver
{
    internal class FarewellReceiver : BaseMessageReceiver
    {
        #region Properties
        #endregion

        protected override void HandleMessage()
        {
            if (_message.Data.Type == EDataType.String)
            {
                StringMessageData stringData = _message.Data as StringMessageData;
                //Reason
                _client.EndConnection(stringData.StringData);
            }
        }
    }
}