using UnityEngine;
using System.Collections;
using System;

using FF.Network.Message;

namespace FF.Network.Receiver
{
    internal class ResponseFail : AReceiver<Message.ResponseFail>
    {
        protected override void HandleMessage()
        {
            FFLog.Log(EDbgCat.Receiver, "Reading Response Success");
            ARequest req = _client.SentRequestForId(_message.requestId);
            if (req != null)
            {
                if (req.onFail != null)
                    req.onFail(_message.errorCode);

                _client.RemoveSentRequest(req.requestId);
            }
        }
    }
}