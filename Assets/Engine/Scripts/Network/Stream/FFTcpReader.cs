using UnityEngine;
using System.Collections;

using System;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace FF.Networking
{
	internal class FFTcpReader : FFTcpStreamThread
	{
		#region Properties
		protected int _currentMessageSize;
		#endregion
		
		internal FFTcpReader(FFTcpClient a_ffClient) : base(a_ffClient)
		{
			_currentMessageSize = -1;
			_data = new FFByteData();
		}
		
		#region Start & Stop
		#endregion
		
		#region Read
		protected void Read()
		{
			try
			{
				FFLog.Log(EDbgCat.Networking, "Reading");
				Byte[] readBytes = new byte[1024];
				int size = _client.GetStream().Read(readBytes, 0, readBytes.Length);
				Array.Resize<byte>(ref readBytes,size);
				_data.Append(readBytes);
				DeserializeData();
			}
			catch(IOException e)
			{
				FFLog.LogError(EDbgCat.Networking, "Couldn't read from stream." + e.StackTrace);
			}
		}
		
		protected override void Task()
		{
			while(_client != null && _client.Connected && _shouldRun && _client.GetStream() != null)
			{
				if(_client.GetStream().CanRead && _client.GetStream().DataAvailable)
				{
					Read ();
				}
				else
				{
					Thread.Sleep(10);
				}
			}
			
			FFLog.LogError(EDbgCat.Networking, "Stoping Reader Thread");
		}
		
		protected void DeserializeData()
		{
			while(ShouldParseLength || ShouldDeserialize)
			{
				if(ShouldParseLength)
				{
					byte[] lengthData = _data.Extract(sizeof(Int64));
					_currentMessageSize = (int)BitConverter.ToInt64(lengthData,0);
				}
				
				if(ShouldDeserialize)
				{
					byte[] messageData = _data.Extract(_currentMessageSize);
					_memoryStream = new MemoryStream(messageData);
					
					object objectData = _binaryFormatter.Deserialize(_memoryStream);
					_currentMessageSize = -1;
					
					FFMessage messages = objectData as FFMessage;
					_ffClient.QueueReadMessage(messages);
				}
			}
		}
		
		protected bool ShouldParseLength
		{
			get
			{
				bool result = false;
				result = (_currentMessageSize == -1 && _data.Length >= sizeof(Int64));
				return result;
			}
		}
		
		protected bool ShouldDeserialize
		{
			get
			{
				bool result = false;
				result = (_currentMessageSize >= 0 && _data.Length >= _currentMessageSize);
				return result;
			}
		}
	}
	#endregion
}