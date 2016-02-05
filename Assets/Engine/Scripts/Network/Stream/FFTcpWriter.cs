﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using FF.Network.Message;

namespace FF.Network
{
	internal class FFTcpWriter : FFTcpStreamThread
	{
		#region Properties
		protected Queue<SentMessage> _toSendMessages;
		
		protected double _heartbeatTimespan = 3000d;//in MS
		protected DateTime _lastHeartbeatTimestamp;

        protected SentMessage _finalMessageRef = null;
		#endregion
		
		internal FFTcpWriter(FFTcpClient a_ffClient) : base(a_ffClient)
		{
			_toSendMessages = new Queue<SentMessage>();
        }
		
		#region Message
		internal void QueueMessage(SentMessage a_message)
		{
			FFLog.Log(EDbgCat.Socket,"Queue message");
			lock(_toSendMessages)
			{
				_toSendMessages.Enqueue(a_message);
			}
		}

        internal void QueueFinalMessage(SentMessage a_message)
        {
            FFLog.Log(EDbgCat.Socket, "Queue final message");
            lock (_toSendMessages)
            {
                _toSendMessages.Clear();
                _toSendMessages.Enqueue(a_message);
            }
            _finalMessageRef = a_message;
        }

        internal override void Start ()
		{
			base.Start ();
			_lastHeartbeatTimestamp = DateTime.Now;
		}
        #endregion

        #region Read
        protected bool Write(SentMessage a_message)
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
				FFLog.LogError(EDbgCat.Socket, "Couldn't write to stream." + e.Message);
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
                    SentMessage toSend;
					lock(_toSendMessages)
					{
						toSend = _toSendMessages.Peek();
					}

                    if (Write(toSend) || !toSend.IsMandatory)
					{
                        _ffClient.QueueWrittenMessage(toSend);
                        if (_finalMessageRef == toSend)
                            break;

						lock(_toSendMessages)
						{
							_toSendMessages.Dequeue();
						}
					}
                }
				else
				{
					Thread.Sleep(0);
				}
			}
			FFLog.LogError(EDbgCat.Socket, "Stoping Writer Thread");
		}
		
		protected void HandleHeartbeat()
		{
			TimeSpan span = DateTime.Now - _lastHeartbeatTimestamp;
			if(span.TotalMilliseconds > _heartbeatTimespan)
			{
                MessageLongData newHeartbeat = new MessageLongData();
                //TODO : FIX THAT SHIT
                //QueueMessage(newHeartbeat);
				_lastHeartbeatTimestamp = DateTime.Now;
			}
		}

        protected void OnHeartbeatSuccessReceived(MessageLongData a_heartbeat)
        {
        }
		#endregion
	}
}