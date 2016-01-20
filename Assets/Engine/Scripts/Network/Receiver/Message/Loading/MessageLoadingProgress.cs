using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Receiver
{
    internal class MessageLoadingProgress : AReceiver<Message.MessageLoadingProgress>
    {
        #region Properties
        PlayerDictionaryLoadingCallback _onReceived;
        #endregion

        internal MessageLoadingProgress(PlayerDictionaryLoadingCallback a_onReceived)
        {
            _onReceived = a_onReceived;
        }

        protected override void HandleMessage()
        {
            if (_onReceived != null)
                _onReceived(_message.PlayersLoadingState);
        }
    }
}