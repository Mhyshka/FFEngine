using UnityEngine;
using System.Collections;
using System;

namespace FF.Networking
{
    internal class FFMessageKick : FFMessage
    {
        internal static SimpleCallback onKickReceived;

        internal override EMessageType Type
        {
            get
            {
                return EMessageType.Kick;
            }
        }

        internal override void Read(FFTcpClient a_tcpClient)
        {
            FFLog.Log(EDbgCat.Networking, "Kick message received.");
            if (onKickReceived != null)
                onKickReceived();
        }

        #region Serialization
        public override void LoadFromData(FFByteReader stream)
        {
        }

        public override void SerializeData(FFByteWriter stream)
        {
        }
        #endregion
    }
}
