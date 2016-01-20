using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Receiver
{
    internal class MessageLoadingReady : AReceiver<Message.MessageLoadingReady>
    {
        #region Properties
        #endregion

        protected override void HandleMessage()
        {
            Engine.Game.Loading.OnPlayerReadyReceived(_client);
        }
    }
}