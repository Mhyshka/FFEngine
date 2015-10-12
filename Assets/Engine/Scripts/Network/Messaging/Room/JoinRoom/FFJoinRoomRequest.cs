using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;

namespace FF.Networking
{
	internal class FFJoinRoomRequest : FFRequestMessage
	{
        internal enum EErrorCode
        {
            UserCanceled,
            PlayerDisconnected,
            ServerOnly,
            RoomIsFull,
            Banned
        }
		#region Properties
		internal FFNetworkPlayer player = null;

        internal override EMessageType Type
        {
            get
            {
                return EMessageType.JoinRoomRequest;
            }
        }

        protected override int DisconnectErrorCode
        {
            get
            {
                return (int)EErrorCode.PlayerDisconnected;
            }
        }
        #endregion

        public FFJoinRoomRequest()
		{
		}
		
		internal FFJoinRoomRequest(FFNetworkPlayer a_player)
		{
			player = a_player;
		}

        #region Methods
        internal override void Read()
		{
            FFResponseMessage answer = null;
            int errorCode = -1;
            FFRoom room = FFEngine.Network.CurrentRoom;

            if (!_client.IsConnected)
            {
                errorCode = (int)EErrorCode.PlayerDisconnected;
                answer = new FFRequestFail(errorCode);
            }
            else if (FFEngine.Network.Server == null)
            {
                errorCode = (int)EErrorCode.ServerOnly;
                answer = new FFRequestFail(errorCode);
            }
            else if (room.IsBanned(_client.NetworkID))
            {
                errorCode = (int)EErrorCode.Banned;
                answer = new FFRequestFail(errorCode);
            }
            else if (room.IsFull)
            {
                errorCode = (int)EErrorCode.RoomIsFull;
                answer = new FFRequestFail(errorCode);
            }
            else
            {
                FFSlot nextSlot = room.NextAvailableSlot();
                player.IpEndPoint = _client.Remote;
                FFEngine.Network.CurrentRoom.SetPlayer(nextSlot.team.teamIndex, nextSlot.slotIndex, player);
                answer = new FFRequestSuccess();
            }

            answer.requestId = requestId;
            _client.QueueMessage(answer);
        }
        #endregion

        #region Serialization
        public override void SerializeData(FFByteWriter stream)
		{
			base.SerializeData(stream);
			stream.Write(player);
		}
		
		public override void LoadFromData (FFByteReader stream)
		{
			base.LoadFromData(stream);
			player = stream.TryReadObject<FFNetworkPlayer>();
		}
        #endregion

        internal static string MessageForCode(int a_errorCode)
        {
            string errorMessage = "";

            if (a_errorCode == -2)
                errorMessage = "Unkown";
            else if (a_errorCode == -1)
                errorMessage = "Timedout";
            else
            {
                EErrorCode errorCode = (EErrorCode)a_errorCode;

                switch (errorCode)
                {
                    case EErrorCode.PlayerDisconnected:
                        errorMessage = "Player disconnected.";
                        break;

                    case EErrorCode.ServerOnly:
                        errorMessage = "Invalid request.";
                        break;

                    case EErrorCode.Banned:
                        errorMessage = "You're banned from this room.";
                        break;

                    case EErrorCode.RoomIsFull:
                        errorMessage = "Room is full.";
                        break;
                }
            }

            return errorMessage;
        }
    }
}