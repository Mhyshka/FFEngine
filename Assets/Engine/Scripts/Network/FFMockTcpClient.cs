using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using System.Net.Sockets;
using System.Net;

namespace FF.Networking
{
    internal class FFMockTcpClient : FFTcpClient
	{
        #region Message Read properties
        protected FFMockTcpClient _mirror;
        internal FFMockTcpClient Mirror
        {
            get
            {
                return _mirror;
            }
        }
		internal override void QueueReadMessage(FFMessage a_message)
		{
		}
        #endregion

        #region Constructors
        internal FFMockTcpClient(int a_networkId, IPEndPoint a_local, IPEndPoint a_remote)
        {
            NetworkID = a_networkId;
            _local = a_local;
            _remote = a_remote;
            _pendingSentRequest = new Dictionary<int, FFRequestMessage>();
            _pendingReadRequest = new Dictionary<int, FFRequestMessage>();
            _requestIndex = 0;
        }

        internal void GenereateMirror()
        {
            _mirror = new FFMockTcpClient(NetworkID, _remote, _local);
            _mirror.SetMirror(this);
        }

        protected void SetMirror(FFMockTcpClient a_mirror)
        {
            _mirror = a_mirror;
        }
		#endregion
		
		#region Interface	
		internal override void Stop()
		{
		}
		
		internal override void Close()
		{
		}
		
		internal override void StartWorkers()
		{
		}
		
		internal override void QueueMessage(FFMessage a_message)
		{
            if (a_message.HandleByMock)
            {
                a_message.Client = this;
                if (a_message is FFResponseMessage)
                {
                    FFResponseMessage res = a_message as FFResponseMessage;
                    _pendingReadRequest.Remove(res.requestId);
                }
                else if (a_message is FFRequestMessage)
                {
                    FFRequestMessage req = a_message as FFRequestMessage;
                    req.requestId = _requestIndex;
                    _pendingSentRequest.Add(req.requestId, req);
                    _requestIndex++;
                }
                if(a_message.onMessageSent != null)
                    a_message.onMessageSent();
                _mirror.Read(a_message);
            }
        }

        internal override void QueueFinalMessage(FFMessage a_message)
        {
            if (a_message.HandleByMock)
            {
                a_message.Client = this;
                if (a_message is FFResponseMessage)
                {
                    FFResponseMessage res = a_message as FFResponseMessage;
                    _pendingReadRequest.Remove(res.requestId);
                }
                else if (a_message is FFRequestMessage)
                {
                    FFRequestMessage req = a_message as FFRequestMessage;
                    req.requestId = _requestIndex;
                    _pendingSentRequest.Add(req.requestId, req);
                    _requestIndex++;
                }

                if (a_message.onMessageSent != null)
                    a_message.onMessageSent();
                _mirror.Read(a_message);
            }
        }

        protected void Read(FFMessage a_message)
        {
            FFMessage messageRead = a_message;
            FFLog.Log(EDbgCat.Networking, "Reading new message : " + messageRead.ToString());
            messageRead.Client = this;
            if (messageRead is FFRequestMessage)// Request
            {
                FFRequestMessage request = messageRead as FFRequestMessage;
                _pendingReadRequest.Add(request.requestId, request);
                request.Read();
            }
            else if (messageRead is FFRequestCancel)// Cancel Request
            {
                FFRequestCancel cancel = messageRead as FFRequestCancel;
                FFRequestMessage request = null;
                if (_pendingReadRequest.TryGetValue(cancel.requestId, out request))
                {
                    cancel.Read(request);
                }
            }
            else if (messageRead is FFResponseMessage)// Response
            {
                FFResponseMessage response = messageRead as FFResponseMessage;
                FFRequestMessage req = null;
                if (_pendingSentRequest.TryGetValue(response.requestId, out req))
                {
                    response.Read(req);
                    _pendingSentRequest.Remove(response.requestId);
                }
            }
            else// Message
            {
                messageRead.Read();
            }
        }
        #endregion
        internal override void DoUpdate()
        {
        }

        #region Connection
        internal override bool IsConnected
        {
            get
            {
                return true;
            }
        }

        internal override bool Connect()
		{
            return true;
        }
		
		internal override void ConnectionSuccess()
		{
        
		}

        internal override void ConnectionFailed()
        {
           
        }
		
		internal override void ConnectionLost()
		{
			
		}

        internal override void EndConnection(string a_reason)
        {
        }
		#endregion
	}
}