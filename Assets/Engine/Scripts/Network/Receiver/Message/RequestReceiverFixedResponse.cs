using UnityEngine;
using System.Collections;
using System;

using FF.Network.Message;

namespace FF.Network.Receiver
{
    internal class RequestReceiverFixedResponse : BaseMessageReceiver
    {
        #region Properties
        protected ERequestErrorCode _errorCode;
        protected MessageData _data;
        #endregion

        internal RequestReceiverFixedResponse(ERequestErrorCode a_errorCode, MessageData a_data)
        {
            _errorCode = a_errorCode;
            _data = a_data;
        }

        protected override void HandleMessage()
        {
            if (_message.HeaderType == EHeaderType.Request)
            {
                ReadRequest request = _message as ReadRequest;
                SentResponse response = new SentResponse(_data,
                                                        request.Channel,
                                                        request.RequestId,
                                                        _errorCode);
                _client.QueueResponse(response);
            }
        }
    }
}