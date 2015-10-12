using UnityEngine;
using System.Collections;

namespace FF.Networking
{
	internal class FFMoveToSlotRequest : FFRequestMessage
	{
        internal enum EErrorCode
        {
            PlayerNotfound,
            PlayerDisconnected,
            SlotIsUsed
        }
		#region Properties
		internal FFSlotRef slotRef;
		
		internal override EMessageType Type
		{
			get
			{
				return EMessageType.MoveToSlotRequest;
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

        #region Constructors
        public FFMoveToSlotRequest()
		{
		
		}
		
		internal FFMoveToSlotRequest(FFSlotRef a_slotRef)
		{
			slotRef = a_slotRef;
		}
        #endregion

        internal override void Read()
        {
            FFResponseMessage answer = null;
            FFNetworkPlayer source = FFEngine.Network.CurrentRoom.GetPlayerForId(_client.NetworkID);

            int errorCode = -1;

            if (!_client.IsConnected)
            {
                errorCode = (int)EErrorCode.PlayerDisconnected;
                answer = new FFRequestFail(errorCode);
            }
            else if(source == null)
            {
                errorCode = (int)EErrorCode.PlayerNotfound;
                answer = new FFRequestFail(errorCode);
            }
            else if (FFEngine.Network.CurrentRoom.teams[slotRef.teamIndex].Slots[slotRef.slotIndex].netPlayer != null)
            {
                errorCode = (int)EErrorCode.SlotIsUsed;
                answer = new FFRequestFail(errorCode);
            }
            else
            {
                FFEngine.Network.CurrentRoom.MovePlayer(source.SlotRef, slotRef);
                answer = new FFRequestSuccess();
            }

            answer.requestId = requestId;
            _client.QueueMessage(answer);
        }

        #region Serialization
        public override void SerializeData (FFByteWriter stream)
		{
			base.SerializeData (stream);
			stream.Write(slotRef);
		}
		
		public override void LoadFromData (FFByteReader stream)
		{
			base.LoadFromData (stream);
			slotRef = stream.TryReadObject<FFSlotRef>();
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
                    case EErrorCode.SlotIsUsed:
                        errorMessage = "Slot is used.";
                        break;

                    case EErrorCode.PlayerNotfound:
                        errorMessage = "Player not found.";
                        break;

                    case EErrorCode.PlayerDisconnected:
                        errorMessage = "Player disconnected.";
                        break;
                }
            }
            return errorMessage;
        }
	}
}