using UnityEngine;
using System.Collections;
using System.Net.Sockets;

namespace FF.Networking
{
	[System.Serializable]
	internal class FFQueryRoomInfo : FFMessage
	{
		#region Properties
		#endregion
		
		#region Methods
		internal override void Read(FFTcpClient a_tcpClient)
		{
			
		}
		#endregion
	}
}