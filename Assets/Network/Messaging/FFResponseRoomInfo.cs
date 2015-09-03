using UnityEngine;
using System.Collections;
using System.Net.Sockets;

namespace FFNetworking
{
	internal class FFResponseRoomInfo : FFMessage
	{
		#region Properties
		public int currentPlayerCount;
		public int maxPlayerCount;
		public string gameName;
		#endregion
		
		#region Methods
		internal override void Read(TcpClient a_tcpClient)
		{
		
		}
		#endregion
	}
}