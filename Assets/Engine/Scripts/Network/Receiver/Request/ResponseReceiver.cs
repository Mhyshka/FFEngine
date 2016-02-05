using UnityEngine;
using System.Collections;
using System;

using FF.Network.Message;

namespace FF.Network.Receiver
{
    internal class ResponseReceiver : BaseMessageReceiver
    {
        protected override void HandleMessage()
        {
            ReadResponse response = _message as ReadResponse;
            FFLog.Log(EDbgCat.Receiver, "Reading Response");
            SentRequest req = _client.SentRequestForId(response.RequestId);
            if (req != null)
            {
                if (response.ErrorCode == ERequestErrorCode.Success)
                {
                    req.OnSuccess(response);
                }
                else
                {
                    req.OnFail(response.ErrorCode, response);
                }
            }
        }
    }
}