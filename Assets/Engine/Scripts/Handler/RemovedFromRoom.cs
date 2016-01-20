using UnityEngine;
using System.Collections;

using FF.Network;
using FF.Network.Message;

namespace FF.Handler
{
    internal class RemovedFromRoom : ASimpleSuccess
    {
        internal RemovedFromRoom(FFTcpClient a_client, bool a_isBan, SimpleCallback a_onMessageSent)
        {
            _onSuccess = a_onMessageSent;

            MessageRemovedFromRoom message = new MessageRemovedFromRoom(a_isBan);
            message.onMessageSent += OnSuccess;
            a_client.QueueMessage(message);
        }
    }
}