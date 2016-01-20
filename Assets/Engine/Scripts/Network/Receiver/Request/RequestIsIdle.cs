using UnityEngine;
using System.Collections;
using System;

using FF.Network.Message;

namespace FF.Network.Receiver
{
    internal class RequestIsIdle : AReceiver<Message.RequestIsIdle>
    {
        protected override void HandleMessage()
        {
            AResponse answer = null;
            if (!Engine.Game.NetPlayer.IsBusy)
            {
                answer = new Message.ResponseSuccess();
            }
            else
            {
                answer = new Message.ResponseFail(0);
            }
            answer.requestId = _message.requestId;
            _client.QueueMessage(answer);
        }
    }
}
