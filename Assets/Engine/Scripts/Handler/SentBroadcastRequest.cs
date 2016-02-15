using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using FF.Network;
using FF.Network.Message;

namespace FF.Handler
{
    internal class SentBroadcastRequest : ABaseHandler
    {

        #region Callbacks
        internal FFClientCallback onMessageSent = null;
        internal SimpleCallback onEveryMessageSent = null;

        internal FFRequestForClientCallback onSuccessForClient = null;
        internal FFRequestForClientCallback onFailureForClient = null;

        /// <summary>
        /// Called when a response for every request has been received. ( Or timeout )
        /// </summary>
        internal FFClientsBroadcastCallback onResult = null;
        #endregion

        protected Dictionary<FFTcpClient, SentRequest> _messageForClients;
        protected Dictionary<FFTcpClient, bool> _sentForClients;
        protected int _messageSentCount = 0;

        protected Dictionary<FFTcpClient, ReadResponse> _success;
        protected Dictionary<FFTcpClient, ReadResponse> _failures;

        internal SentBroadcastRequest(MessageData a_data,
                                string a_channel,
                                long a_requestId,
                                bool a_isMandatory = true,
                                bool a_isHandleByMock = false,
                                float a_timeoutDuration = 5f)
        {
            _messageSentCount = 0;
            _sentForClients = new Dictionary<FFTcpClient, bool>();
            _messageForClients = new Dictionary<FFTcpClient, SentRequest>();
            _success = new Dictionary<FFTcpClient, ReadResponse>();
            _failures = new Dictionary<FFTcpClient, ReadResponse>();

            foreach (FFTcpClient each in Engine.Network.Server.Clients.Values)
            {
                if (each != null)
                {
                    if (a_isHandleByMock || each != Engine.Network.Server.LoopbackClient)
                    {
                        SentRequest request = new SentRequest(a_data,
                                                               a_channel,
                                                               a_requestId,
                                                               a_timeoutDuration,
                                                               a_isMandatory,
                                                               a_isHandleByMock);
                        SentRequestForClient messageForClient = new SentRequestForClient(request);
                        messageForClient.onMessageSent += OnMessageSent;
                        messageForClient.onSuccess += OnSuccess;
                        messageForClient.onFail += OnFailure;
                        _sentForClients.Add(each, false);
                        _messageForClients.Add(each, request);
                    }
                }
            }
        }

        internal virtual void Broadcast()
        {
            if (_messageForClients.Count == 0)
            {
                if(onEveryMessageSent != null)
                    onEveryMessageSent();

                if (onResult != null)
                    onResult(_success, _failures);

                Complete();
            }

            foreach (KeyValuePair<FFTcpClient, SentRequest> pair in _messageForClients)
            {
                pair.Key.QueueRequest(pair.Value);
            }
        }

        protected void OnMessageSent(SentMessage a_message)
        {
            _sentForClients[a_message.Client] = true;
            if (onMessageSent != null)
            {
                onMessageSent(a_message.Client);
            }

            _messageSentCount++;
            if (_messageSentCount == _sentForClients.Count)
            {
                if (onEveryMessageSent != null)
                    onEveryMessageSent();
            }
        }

        protected void OnSuccess(ReadResponse a_response, SentMessage a_message)
        {
            _success.Add(a_message.Client, a_response);

            if (onSuccessForClient != null)
                onSuccessForClient(a_message.Client, a_response);

            if (_success.Count + _failures.Count == _messageForClients.Count)
            {
                if(onResult != null)
                    onResult(_success, _failures);
                Complete();
            }
        }

        protected void OnFailure(ERequestErrorCode a_errCode, ReadResponse a_response, SentMessage a_message)
        {
            _failures.Add(a_message.Client, a_response);

            if (onFailureForClient != null)
                onFailureForClient(a_message.Client, a_response);

            if (_success.Count + _failures.Count == _messageForClients.Count)
            {
                if (onResult != null)
                    onResult(_success, _failures);
                Complete();
            }
        }

        internal override void Complete()
        {
            base.Complete();
            onMessageSent = null;
            onEveryMessageSent = null;
            onSuccessForClient = null;
            onFailureForClient = null;
            onResult = null;

            /*_messageForClients.Clear();
            _sentForClients.Clear();
            _success.Clear();
            _failures.Clear();*/
        }
    }
}
