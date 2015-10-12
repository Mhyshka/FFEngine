using UnityEngine;
using System.Collections;
using System;

namespace FF.Networking
{
    internal class FFFarewellHandler : FFHandler
    {
        #region Properties
        protected int _count = 0;
        protected int _target = 0;

        protected FFMessageFarewell _message;
        #endregion

        internal FFFarewellHandler(SimpleCallback a_onShutdownComplete) : base()
        {
            _onSuccess = a_onShutdownComplete;
            _message = new FFMessageFarewell("Server shuting down.");
            _message.onMessageSent = OnPostWrite;
            _target = FFEngine.Network.Server.BroadcastMessage(_message);
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
