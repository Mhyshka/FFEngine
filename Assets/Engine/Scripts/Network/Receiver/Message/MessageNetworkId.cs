using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Receiver
{
    internal class MessageNetworkId : AReceiver<Message.MessageNetworkID>
    {
        #region Properties
        #endregion

        protected override void HandleMessage()
        {
            FFLog.Log(EDbgCat.Receiver, "Reading Message Network Id");
            _client.NetworkID = _message.id;
			Engine.Game.CurrentRoom.OnNetworkIdReceived(_client);
            Engine.Network.Server.OnIdReceived(_client);
        }
    }
}