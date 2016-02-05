using UnityEngine;
using System.Collections;
using System;

using FF.Network;
using FF.Network.Message;

namespace FF.Handler
{
    internal class Farewell : ABaseHandler
    {
        #region Properties
        protected int _count = 0;
        protected int _target = 0;

        protected StringMessageData _data;
        #endregion

        internal Farewell(SimpleCallback a_onShutdownComplete) : base()
        {
            _onSuccess = a_onShutdownComplete;
            _data = new StringMessageData("Server shuting down.");
            _data.onMessageSent = OnPostWrite;
            _target = Engine.Network.Server.BroadcastMessage(_data);
        }

        internal void OnPostWrite()
        {
            _count++;
            _isComplete = _count >= _target;
        }

        internal override void Complete()
        {
            if (_onSuccess != null)
                _onSuccess();

            _data.onMessageSent -= OnPostWrite;

            base.Complete();
        }
    }
}
