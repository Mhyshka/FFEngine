using UnityEngine;
using System.Collections;
using System;

using FF.Network;
using FF.Network.Message;

namespace FF.Handler
{
    internal class Farewell : ASimpleSuccess
    {
        #region Properties
        protected int _count = 0;
        protected int _target = 0;

        protected MessageFarewell _message;
        #endregion

        internal Farewell(SimpleCallback a_onShutdownComplete) : base()
        {
            _onSuccess = a_onShutdownComplete;
            _message = new MessageFarewell("Server shuting down.");
            _message.onMessageSent = OnPostWrite;
            _target = Engine.Network.Server.BroadcastMessage(_message);
        }

        internal void OnPostWrite()
        {
            _count++;
            _isComplete = _count >= _target;
        }

        internal override void OnComplete()
        {
            if (_onSuccess != null)
                _onSuccess();

            _message.onMessageSent -= OnPostWrite;

            base.OnComplete();
        }
    }
}
