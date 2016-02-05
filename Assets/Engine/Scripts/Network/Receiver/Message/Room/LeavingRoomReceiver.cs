using UnityEngine;
using System;

using FF.Network.Message;

namespace FF.Network.Receiver
{
    internal class LeavingRoomReceiver : BaseMessageReceiver
    {
        #region Properties
        #endregion

        protected override void HandleMessage()
        {
            FFLog.Log(EDbgCat.Receiver, "Reading leaving room message");
            Engine.Game.CurrentRoom.RemovePlayer(_client.NetworkID);
            _client.EndConnection("Leaving room");
        }
    }
}