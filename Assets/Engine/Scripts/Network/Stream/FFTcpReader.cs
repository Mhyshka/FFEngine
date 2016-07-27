using UnityEngine;
using System.Collections;

using System;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using FF.Network.Message;

namespace FF.Network
{
	internal class FFTcpReader : FFTcpStreamThread
	{
		#region Properties
		protected int _currentMessageSize;
		protected byte[] _data;
        protected const int _timeoutDuration = 5000;
        protected SimpleCallback _onConnectionLost = null;
		#endregion
		
		internal FFTcpReader(FFNetworkClient a_ffClient, SimpleCallback a_onConnectionLost) : base(a_ffClient)
		{
			_currentMessageSize = -1;
			_data = new byte[0];
            _onConnectionLost = a_onConnectionLost;
        }

        #region Read
        protected bool Read()
		{
			try
			{
				Byte[] readBytes = new byte[1024];
				int size = _stream.Read(readBytes, 0, readBytes.Length);
				//FFLog.Log(EDbgCat.Networking, "Reading : " + size.ToString());
				Array.Resize<byte>(ref readBytes,size);
				_data = _data.Append(readBytes);
				//FFLog.Log("Total size : " + _data.Length);
				DeserializeData();
                _ffClient.Clock.NewMessageReceived();
                return true;
			}
			catch(IOException e)
			{
                _crashed = true;
                FFLog.LogError(EDbgCat.Socket, "Couldn't read from stream.\n" + e.Message);
                return false;
            }
		}
		
		protected override void Task()
        {
            while (_shouldRun && Client != null && Client.Connected && _stream != null && _stream.CanRead)
            {
                if (!Read())
                    break;
            }

            if (_crashed && _onConnectionLost != null)
                _onConnectionLost();

            _thread = null;

            FFLog.LogError(EDbgCat.Socket, "Stoping Reader Thread");
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
					
					ReadMessage message = ReadMessage.Deserialize(messageData);

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