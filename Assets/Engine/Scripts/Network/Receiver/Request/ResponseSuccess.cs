using UnityEngine;
using System.Collections;
using System;

using FF.Network.Message;

namespace FF.Network.Receiver
{
    internal class ResponseSuccess : AReceiver<Message.ResponseSuccess>
    {
        protected override void HandleMessage()
        {
            FFLog.Log(EDbgCat.Receiver, "Reading Response Success");
            ARequest req = _client.SentRequestForId(_message.requestId);
            if (req != null)
            {
                if (req.onSuccess != null)
                    req.onSuccess();

                _client.RemoveSentRequest(req.requestId);
            }
        }
    }
}