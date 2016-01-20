using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Receiver
{
    internal class GenericReceiver<T> : AReceiver<T> where T : Message.AMessage
    {
        internal delegate void OnMessageReceived(T a_message);
        internal OnMessageReceived onMessageReceived = null;

        internal GenericReceiver(OnMessageReceived a_callback)
        {
            onMessageReceived = a_callback;
        }

        protected override void HandleMessage()
        {
            onMessageReceived(_message);
        }
    }
}
