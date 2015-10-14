using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;

namespace FF.Networking
{
	internal class FFMessageHeartBeat : FFMessage
	{
		#region Properties
		public long timeSent;
		#endregion
		
		public FFMessageHeartBeat()
		{
		 	timeSent = DateTime.Now.Ticks;
		}
		
		#region Methods
		internal override void Read()
		{
			long spanTick = DateTime.Now.Ticks - timeSent;
			TimeSpan span = TimeSpan.FromTicks(spanTick);
			//FFLog.Log(EDbgCat.Networking, "Heartbeat " + span.TotalMilliseconds.ToString("0.") + "ms");
		}	
		
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