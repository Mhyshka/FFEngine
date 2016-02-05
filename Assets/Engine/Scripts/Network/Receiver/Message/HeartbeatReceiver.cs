using UnityEngine;
using System.Collections;
using System;

using FF.Network.Message;

namespace FF.Network.Receiver
{
    internal class HeartbeatReceiver : BaseMessageReceiver
    {
        #region Properties
        #endregion

        protected override void HandleMessage()
        {
            if (_message.HeaderType == EHeaderType.Request)
            {
                ReadRequest request = _message as ReadRequest;
                MessageLongData data = new MessageLongData(_message.Timestamp);
                SentResponse response = new SentResponse(data,
                                                         _message.Channel,
                                                         request.RequestId,
                                                         ERequestErrorCode.Success);

                _client.QueueResponse(response);
            }
            
        }
    }
}