using UnityEngine;
using System.Collections;
using System;

using FF.Network.Message; 

namespace FF.Network.Receiver
{
    internal class CancelReceiver : BaseMessageReceiver
    {
        protected override void HandleMessage()
        {
            MessageLongData data = _message.Data as MessageLongData;
            FFLog.Log(EDbgCat.Receiver, "Reading Message Cancel");
            ReadRequest request = _client.ReadRequestForId(data.Data);
            if (request != null)
            {
                request.OnCanceled();
            }
        }
    }
}