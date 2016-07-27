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

        protected Dictionary<FFNetworkClient, SentMessage> _messageForClients;
        protected Dictionary<FFNetworkClient, bool> _sentForClients;
        protected int _messageSentCount = 0;

        protected float _timeoutDuration = 0f;
        protected float _timeElapsed = 0f;

        internal SentBroadcastMessage(List<int> a_ids,
                                    MessageData a_data,
                                    string a_channel,
                                    bool a_isMandatory = true,
                                    bool a_isHandleByMock = false,
                                    float a_timeoutDuration = 5f,
                                    long a_timestamp = -1L)
        {
            _timeElapsed = 0f;
            _timeoutDuration = a_timeoutDuration;
            _messageSentCount = 0;
            _sentForClients = new Dictionary<FFNetworkClient, bool>();
            _messageForClients = new Dictionary<FFNetworkClient, SentMessage>();

            foreach (int id in a_ids)
            {
                FFNetworkClient client = Engine.Network.GameServer.ClientForId(id);
                /*if (client != null && client.IsConnected)
                {*/
                if (a_isHandleByMock || client != Engine.Network.TcpServer.LoopbackClient)
                {
                    SentMessage message = new SentMessage(a_data,
                                                        a_channel,
                                                        a_isMandatory,
                                                        a_isHandleByMock);

                    if (a_timestamp != -1L)
                        message.Timestamp = a_timestamp;

                    SentMessageForClient messageForClient = new SentMessageForClient(message);
                    messageForClient.onMessageSent += OnMessageSent;

                    _sentForClients.Add(client, false);
                    _messageForClients.Add(client, message);
                }
               // }
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

            foreach (KeyValuePair<FFNetworkClient, SentMessage> pair in _messageForClients)
            {
                pair.Key.QueueMessage(pair.Value);
                if (!pair.Key.IsConnected)
                {
                    OnMessageSent(pair.Value);
                }
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
