using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;

using FF.Multiplayer;

namespace FF.Network.Message
{
	internal class MessageRoomData : MessageData
	{
		#region Properties
		protected Room _room;
        internal Room Room
        {
            get
            {
                return _room;
            }
        }
        #endregion

        internal override EDataType Type
        {
            get
            {
                return EDataType.Room;
            }
        }

        public MessageRoomData()
		{
		
		}
		
		internal MessageRoomData(Room a_room)
		{
		 	_room = a_room;
		}
		
		#region Methods
		#endregion
		
		public override void SerializeData(FFByteWriter stream)
		{
			stream.Write(_room);
		}
		
		public override void LoadFromData (FFByteReader stream)
		{
			_room = stream.TryReadObject<Room>();
		}
		
		
	}
}