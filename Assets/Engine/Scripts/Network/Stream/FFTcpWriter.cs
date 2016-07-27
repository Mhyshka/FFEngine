using UnityEngine;
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
        protected AutoResetEvent _waitHandler = null;
        protected Queue<SentMessage> _toSendMessages;

        protected SentMessage _finalMessageRef = null;


        protected SimpleCallback _onConnectionLost = null;

        protected Thread _heartbeatThread = null;
        #endregion

        internal FFTcpWriter(FFNetworkClient a_ffClient, SimpleCallback a_onConnectionLost) : base(a_ffClient)
		{
			_toSendMessages = new Queue<SentMessage>();
            _onConnectionLost = a_onConnectionLost;
        }

        internal override void Start()
        {
            _waitHandler = new AutoResetEvent(false);
            base.Start();
            if (_heartbeatThread == null || !_heartbeatThread.IsAlive)
            {
                _heartbeatThread = new Thread(new ThreadStart(HeartbeatTask));
                _heartbeatThread.IsBackground = true;
                _heartbeatThread.Start();
            }
        }

        internal override void Stop()
        {
            base.Stop();
            if(_waitHandler != null)
                _waitHandler.Set();
        }

        #region Message
        internal void QueueMessage(SentMessage a_message)
		{
			FFLog.Log(EDbgCat.Socket,"Queue message");
			lock(_toSendMessages)
			{
				_toSendMessages.Enqueue(a_message);
			}
            _waitHandler.Set();
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
            _waitHandler.Set();
        }
        #endregion

        #region Read
        protected bool Write(SentMessage a_message)
		{
			try
			{
				FFLog.Log(EDbgCat.NetworkSerialization, "Start Writing : " + a_message.ToString() + " to " + _ffClient.Remote.ToString());
				byte[] messageData = a_message.Serialize();
				
				int length = messageData.Length;
				messageData = messageData.Insert(BitConverter.GetBytes(length),0);

                Client.GetStream().Write(messageData, 0, messageData.Length);
				FFLog.Log(EDbgCat.NetworkSerialization, "End Writing : " + messageData.Length);
				return true;
			}
			catch(IOException e)
			{
                FFLog.LogError("Couldn't write to stream." + e.Message);
                FFLog.LogError(EDbgCat.Socket, "Couldn't write to stream." + e.Message);
				return false;
			}
		}

        protected void HeartbeatTask()
        {
            while (_shouldRun && _ffClient != null && _ffClient.Clock != null)
            {
                _ffClient.Clock.UpdateHeartbeat();
                Thread.Sleep(50);
            }
        }

		protected override void Task()
		{
            while (_shouldRun && Client != null && _stream != null && _stream.CanWrite)
			{
                if (_ffClient.Clock.IsTimedout)
                {
                    _crashed = true;
                    break;
                }

                _waitHandler.WaitOne();
                if (_shouldRun && _toSendMessages.Count > 0)
				{
                    SentMessage toSend;
                    while (_toSendMessages.Count > 0)
                    {
                        lock (_toSendMessages)
                        {
                            toSend = _toSendMessages.Peek();
                        }
                        if (!TrySendMessage(toSend))
                        {
                            _shouldRun = false;
                            break;
                        }
                    }
                }
			}
            if (_crashed && _onConnectionLost != null)
                _onConnectionLost();
            _thread = null;

            FFLog.LogError(EDbgCat.Socket, "Stoping Writer Thread");
		}

        protected bool TrySendMessage(SentMessage toSend)
        {
            if (Write(toSend))
            {
                lock (_toSendMessages)
                {
                    _toSendMessages.Dequeue();
                }
                _ffClient.QueueWrittenMessage(toSend);
                if (_finalMessageRef == toSend)
                    return false;//Final message
            }
            else
            {
                _crashed = true;
                //Couldn't send message
                return false;
            }
            return true;
        }
		#endregion
	}
}