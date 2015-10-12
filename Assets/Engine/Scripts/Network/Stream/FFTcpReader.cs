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
		protected byte[] _data;
        protected long _lastMessageTimestamp;
        protected const int _timeoutDuration = 5000;
		#endregion
		
		internal FFTcpReader(FFTcpClient a_ffClient) : base(a_ffClient)
		{
			_currentMessageSize = -1;
			_data = new byte[0];
            _lastMessageTimestamp = 0L;
        }

        #region Start & Stop
        internal override void Start()
        {
            _lastMessageTimestamp = DateTime.Now.Ticks;
            base.Start();
        }
        #endregion

        #region Read
        protected void Read()
		{
			try
			{
                _lastMessageTimestamp = DateTime.Now.Ticks;
				Byte[] readBytes = new byte[1024];
				int size = Client.GetStream().Read(readBytes, 0, readBytes.Length);
				//FFLog.Log(EDbgCat.Networking, "Reading : " + size.ToString());
				Array.Resize<byte>(ref readBytes,size);
				_data = _data.Append(readBytes);
				//FFLog.Log("Total size : " + _data.Length);
				DeserializeData();
			}
			catch(IOException e)
			{
				FFLog.LogError(EDbgCat.Networking, "Couldn't read from stream." + e.Message);
			}
		}
		
		protected override void Task()
		{
			while(_shouldRun &&  Client != null && Client.Connected && _stream != null)
			{
                
				if(_shouldRun && _stream.CanRead && _stream.DataAvailable)
				{
					Read ();
				}
				else
				{
                    TimeSpan span = new TimeSpan(DateTime.Now.Ticks - _lastMessageTimestamp);
                    if (span.TotalMilliseconds > _timeoutDuration)
                    {
                        FFLog.Log(EDbgCat.Networking, "Connection timedout.");
                        if (_ffClient.IsConnected && _ffClient.WasConnected)
                        {
                            _ffClient.ConnectionLost();
                            break;
                        }
                    }
                    else
                    {
                        Thread.Sleep(3);
                    }
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
					byte[] lengthData = FFByteArrayExtension.Extract(ref _data, sizeof(Int32));
					_currentMessageSize = (int)BitConverter.ToInt32(lengthData,0);
				}
				
				if(ShouldDeserialize)
				{
					byte[] messageData = FFByteArrayExtension.Extract(ref _data, _currentMessageSize);
					_currentMessageSize = -1;
					
					FFMessage message = FFMessage.Deserialize(messageData);
					_ffClient.QueueReadMessage(message);
				}
			}
		}
		
		protected bool ShouldParseLength
		{
			get
			{
				bool result = false;
				result = (_currentMessageSize == -1 && _data.Length >= sizeof(Int32));
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