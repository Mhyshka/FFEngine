using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Receiver
{
    internal class MessageHeartbeat : AReceiver<Message.MessageHeartBeat>
    {
        #region Properties
        #endregion

        protected override void HandleMessage()
        {
            long spanTick = DateTime.Now.Ticks - _message.timeSent;
			TimeSpan span = TimeSpan.FromTicks(spanTick);
			FFLog.Log(EDbgCat.Networking, "Heartbeat received : " + span.TotalMilliseconds.ToString("0.") + "ms");
        }
    }
}