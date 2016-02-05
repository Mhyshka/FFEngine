using UnityEngine;
using System.Collections;
using System;

using FF.Network.Message;

namespace FF.Network.Receiver
{
    internal class LoadingCompleteReceiver : BaseMessageReceiver
    {
        #region Properties
        #endregion

        protected override void HandleMessage()
        {
            Engine.Game.Loading.OnLoadingCompleteReceived(_client);
        }
    }
}