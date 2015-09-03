using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;

namespace FFNetworking
{
	internal class Client
	{
		#region Properties
		protected TcpClient _tcpClient;
		#endregion
		
		internal Client(IPEndPoint a_serverEndpoint)
		{
			_tcpClient = new TcpClient(new IPEndPoint(IPAddress.Loopback,0));
			_tcpClient.Connect(a_serverEndpoint);
		}
		
		#region Connection
		internal void Close()
		{
			_tcpClient.Close();
		}
		#endregion
	}
}