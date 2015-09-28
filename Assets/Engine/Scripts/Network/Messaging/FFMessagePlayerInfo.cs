using UnityEngine;
using System.Collections;
using System.Net.Sockets;

namespace FF.Networking
{
	internal class FFMessagePlayerInfo : FFMessage
	{
		#region Properties
		public FFNetworkPlayer player = null;
		#endregion
		
		#region Methods
		internal override void Read(FFTcpClient a_tcpClient)
		{
			FFLog.LogError("Player Infos read!");
		}	
		#endregion
		
		public override void SerializeData(FFByteWriter stream)
		{
		}
		
		public override void LoadFromData (FFByteReader stream)
		{
		}
		
		internal override EMessageType Type
		{
			get
			{
				return EMessageType.Heartbeat;
			}
		}
	}
}