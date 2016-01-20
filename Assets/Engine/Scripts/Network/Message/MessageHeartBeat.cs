using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;

namespace FF.Network.Message
{
	internal class MessageHeartBeat : AMessage
	{
		#region Properties
		public long timeSent;
		#endregion
		
		public MessageHeartBeat()
		{
		 	timeSent = DateTime.Now.Ticks;
		}
		
		#region Methods
		internal override bool IsMandatory
		{
			get
			{
				return false;
			}
		}
		
		public override void SerializeData(FFByteWriter stream)
		{
			stream.Write(timeSent);
		}
		
		public override void LoadFromData (FFByteReader stream)
		{
			timeSent = stream.TryReadLong();
		}
		
		internal override EMessageType Type
		{
			get
			{
				return EMessageType.Heartbeat;
			}
		}
		#endregion
	}
}