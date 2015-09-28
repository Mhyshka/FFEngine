using UnityEngine;
using System.Collections;
using System.Net.Sockets;

namespace FF.Networking
{
	internal class FFJoinRoomRequest : FFRequestMessage
	{
		#region Properties
		public FFNetworkPlayer player = null;
		#endregion
		
		internal StringCallback onFail;
		internal SimpleCallback onSuccess;
		
		public FFJoinRoomRequest()
		{
		}
		
		internal FFJoinRoomRequest(FFNetworkPlayer a_player)
		{
			player = a_player;
		}
		
		#region Methods
		internal override void Read(FFTcpClient a_tcpClient)
		{
			if(FFEngine.Network.Server != null)
			{
				FFRoom room = FFEngine.Network.CurrentRoom;
				if(room.IsFull)
				{
                    FFJoinRoomFail answer = new FFJoinRoomFail("Room is full.");
					answer.requestId = requestId;
					a_tcpClient.QueueMessage(answer);
				}
				else
				{

                    FFSlot nextSlot = room.NextAvailableSlot();
                    player.ipEndPoint = a_tcpClient.Remote;
                    FFEngine.Network.CurrentRoom.SetPlayer(nextSlot.team.teamIndex, nextSlot.slotIndex, player);
                    FFJoinRoomSuccess answer = new FFJoinRoomSuccess();
                    answer.requestId = requestId;
					a_tcpClient.QueueMessage(answer);
				}
			}
		}
		
		internal override EMessageType Type
		{
			get
			{
				return EMessageType.JoinRoomRequest;
			}
		}
		#endregion
		
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
	}
}