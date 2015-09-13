using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace FF.Networking
{
	internal delegate void TcpClientCallback(FFTcpClient a_ffclient);
	
	internal class FFTcpServer
	{
		#region Properties
		protected IPEndPoint _endPoint;
		protected TcpListener _tcpListener;
		protected Thread _listeningThread;
		protected Dictionary<IPEndPoint,FFTcpClient> _clients;
		protected bool _isListening = false;
		
		protected List<FFTcpClient> _newClients;
		internal TcpClientCallback onClientAdded = null;
		
		protected List<FFTcpClient> _removedClients;
		internal TcpClientCallback onClientRemoved = null;
		
		internal int Port
		{
			get
			{
				return _endPoint.Port;
			}
		}
		#endregion
		
		internal FFTcpServer(IPAddress a_ipv4)
		{
			try
			{
				_isListening = false;
				_newClients = new List<FFTcpClient>();
				_removedClients = new List<FFTcpClient>();
				_tcpListener = new TcpListener(a_ipv4, 0);
				_tcpListener.Start();
				_endPoint = (IPEndPoint)_tcpListener.Server.LocalEndPoint;
				_clients = new Dictionary<IPEndPoint, FFTcpClient>();
				FFLog.Log(EDbgCat.Networking, "Server started on address : " + _endPoint.Address + " & port : " + _endPoint.Port);
			}
			catch(SocketException e)
			{
				FFLog.LogError(EDbgCat.Networking, "Couldn't create server TCPListener." + e.StackTrace);
			}
		}
		
		internal void Close()
		{
			if(IsAcceptingConnections)
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
				_isListening = false;
				FFLog.Log(EDbgCat.Networking,"Stop thread");
			}
			else
			{
				FFLog.LogError(EDbgCat.Networking,"Server isn't listening");
			}
		}
		
		internal bool IsAcceptingConnections
		{
			get
			{
				return _isListening;
			}
		}
		#endregion
		
		#region Listening Work
		internal void ListeningTask()
		{
			while(_tcpListener != null && _isListening)
			{
				HandlePendingConnections();
				Thread.Sleep(50);
			}
			
			FFLog.LogError(EDbgCat.Networking, "Stoping Listener Thread");
		}
		
		protected void HandlePendingConnections()
		{
			if(_isListening && _tcpListener.Pending())
			{
				FFLog.Log(EDbgCat.Networking, "Pending connection.");
				TcpClient newClient = _tcpListener.AcceptTcpClient();
				
				FFTcpClient newFFClient = new FFTcpClient(newClient);
				newFFClient.StartWorkers();
				IPEndPoint newEp = newClient.Client.RemoteEndPoint as IPEndPoint;
				_clients.Add(newEp, newFFClient);
				
				_newClients.Add(newFFClient);
				_removedClients.Remove(newFFClient);
				
				FFLog.LogError(EDbgCat.Networking, "New Client");
			}
		}
		#endregion
		
		#region Client Management
		internal void DoUpdate()
		{
			while(_newClients.Count > 0)
			{
				FFTcpClient client = _newClients[0];
				_newClients.RemoveAt(0);
				if(onClientAdded != null)
					onClientAdded(client);
			}
			
			while(_removedClients.Count > 0)
			{
				FFTcpClient client = _removedClients[0];
				_removedClients.RemoveAt(0);
				if(onClientRemoved != null)
					onClientRemoved(client);
			}
			
			foreach(FFTcpClient each in _clients.Values)
			{
				each.DoUpdate();
			}
		}
		
		internal void BroadcastMessage(FFMessage a_message)
		{
			foreach(IPEndPoint endpoint in _clients.Keys)
			{
				SendMessageToClient(endpoint, a_message);
			}
		}
		
		internal void SendMessageToClient(IPEndPoint a_endpoint, FFMessage a_message)
		{
			_clients[a_endpoint].QueueMessage(a_message);
		}
		#endregion
	}
}