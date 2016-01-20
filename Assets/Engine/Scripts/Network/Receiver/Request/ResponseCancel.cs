using UnityEngine;
using System.Collections;
using System;

using FF.Network.Message; 

namespace FF.Network.Receiver
{
    internal class ResponseCancel : AReceiver<Message.ResponseCancel>
    {
        protected override void HandleMessage()
        {
            FFLog.Log(EDbgCat.Receiver, "Reading Response Cancel");
            ARequest request = _client.SentRequestForId(_message.requestId);
            if (request != null)
            {
                request.Cancel(false);
            }
        }
    }
}