using UnityEngine;
using System.Collections;
using System.Net.Sockets;

namespace FF.Networking
{	
	[System.Serializable]
	internal abstract class FFMessage
	{
		#region properties
		#endregion
		
		#region Methods
		internal abstract void Read(TcpClient a_tcpClient);
		#endregion
	}
}