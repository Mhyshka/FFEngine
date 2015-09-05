using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace FFNetworking
{
	internal class FFTcpServer
	{
		#region Properties
		protected IPEndPoint _endPoint;
		protected TcpListener _tcpListener;
		protected Thread _listeningThread;
		protected Dictionary<Player,FFTcpClient> _clients;
		protected bool _isListening = false;
		
		internal int Port
		{
			get
			{
				return _endPoint.Port;
			}
		}
		#endregion
		
		internal FFTcpServer()
		{
			try
			{
				_tcpListener = new TcpListener(IPAddress.Loopback,0);
				_tcpListener.Start();
				_endPoint = (IPEndPoint)_tcpListener.Server.LocalEndPoint;
				_clients = new Dictionary<Player,FFTcpClient>();
				FFLog.Log(EDbgCat.Networking, "Server started on address : " + _endPoint.Address + " & port : " + _endPoint.Port);
			}
			catch(SocketException e)
			{
				FFLog.LogError(EDbgCat.Networking, "Couldn't start create server TCPListener." + e.StackTrace);
			}
		}
		
		internal void Close()
		{
			if(_isListening)
			{
				StopAcceptingConnections();
			}
			
			try
			{
				_tcpListener.Stop();
			}
			catch(SocketException e)
			{
				FFLog.LogError(EDbgCat.Networking, "Couldn't Stop server." + e.StackTrace);
			}
			
			foreach(FFTcpClient client in _clients.Values)
			{
				client.Close();
			}
			_clients.Clear();
		}
		
		#region Client Acceptation
		internal void StartAcceptingConnections()
		{
			if(!_isListening)
			{
				FFLog.Log(EDbgCat.Networking,"Server start listening");
				_isListening = true;
				_listeningThread = new Thread(new ThreadStart(ListeningTask));
				_listeningThread.IsBackground = true;
				_listeningThread.Start();
			}
			else
			{
				FFLog.LogError(EDbgCat.Networking,"Server is already listening");
			}
		}
		
		internal void StopAcceptingConnections()
		{
			if(_isListening)
			{
				_listeningThread.Abort();
				_isListening = false;
				FFLog.Log(EDbgCat.Networking,"Stop thread");
			}
			else
			{
				FFLog.LogError(EDbgCat.Networking,"Server isn't listening");
			}
		}
		
		internal void ListeningTask()
		{
			while(_isListening)
			{
				HandlePendingConnections();
				Thread.Sleep(50);
			}
		}
		
		internal void HandlePendingConnections()
		{
			if(_isListening && _tcpListener.Pending())
			{
				FFLog.Log(EDbgCat.Networking, "Pending connection.");
				TcpClient newClient = _tcpListener.AcceptTcpClient();
				
				FFTcpClient newFFClient = new FFTcpClient(newClient);
				newFFClient.StartWorkers();
				IPEndPoint newEp = newClient.Client.RemoteEndPoint as IPEndPoint;
				Player player = new Player(newEp);
				_clients.Add(player, newFFClient);
				
				WelcomeClient(newFFClient);
			}
		}
		
		protected void WelcomeClient(FFTcpClient a_newClient)
		{
			FFLog.LogError(EDbgCat.Networking, "New Client");
			
			FFResponseRoomInfo roomInfo = new FFResponseRoomInfo();
			roomInfo.currentPlayerCount = 2;
			roomInfo.maxPlayerCount = 3;
			roomInfo.gameName = "My Game!";
			
			a_newClient.QueueMessage(roomInfo);
		}
		#endregion
	}
}