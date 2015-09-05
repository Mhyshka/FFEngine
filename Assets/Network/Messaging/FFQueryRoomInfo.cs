using UnityEngine;
using System.Collections;
using System.Net.Sockets;

namespace FFNetworking
{
	[System.Serializable]
	internal class FFQueryRoomInfo : FFMessage
	{
		#region Properties
		#endregion
		
		#region Methods
		internal override void Read(TcpClient a_tcpClient)
		{
			
		}
		#endregion
	}
}