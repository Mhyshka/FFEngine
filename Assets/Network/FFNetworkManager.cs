using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Net.Sockets;
using System.Net;

using Zeroconf;

namespace FF.Networking
{
	internal class FFNetworkManager
	{
		#region Properties
		protected FFTcpServer _server;
		protected Dictionary<ZeroconfRoom,FFTcpClient> _clients;
		protected ZeroconfRoom _mainRoom;
		protected FFTcpClient _mainClient;
		#endregion
		
		
		#region Main methods
		internal FFNetworkManager()
		{
			_mainRoom = null;
			_mainClient = null;
		}
		
		~FFNetworkManager()
		{
			_server.Close();
			foreach(ZeroconfRoom room in _clients.Keys)
			{
				_clients[room].Close();
			}
		}
	
		internal void DoUpdate()
		{
		
		}
		#endregion
		
		
		#region Server
		internal FFTcpServer Server
		{
			get
			{
				return _server;
			}
		}
		
		internal void StartServer()
		{
			if(_server == null)
			{
				_server = new FFTcpServer(NetworkIP);
			}
			else
			{
				FFLog.LogWarning(EDbgCat.Networking, "You're trying to start another server.");
			}
		}
		
		internal void StopServer()
		{
			_server.Close();
			_server = null;
		}
		
		#endregion
		
		#region Clients
		internal void JoinZeroconfRoom(ZeroconfRoom a_room)
		{
			
			if(_clients.TryGetValue(a_room,out _mainClient))
			{
				_mainRoom = a_room;
			}
			else
			{
				LeaveRoom();
			}
		}
		
		internal void LeaveRoom()
		{
			_mainRoom = null;
			_mainClient = null;
		}
		
		internal FFTcpClient MainClient
		{
			get
			{
				return _mainClient;
			}
		}
		#endregion
		
		#region IP
		internal IPAddress NetworkIP
		{
			get
			{
				IPAddress[] ipAddresses = Dns.GetHostAddresses (Dns.GetHostName());
				IPAddress ipv4 = null;
				foreach(IPAddress each in ipAddresses)
				{
					if(each.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(each))
					{
						return each;
					}
				}
				
				return null;
			}
		}
		#endregion
	}
}