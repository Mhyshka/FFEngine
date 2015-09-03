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
		
		protected ZeroconfRoom _room;
		protected Thread _readerThread;
		protected Thread _writerThread;
		#endregion
		
		#region Constructors
		/// <summary>
		/// Called by the client
		/// </summary>
		internal FFTcpClient(ZeroconfRoom a_room)
		{
			_room = a_room;
			tcpClient = new TcpClient(new IPEndPoint(IPAddress.Loopback,0));
			tcpClient.Connect(a_room.EndPoint);
			
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
		}
		#endregion
		
		#region Writer		
		internal void QueueMessage(FFMessage a_message)
		{
			lock(_toSendMessages)
			{
				_toSendMessages.Enqueue(a_message);
			}
		}
		
		protected Queue<FFMessage> _toSendMessages = new Queue<FFMessage>();
		
		protected void Write(FFMessage a_message)
		{
			MemoryStream stream = new MemoryStream();
			s_binaryFormatter.Serialize(stream,a_message);
			byte[] data = stream.ToArray();
			tcpClient.GetStream().Write(data,0,data.Length);
		}
		
		protected void WriterTask()
		{
			while(tcpClient.GetStream().CanWrite && _toSendMessages.Count > 0)
			{
				lock(_toSendMessages)
				{
					Write (_toSendMessages.Dequeue());
				}
			}
		}
		#endregion
		
		#region Reader
		protected void Read()
		{
			object data = s_binaryFormatter.Deserialize(tcpClient.GetStream());
			/*FFMessage messages = data as FFMessage[];
			foreach(FFMessage each in messages)
			{
				each.Read();
			}*/
		}
		
		protected void ReaderTask()
		{
			while(tcpClient.GetStream().CanRead && tcpClient.GetStream().DataAvailable)
			{
				Read ();
			}
		}
		#endregion
	}
}