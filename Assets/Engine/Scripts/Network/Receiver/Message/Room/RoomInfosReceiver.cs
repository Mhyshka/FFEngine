using UnityEngine;
using System.Collections;
using System;

using FF.Network.Message;

namespace FF.Network.Receiver
{
    internal class RoomInfosReceiver : BaseMessageReceiver
    {
        #region Properties
        #endregion

        protected override void HandleMessage()
        {
            if (_message.Data.Type == EDataType.Room)
            {
                MessageRoomData data = _message.Data as MessageRoomData;
                FFLog.Log(EDbgCat.Receiver, "Room Infos received");
                data.Room.serverEndPoint = _client.Remote;
                Engine.Network.OnRoomInfosReceived(data.Room);
            }
        }
    }
}