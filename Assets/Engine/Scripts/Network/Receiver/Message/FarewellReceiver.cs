using UnityEngine;
using System;

using FF.Network.Message;

namespace FF.Network.Receiver
{
    internal enum EFarewellCode
    {
        Shuttingdown,

    }

    internal class FarewellReceiver : BaseMessageReceiver
    {
        #region Properties
        #endregion

        protected override void HandleMessage()
        {
            if (_message.Data.Type == EDataType.Integer)
            {
                MessageIntegerData data = _message.Data as MessageIntegerData;
                //TODO Localize
                EFarewellCode code = (EFarewellCode)data.Data;
                _client.EndConnection(code.ToString());
            }
        }
    }
}