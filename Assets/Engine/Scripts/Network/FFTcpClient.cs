using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


using Zeroconf;


namespace FF.Networking
{
	internal class FFTcpClient
	{
		#region Properties
		protected TcpClient _tcpClient;
		internal TcpClient TcpClient
		{
			get
			{
				return _tcpClient;
			}
		}
		
		protected IPEndPoint _remote;
		internal IPEndPoint Remote
		{
			get
			{
				return _remote;
			}
		}
		
		protected IPEndPoint _local;
		internal IPEndPoint Local
		{
			get
			{
				
				return _remote;
			}
		}
		
		protected FFTcpReader _reader;
		protected FFTcpWriter _writer;
		
		protected Queue<FFMessage> _readMessages = new Queue<FFMessage>();
		
		internal void QueueReadMessage(FFMessage a_message)
		{
			lock(_readMessages)
			{
				_readMessages.Enqueue(a_message);
			}
		}
		#endregion
		
		#region Constructors
		/// <summary>
		/// Called by the client
		/// </summary>
		internal FFTcpClient(IPEndPoint a_local, IPEndPoint a_remote)
		{
			_tcpClient = new TcpClient(a_local);
			_tcpClient.SendTimeout = 60000;
			_tcpClient.ReceiveTimeout = 60000;
			_local = a_local;
			_remote = a_remote;
			_reader = new FFTcpReader(this);
			_writer = new FFTcpWriter(this);
		}
		
		/// <summary>
		/// Called by the server when accepting the connection
		/// </summary>
		internal FFTcpClient(TcpClient a_client)
		{
			_tcpClient = a_client;
			_tcpClient.SendTimeout = 60000;
			_tcpClient.ReceiveTimeout = 60000;
			_local = _tcpClient.Client.LocalEndPoint as IPEndPoint;
			_remote = _tcpClient.Client.RemoteEndPoint as IPEndPoint;
			_reader = new FFTcpReader(this);
			_writer = new FFTcpWriter(this);
		}
		#endregion
		
		#region Connection
		internal bool Connect()
		{
			try
			{
				_tcpClient.Connect(_remote);
				return true;
			}
			catch (SocketException e) 
			{
				FFLog.LogError("Couldn't connect to server." + e.StackTrace);
				return false;
			}
		}
	
		internal void Close()
		{
			if(_tcpClient.Connected)
			{
				_tcpClient.GetStream().Close();
				_tcpClient.Close();
			}
			_reader.Stop();
			_writer.Stop();
		}
		
		internal void StartWorkers()
		{
			_reader.Start();
			_writer.Start();
		}
		#endregion
		
		internal void QueueMessage(FFMessage a_message)
		{
			_writer.QueueMessage(a_message);
		}
		
		internal void DoUpdate()
		{
			if(_readMessages.Count > 0)
			{
				lock(_readMessages)
				{
					while(_readMessages.Count > 0 )
					{
						FFLog.Log(EDbgCat.Networking,"Reading new message");
						_readMessages.Dequeue().Read (this);
					}
				}
			}
		}
	}
}