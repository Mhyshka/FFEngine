using UnityEngine;
using System.Collections;
using System;

namespace FF.Networking
{
    internal class FFMessageNetworkID : FFMessage
    {
        internal static FFTcpClientCallback onNetworkIdReceived = null;

        internal int id;

        internal override EMessageType Type
        {
            get
            {
                return EMessageType.NetworkID;
            }
        }

        public FFMessageNetworkID()
        {
            id = -1;
        }

        internal FFMessageNetworkID(int a_id)
        {
            id = a_id;
        }

        internal override void Read(FFTcpClient a_tcpClient)
        {
            a_tcpClient.NetworkID = id;
            if (onNetworkIdReceived != null)
                onNetworkIdReceived(a_tcpClient);
        }

        public override void LoadFromData(FFByteReader stream)
        {
            id = stream.TryReadInt();
        }

        public override void SerializeData(FFByteWriter stream)
        {
            stream.Write(id);
        }
    }
}