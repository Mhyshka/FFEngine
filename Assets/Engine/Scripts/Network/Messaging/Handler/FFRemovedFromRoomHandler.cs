using UnityEngine;
using System.Collections;

namespace FF.Networking
{
    internal class FFRemovedFromRoomHandler : FFHandler
    {
        internal FFRemovedFromRoomHandler(FFTcpClient a_client, bool a_isBan, SimpleCallback a_onMessageSent)
        {
            FFMessageRemovedFromRoom message = new FFMessageRemovedFromRoom(a_isBan);
            message.onMessageSent += OnSuccess;
            a_client.QueueMessage(message);
        }
    }
}