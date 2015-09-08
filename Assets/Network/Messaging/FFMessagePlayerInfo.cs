using UnityEngine;
using System.Collections;
using System.Net.Sockets;

namespace FF.Networking
{
	[System.Serializable]
	internal class FFMessagePlayerInfo : FFMessage
	{
		#region Properties
		public Player player = null;
		#endregion
		
		#region Methods
		internal override void Read(FFTcpClient a_tcpClient)
		{
			FFLog.LogError("Player Infos read!");
		}	
		#endregion
	}
}