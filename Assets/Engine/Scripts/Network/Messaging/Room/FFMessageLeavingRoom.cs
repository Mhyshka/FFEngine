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

        internal override void Read()
        {
            FFLog.Log(EDbgCat.Networking, "Reading leaving room message");
            if (FFEngine.Network.IsServer)
            {
                FFEngine.Network.CurrentRoom.RemovePlayer(_client.NetworkID);
                _client.EndConnection("Leaving room");
            }
        }

        internal override bool IsMandatory
        {
            get
            {
                return false;
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