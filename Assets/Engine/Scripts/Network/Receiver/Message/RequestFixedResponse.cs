using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Receiver
{
    internal class RequestFixedResponse : AReceiver<Message.ARequest>
    {
        #region Properties
        protected Message.AResponse _response;
        #endregion

        internal RequestFixedResponse(Message.AResponse a_response)
        {
            _response = a_response;
        }

        protected override void HandleMessage()
        {
            _response.requestId = _message.requestId;
            _message.Client.QueueMessage(_response);
        }
    }
}