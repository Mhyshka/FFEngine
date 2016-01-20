using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;

using FF.Multiplayer;

namespace FF.Network.Message
{
	internal class RequestJoinRoom : ARequest
	{
        internal enum EErrorCode
        {
            UserCanceled,
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
        #endregion

        public RequestJoinRoom()
		{
		}
		
		internal RequestJoinRoom(FFNetworkPlayer a_player)
		{
			player = a_player;
		}

        #region Methods
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

            errorMessage = ARequest.MessageForErrorCode(a_errorCode);
            if (string.IsNullOrEmpty(errorMessage))
            {
                EErrorCode errorCode = (EErrorCode)a_errorCode;

                switch (errorCode)
                {

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