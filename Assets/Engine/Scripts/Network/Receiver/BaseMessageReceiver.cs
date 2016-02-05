using UnityEngine;
using System.Collections;

using FF.Network.Message;
using System;

namespace FF.Network.Receiver
{
    internal abstract class BaseMessageReceiver : BaseReceiver
    {
        #region Properties
        protected FFTcpClient _client;
        protected ReadMessage _message;
        #endregion

        internal sealed override void Read(ReadMessage a_message)
        {
            _message = a_message;
            _client = a_message.Client;
            HandleMessage();
        }

        protected abstract void HandleMessage();
    }

    internal class GenericMessageReceiver : BaseMessageReceiver
    {
        internal delegate void GenericMessageCallback(ReadMessage a_message);
        internal delegate void GenericRequestCallback(ReadRequest a_request);

        #region Callbacks
        internal GenericMessageCallback onMessageReceived = null;
        internal GenericRequestCallback onRequestReceived = null;
        #endregion

        #region Constructor
        protected GenericMessageReceiver()
        {
        }

        internal GenericMessageReceiver(GenericMessageCallback a_onMessageReceived)
        {
            onMessageReceived = a_onMessageReceived;
        }

        /*internal GenericMessageReceiver(GenericRequestCallback a_onRequestReceived)
        {
            onRequestReceived = a_onRequestReceived;
        }*/

        internal GenericMessageReceiver(GenericMessageCallback a_onMessageReceived, GenericRequestCallback a_onRequestReceived)
        {
            onMessageReceived = a_onMessageReceived;
            onRequestReceived = a_onRequestReceived;
        }
        #endregion

        protected override void HandleMessage()
        {
            if (_message.HeaderType == EHeaderType.Message)
            {
                if (onMessageReceived != null)
                    onMessageReceived(_message);
            }
            else if (_message.HeaderType == EHeaderType.Request)
            {
                if(onRequestReceived != null)
                    onRequestReceived(_message as ReadRequest);
            }
        }
    }
}
