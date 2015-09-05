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
		internal TcpClient tcpClient;
		
		protected Thread _readerThread;
		protected Thread _writerThread;
		#endregion
		
		#region Constructors
		/// <summary>
		/// Called by the client
		/// </summary>
		internal FFTcpClient(IPEndPoint a_remote)
		{
			tcpClient = new TcpClient(new IPEndPoint(IPAddress.Loopback,0));
			tcpClient.Connect(a_remote);
			
			_readerThread = new Thread(new ThreadStart(ReaderTask));
			_readerThread.Start();
			
			_writerThread = new Thread(new ThreadStart(WriterTask));
			_writerThread.Start();
		}
		
		/// <summary>
		/// Called by the server when accepting the connection
		/// </summary>
		internal FFTcpClient(TcpClient a_client)
		{
			tcpClient = a_client;
			
			_readerThread = new Thread(new ThreadStart(ReaderTask));
			_readerThread.Start();
			
			_writerThread = new Thread(new ThreadStart(WriterTask));
			_writerThread.Start();
		}
		#endregion
		
		#region Connection
		internal void Close()
		{
			tcpClient.Close();
			_writerThread.Abort();
			_readerThread.Abort();
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
			FFLog.Log(EDbgCat.Networking,"Write");
			MemoryStream stream = new MemoryStream();
			s_binaryFormatter.Serialize(stream,a_message);
			byte[] data = stream.ToArray();
			FFLog.LogError(data.Length.ToString());
			tcpClient.GetStream().Write(data,0,data.Length);
		}
		
		protected void WriterTask()
		{
			while(tcpClient.Connected)
			{
				if(tcpClient.GetStream().CanWrite && _toSendMessages.Count > 0)
				{
					lock(_toSendMessages)
					{
						Write (_toSendMessages.Dequeue());
					}
				}
			}
		}
		#endregion
		
		#region Reader
		protected void Read()
		{
			FFLog.Log(EDbgCat.Networking,"Read");
			object data = s_binaryFormatter.Deserialize(tcpClient.GetStream());
			FFLog.LogError(data.ToString());
			FFMessage[] messages = data as FFMessage[];
			foreach(FFMessage each in messages)
			{
				each.Read(tcpClient);
			}
		}
		
		protected void ReaderTask()
		{
			while(tcpClient.Connected)
			{
				if(tcpClient.GetStream().CanRead && tcpClient.GetStream().DataAvailable)
				{
					Read ();
				}
			}
		}
		#endregion
	}
}