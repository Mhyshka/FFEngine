using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Receiver
{
    internal class MessageLoadingComplete : AReceiver<Message.MessageLoadingComplete>
    {
        #region Properties
        #endregion

        protected override void HandleMessage()
        {
            Engine.Game.Loading.OnLoadingCompleteReceived(_client);
        }
    }
}