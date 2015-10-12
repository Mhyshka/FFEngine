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
		
		protected double _heartbeatTimespan = 3000d;//in MS
		protected DateTime _lastHeartbeatTimestamp;
		#endregion
		
		internal FFTcpWriter(FFTcpClient a_ffClient) : base(a_ffClient)
		{
			_toSendMessages = new Queue<FFMessage>();
        }
		
		#region Message
		internal void QueueMessage(FFMessage a_message)
		{
			FFLog.Log(EDbgCat.Networking,"Queue message");
			lock(_toSendMessages)
			{
				_toSendMessages.Enqueue(a_message);
			}
		}

        internal void QueueFinalMessage(FFMessage a_message)
        {
            FFLog.Log(EDbgCat.Networking, "Queue message");
            lock (_toSendMessages)
            {
                _toSendMessages.Clear();
                _toSendMessages.Enqueue(a_message);
            }
        }

        internal override void Start ()
		{
			base.Start ();
			_lastHeartbeatTimestamp = DateTime.Now;
		}
        #endregion

        #region Read
        protected bool Write(FFMessage a_message)
		{
			try
			{
				//FFLog.Log(EDbgCat.Networking, "Start Writing : " + a_message.ToString() + " to " + _ffClient.Remote.ToString());
				byte[] messageData = a_message.Serialize();
				
				int length = messageData.Length;
				messageData = messageData.Insert(BitConverter.GetBytes(length),0);

                Client.GetStream().Write(messageData, 0, messageData.Length);
				//FFLog.Log(EDbgCat.Networking, "End Writing : " + messageData.Length);
				return true;
			}
			catch(IOException e)
			{
				FFLog.LogError(EDbgCat.Networking, "Couldn't write to stream." + e.Message);
				return false;
			}
		}
		
		protected override void Task()
		{
			while(_shouldRun && Client != null && Client.Connected && _stream != null && _stream.CanWrite)
			{
                HandleHeartbeat();
				
				if(_shouldRun && _toSendMessages.Count > 0)
				{
					FFMessage toSend;
					lock(_toSendMessages)
					{
						toSend = _toSendMessages.Peek();
					}

                    if (Write(toSend) || !toSend.IsMandatory)
					{
                        if(toSend.PostWrite())
                            break;

						lock(_toSendMessages)
						{
							_toSendMessages.Dequeue();
						}
					}
                }
				else
				{
					Thread.Sleep(3);
				}
			}
			FFLog.LogError(EDbgCat.Networking, "Stoping Writer Thread");
		}
		
		protected void HandleHeartbeat()
		{
			TimeSpan span = DateTime.Now - _lastHeartbeatTimestamp;
			if(span.TotalMilliseconds > _heartbeatTimespan)
			{
				QueueMessage(new FFMessageHeartBeat());
				_lastHeartbeatTimestamp = DateTime.Now;
			}
		}
		#endregion
	}
}