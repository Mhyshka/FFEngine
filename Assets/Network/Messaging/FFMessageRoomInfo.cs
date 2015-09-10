using UnityEngine;
using System.Collections;
using System.Net.Sockets;

namespace FF.Networking
{
	[System.Serializable]
	internal class FFMessageRoomInfo : FFMessage
	{
		#region Properties
		public int currentPlayerCount;
		public int maxPlayerCount;
		public string gameName;
		#endregion
		
		#region Methods
		internal override void Read(FFTcpClient a_tcpClient)
		{
			//FFLog.LogError("Je suis un message de room info.");
			a_tcpClient.QueueMessage(new FFMessagePlayerInfo());
		}	
		#endregion
	}
}