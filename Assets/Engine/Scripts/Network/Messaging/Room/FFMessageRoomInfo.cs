using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;

namespace FF.Networking
{
	internal class FFMessageRoomInfo : FFMessage
	{
		#region Properties
		public FFRoom room;
		#endregion
		
		public FFMessageRoomInfo()
		{
		
		}
		
		internal FFMessageRoomInfo(FFRoom a_room)
		{
		 	room = a_room;
		}
		
		#region Methods
		internal override void Read(FFTcpClient a_tcpClient)
		{
			room.serverEndPoint = a_tcpClient.Remote;
			FFEngine.Network.OnRoomInfosReceived(room);
		}	
		#endregion
		
		public override void SerializeData(FFByteWriter stream)
		{
			stream.Write(room);
		}
		
		public override void LoadFromData (FFByteReader stream)
		{
			room = stream.TryReadObject<FFRoom>();
		}
		
		internal override EMessageType Type
		{
			get
			{
				return EMessageType.RoomInfos;
			}
		}
	}
}