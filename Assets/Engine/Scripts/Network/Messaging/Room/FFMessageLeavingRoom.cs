using UnityEngine;
using System.Collections;
using System;

namespace FF.Networking
{
    internal class FFMessageLeavingRoom : FFMessage
    {
        #region Properties
        #endregion

        internal override EMessageType Type
        {
            get
            {
                return EMessageType.LeavingRoom;
            }
        }

        public FFMessageLeavingRoom()
        {
        }

        internal override void Read(FFTcpClient a_tcpClient)
        {
            FFLog.Log(EDbgCat.Networking, "Reading leaving room message");
            if (FFEngine.Network.IsServer)
            {
                FFNetworkPlayer player = FFEngine.Network.CurrentRoom.GetPlayerForEndpoint(a_tcpClient.Remote);
                FFEngine.Network.CurrentRoom.RemovePlayer(player.SlotRef);
                a_tcpClient.EndConnection("Leaving room");
            }
        }

        #region Serialization
        public override void SerializeData(FFByteWriter stream)
        {
        }

        public override void LoadFromData(FFByteReader stream)
        {
        }
        #endregion
    }
}