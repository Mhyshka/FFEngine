using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Receiver
{
    internal class MessageRequestGameMode : AReceiver<Message.MessageRequestGameMode>
    {
        #region Properties
        protected SimpleCallback _onReceived;
        #endregion

        internal MessageRequestGameMode(SimpleCallback a_onMessageReceived)
        {
            _onReceived = a_onMessageReceived;
        }

        protected override void HandleMessage()
        {
            if (_onReceived != null)
                _onReceived();
        }
    }
}