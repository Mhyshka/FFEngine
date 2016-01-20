using UnityEngine;
using System.Collections;

using FF.Network.Message;

namespace FF.Network.Receiver
{
    internal abstract class AReceiver<T> : BaseReceiver where T : AMessage
    {
        #region Properties
        protected FFTcpClient _client;
        protected T _message;
        #endregion

        internal sealed override void Read(AMessage a_message)
        {
            _client = a_message.Client;
            _message = a_message as T;
            if (_message != null)
                HandleMessage();
        }

        protected abstract void HandleMessage();
    }

    internal abstract class APendingReceiver<T> : BaseReceiver where T : BaseReceiver, new()
    {
        #region Properties
        #endregion

        internal sealed override void Read(AMessage a_message)
        {
            T instance = new T();
            instance.Read(a_message);
        }
    }

    internal abstract class BaseReceiver
    {
        internal abstract void Read(AMessage a_message);
    }
}
