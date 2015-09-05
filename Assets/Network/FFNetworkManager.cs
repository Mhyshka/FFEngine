using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Zeroconf;

namespace FFNetworking
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
			_server = new FFTcpServer();
			_clients = new Dictionary<ZeroconfRoom, FFTcpClient>();
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
	}
}