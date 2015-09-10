using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace FF.Networking
{
	internal class FFTcpWriter : FFTcpStreamThread
	{
		#region Properties
		protected Queue<FFMessage> _toSendMessages;
		#endregion
		
		internal FFTcpWriter(FFTcpClient a_ffClient) : base(a_ffClient)
		{
			_toSendMessages = new Queue<FFMessage>();
		}
		
		#region Message
		internal void QueueMessage(FFMessage a_message)
		{
			lock(_toSendMessages)
			{
				FFLog.Log(EDbgCat.Networking,"Queue message");
				_toSendMessages.Enqueue(a_message);
			}
		}
		#endregion
		
		#region Read
		protected bool Write(FFMessage a_message)
		{
			try
			{
				FFLog.Log(EDbgCat.Networking, "Writing");
				byte[] messageData;
				_memoryStream = new MemoryStream();
				_binaryFormatter.Serialize(_memoryStream,a_message);
				messageData = _memoryStream.ToArray();
				
				_data = new FFByteData();
				_data.Append(BitConverter.GetBytes(_memoryStream.Length));
				_data.Append(messageData);
				
				_client.GetStream().Write(_data.Data, 0, _data.Length);
				return true;
			}
			catch(IOException e)
			{
				FFLog.LogError(EDbgCat.Networking, "Couldn't write from stream." + e.StackTrace);
				return false;
			}
		}
		
		protected override void Task()
		{
			while(_client != null && _client.Connected && _shouldRun && _client.GetStream() != null)
			{
				if(_client.GetStream().CanWrite && _toSendMessages.Count > 0)
				{
					FFMessage toSend = _toSendMessages.Peek();
					if(Write(toSend) || !toSend.IsMandatory)
					{
						_toSendMessages.Dequeue();
					}
				}
				else
				{
					Thread.Sleep(10);
				}
			}
			
			FFLog.LogError(EDbgCat.Networking, "Stoping Writer Thread");
		}
		#endregion
	}
}