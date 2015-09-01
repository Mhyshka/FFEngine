using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;

namespace FFNetworking
{
	internal class Server
	{
		protected IPEndPoint _endPoint;
		protected TcpListener _server;
		protected Dictionary<Player,TcpClient> _clients;
		
		internal Server(string a_localAddress, int port)
		{
			IPAddress address = IPAddress.Parse(a_localAddress);
			IPEndPoint endPoint = new IPEndPoint(address,port);
			_clients = new Dictionary<Player,TcpClient>();
		}
		
	#region Client Acceptation
		internal void AcceptConnections()
		{
			if(_server == null)
			{
				_server = new TcpListener(_endPoint);
				_server.Start ();
			}
		}
		
		internal void StopAcceptingConnections()
		{
			if(_server != null)
			{
				_server.Stop ();
				_server = null;
			}
		}
		
		internal void Connect(string a_localAddress, int port)
		{
			if(_server.Pending())
			{
				TcpClient newClient = _server.AcceptTcpClient();
				IPEndPoint newEp = newClient.Client.RemoteEndPoint as IPEndPoint;
				
				Player player = new Player(newEp);
				
				_clients.Add(player, newClient);
			}
		}
	#endregion
	}
}