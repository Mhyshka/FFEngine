using UnityEngine;
using System.Collections;

using FF.Network.Message;

namespace FF.Handler
{
    internal class SentMessageForClient : ABaseHandler
    {
        #region Properties
        internal MessageSentCallback onMessageSent = null;
        protected SentMessage _message = null;
        #endregion

        internal SentMessageForClient(SentMessage a_message)
        {
            _message = a_message;
            _message.onMessageSent += OnMessageSent;
        }

        protected virtual void OnMessageSent()
        {
            if (onMessageSent != null)
                onMessageSent(_message);
            Complete();
        }

        internal override void Complete()
        {
            base.Complete();

            _message.onMessageSent -= OnMessageSent;

            onMessageSent = null;
        }
    }
}