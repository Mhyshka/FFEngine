using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using FF.Network;
using FF.Network.Message;

namespace FF.Handler
{
    internal class SentBroadcastMessage : ABaseHandler
    {
        #region Callbacks
        internal FFClientCallback onMessageSent = null;
        internal SimpleCallback onEveryMessageSent = null;
        internal FFBoolByClientsCallback onTimeout = null;
        #endregion

        protected Dictionary<FFTcpClient, SentMessage> _messageForClients;
        protected Dictionary<FFTcpClient, bool> _sentForClients;
        protected int _messageSentCount = 0;

        protected float _timeoutDuration = 0f;
        protected float _timeElapsed = 0f;

        internal SentBroadcastMessage(MessageData a_data,
                                string a_channel,
                                bool a_isMandatory = true,
                                bool a_isHandleByMock = false,
                                float a_timeoutDuration = 5f)
        {
            _timeElapsed = 0f;
            _timeoutDuration = a_timeoutDuration;
            _messageSentCount = 0;
            _sentForClients = new Dictionary<FFTcpClient, bool>();
            _messageForClients = new Dictionary<FFTcpClient, SentMessage>();

            foreach (FFTcpClient each in Engine.Network.Server.Clients.Values)
            {
                if (each != null)
                {
                    if (a_isHandleByMock || each != Engine.Network.Server.LoopbackClient)
                    {
                        SentMessage message = new SentMessage(a_data,
                                                           a_channel,
                                                           a_isMandatory,
                                                           a_isHandleByMock);
                        message.onMessageSent += delegate { OnMessageSent(message); };
                        _sentForClients.Add(each, false);
                        _messageForClients.Add(each, message);
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

                Complete();
            }

            foreach (KeyValuePair<FFTcpClient, SentMessage> pair in _messageForClients)
            {
                pair.Key.QueueMessage(pair.Value);
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
                Complete();
            }
        }

        internal override void Complete()
        {
            base.Complete();
            onTimeout = null;
            onMessageSent = null;
            onEveryMessageSent = null;

            /*_messageForClients.Clear();
            _sentForClients.Clear();*/
        }

        #region timeout
        internal override void DoUpdate()
        {
            base.DoUpdate();
            _timeElapsed += Time.deltaTime;
            if (_timeElapsed >= _timeoutDuration)
            {
                OnTimeout();
            }
        }

        protected virtual void OnTimeout()
        {
            if (onTimeout != null)
            {
                onTimeout(_sentForClients);
            }

            Complete();
        }
        #endregion
    }
}
