using UnityEngine;
using System;


namespace FF.Network.Receiver
{
    internal class MessageFarewell : AReceiver<Message.MessageFarewell>
    {
        #region Properties
        #endregion

        protected override void HandleMessage()
        {
            _client.EndConnection(_message.reason);
        }
    }
}