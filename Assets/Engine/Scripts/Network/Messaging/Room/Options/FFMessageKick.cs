using UnityEngine;
using System.Collections;
using System;

namespace FF.Networking
{
    internal class FFMessageRemovedFromRoom : FFMessage
    {
        internal static SimpleCallback onKickReceived = null;
        internal static SimpleCallback onBanReceived = null;

        internal bool isBan = false;

        internal override EMessageType Type
        {
            get
            {
                return EMessageType.RemovedFromRoom;
            }
        }

        public FFMessageRemovedFromRoom()
        {
        }

        internal FFMessageRemovedFromRoom(bool a_isBan)
        {
            isBan = a_isBan;
        }

        internal override void Read(FFTcpClient a_tcpClient)
        {
            FFLog.Log(EDbgCat.Networking, "Removed From Room message received.");
            if (!isBan && onKickReceived != null)
            {
                onKickReceived();
            }
            else if (isBan && onBanReceived != null)
            {
                onBanReceived();
            }
        }

        #region Serialization
        public override void LoadFromData(FFByteReader stream)
        {
            isBan = stream.TryReadBool();
        }

        public override void SerializeData(FFByteWriter stream)
        {
            stream.Write(isBan);
        }
        #endregion
    }
}
