using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;

namespace FF.Networking
{	
	[System.Serializable]
	internal class Player
	{
		#region Properties
		internal IPEndPoint ipEndPoint = null;
		internal string hostName = "";
		internal string nickName = "";
		
		internal string Address
		{
			get
			{
				return ipEndPoint.Address.ToString();
			}
		}
		
		internal int Port
		{
			get
			{
				return ipEndPoint.Port;
			}
		}
		#endregion
		
		internal Player (IPEndPoint a_endPoint)
		{
			ipEndPoint = a_endPoint;
		}
	}
}