using UnityEngine;
using System.Collections;
using System;

using FF.Network.Message;

namespace FF.Network.Receiver
{
    internal class NetworkIdReceiver : BaseMessageReceiver
    {
        #region Properties
        #endregion

        protected override void HandleMessage()
        {
            if (_message.HeaderType == EHeaderType.Message)
            {
                if (_message.Data.Type == EDataType.Integer)
                {
                    MessageIntegerData networkId = _message.Data as MessageIntegerData;
                    _client.NetworkID = networkId.Data;
                    Engine.Game.CurrentRoom.OnNetworkIdReceived(_client);
                    Engine.Network.Server.OnIdReceived(_client);
                }
            }
        }
    }
}