using UnityEngine;
using System.Collections;

namespace FF.Networking
{
	internal class FFMoveToSlotRequest : FFRequestMessage
	{
		#region Properties
		internal FFSlotRef slotRef;
		
		internal override EMessageType Type
		{
			get
			{
				return EMessageType.MoveToSlotRequest;
			}
		}
		#endregion
		
		public FFMoveToSlotRequest()
		{
		
		}
		
		internal FFMoveToSlotRequest(FFSlotRef a_slotRef)
		{
			slotRef = a_slotRef;
		}

        internal override void Read(FFTcpClient a_tcpClient)
        {
            FFNetworkPlayer player = FFEngine.Network.CurrentRoom.GetPlayerForEndpoint(a_tcpClient.Remote);
            string errorMessage = null;
            if (player != null)
            {
                if (FFEngine.Network.CurrentRoom.teams[slotRef.teamIndex].Slots[slotRef.slotIndex].netPlayer == null)
                {
                    FFEngine.Network.CurrentRoom.MovePlayer(player.SlotRef, slotRef);
                    FFMoveToSlotSuccess success = new FFMoveToSlotSuccess();
                    success.requestId = requestId;
                    a_tcpClient.QueueMessage(success);
                    return;
                }
                else
                {
                    errorMessage = "Slot is used.";
                }
            }
            else
            {
                errorMessage = "Player not found.";
            }

            FFMoveToSlotFail answer = new FFMoveToSlotFail(errorMessage);
            answer.requestId = requestId;
            a_tcpClient.QueueMessage(answer);
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
		
		internal SimpleCallback onSuccess = null;
		internal StringCallback onFail = null;
	}
}