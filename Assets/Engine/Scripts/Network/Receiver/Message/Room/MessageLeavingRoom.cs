using UnityEngine;
using System;

namespace FF.Network.Receiver
{
    internal class MessageLeavingRoom : AReceiver<Message.MessageLeavingRoom>
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