using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Receiver
{
    internal class MessageRoomInfos : AReceiver<Message.MessageRoomInfos>
    {
        #region Properties
        #endregion

        protected override void HandleMessage()
        {
            FFLog.Log(EDbgCat.Receiver,"Room Infos received");
            _message.room.serverEndPoint = _client.Remote;
            Engine.Network.OnRoomInfosReceived(_message.room);
        }
    }
}