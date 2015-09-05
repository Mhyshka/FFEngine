using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


using Zeroconf;


namespace FFNetworking
{
	internal class FFTcpClient
	{
		protected static BinaryFormatter s_binaryFormatter = new BinaryFormatter();
		
		#region Properties
		protected TcpClient _tcpClient;
		protected IPEndPoint _remote;
		protected Thread _readerThread;
		protected Thread _writerThread;
		#endregion
		
		#region Constructors
		/// <summary>
		/// Called by the client
		/// </summary>
		internal FFTcpClient(IPEndPoint a_remote)
		{
			_tcpClient = new TcpClient(new IPEndPoint(IPAddress.Loopback,0));
			_remote = a_remote;
		}
		
		/// <summary>
		/// Called by the server when accepting the connection
		/// </summary>
		internal FFTcpClient(TcpClient a_client)
		{
			_tcpClient = a_client;
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
				FFLog.LogError("Couldn't connect to server.");
				FFLog.LogError(e.StackTrace);
				return false;
			}
			
			return false;
		}
	
		internal void Close()
		{
			if(_tcpClient.Connected)
			{
				_tcpClient.GetStream().Close();
				_tcpClient.Close();
			}
		}
		
		internal void StartWorkers()
		{
			if(_readerThread == null && !_readerThread.IsAlive)
			{
				_readerThread = new Thread(new ThreadStart(ReaderTask));
				_readerThread.IsBackground = true;
				_readerThread.Start();
			}
			else
			{
				FFLog.LogError(EDbgCat.Networking, "Reader thread is already running.");
			}
			
			if(_writerThread == null || !_writerThread.IsAlive)
			{
				_writerThread = new Thread(new ThreadStart(WriterTask));
				_writerThread.IsBackground = true;
				_writerThread.Start();
			}
			else
			{
				FFLog.LogError(EDbgCat.Networking, "Writer thread is already running.");
			}
		}
		#endregion
		
		#region Writer		
		internal void QueueMessage(FFMessage a_message)
		{
			lock(_toSendMessages)
			{
				FFLog.Log(EDbgCat.Networking,"Queue message");
				_toSendMessages.Enqueue(a_message);
			}
		}
		
		protected Queue<FFMessage> _toSendMessages = new Queue<FFMessage>();
		
		protected void Write(FFMessage a_message)
		{
			try
			{
				FFLog.Log(EDbgCat.Networking, "Write");
				MemoryStream stream = new MemoryStream();
				s_binaryFormatter.Serialize(stream,a_message);
				byte[] data = stream.ToArray();
				_tcpClient.GetStream().Write(data,0,data.Length);
				_toSendMessages.Dequeue();
				
				FFLog.LogError(EDbgCat.Networking, data.Length.ToString());
			}
			catch(IOException e)
			{
				FFLog.LogError(EDbgCat.Networking, "Couldn't write to stream." + e.StackTrace);
			}
		}
		
		protected void WriterTask()
		{
			while(_tcpClient != null && _tcpClient.Connected)
			{
				if(_tcpClient.GetStream().CanWrite && _toSendMessages.Count > 0)
				{
					lock(_toSendMessages)
					{
						Write (_toSendMessages.Peek());
					}
				}
			}
		}
		#endregion
		
		#region Reader
		protected void Read()
		{
			try
			{
				FFLog.Log(EDbgCat.Networking, "Read");
				object data = s_binaryFormatter.Deserialize(_tcpClient.GetStream());
				FFLog.LogError(data.ToString());
				FFMessage[] messages = data as FFMessage[];
				foreach(FFMessage each in messages)
				{
					each.Read(_tcpClient);
				}
			}
			catch(IOException e)
			{
				FFLog.LogError(EDbgCat.Networking, "Couldn't read from stream." + e.StackTrace);
			}
		}
		
		protected void ReaderTask()
		{
			while(_tcpClient != null && _tcpClient.Connected)
			{
				if(_tcpClient.GetStream().CanRead && _tcpClient.GetStream().DataAvailable)
				{
					Read ();
				}
			}
		}
		#endregion
	}
}