using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;

using FF.Multiplayer;

namespace FF.Network.Message
{
	internal class MessageRoomInfos : AMessage
	{
		#region Properties
		public Room room;
		#endregion
		
		public MessageRoomInfos()
		{
		
		}
		
		internal MessageRoomInfos(Room a_room)
		{
		 	room = a_room;
		}
		
		#region Methods
		#endregion
		
		public override void SerializeData(FFByteWriter stream)
		{
			stream.Write(room);
		}
		
		public override void LoadFromData (FFByteReader stream)
		{
			room = stream.TryReadObject<Room>();
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