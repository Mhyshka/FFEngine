using UnityEngine;
using System.Collections;
using System;

using FF.Network.Message;

namespace FF.Network.Receiver
{
    internal class IsIdleReceiver : BaseMessageReceiver
    {
        protected override void HandleMessage()
        {
            ReadRequest request = _message as ReadRequest;
            SentResponse response;
            if (!Engine.Game.NetPlayer.IsBusy)
            {
                response = new SentResponse(new MessageEmptyData(),
                                            request.RequestId,
                                            ERequestErrorCode.Success);
            }
            else
            {
                response = new SentResponse(new MessageEmptyData(),
                                                request.RequestId,
                                                ERequestErrorCode.Failed);
            }
            _client.QueueResponse(response);
        }
    }
}
