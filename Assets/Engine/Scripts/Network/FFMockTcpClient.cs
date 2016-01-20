using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using System.Net.Sockets;
using System.Net;

using FF.Network.Receiver;
using FF.Network.Message;

namespace FF.Network
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
		internal override void QueueReadMessage(AMessage a_message)
		{
		}
        #endregion

        #region Constructors
        internal FFMockTcpClient(int a_networkId, IPEndPoint a_local, IPEndPoint a_remote)
        {
            NetworkID = a_networkId;
            _local = a_local;
            _remote = a_remote;
            _pendingSentRequest = new Dictionary<int, ARequest>();
            _pendingReadRequest = new Dictionary<int, ARequest>();
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
		
		internal override void QueueMessage(AMessage a_message)
		{
            if (a_message.HandleByMock)
            {
                a_message.Client = this;
                if (a_message is AResponse)
                {
                    AResponse res = a_message as AResponse;
                    _pendingReadRequest.Remove(res.requestId);
                }
                else if (a_message is ARequest)
                {
                    ARequest req = a_message as ARequest;
                    req.requestId = _requestIndex;
                    _pendingSentRequest.Add(req.requestId, req);
                    _requestIndex++;
                }

                if(a_message.onMessageSent != null)
                    a_message.onMessageSent();

                _mirror.Read(a_message);
            }
        }

        internal override void QueueFinalMessage(AMessage a_message)
        {
            if (a_message.HandleByMock)
            {
                a_message.Client = this;
                if (a_message is AResponse)
                {
                    AResponse res = a_message as AResponse;
                    _pendingReadRequest.Remove(res.requestId);
                }
                else if (a_message is ARequest)
                {
                    ARequest req = a_message as ARequest;
                    req.requestId = _requestIndex;
                    _pendingSentRequest.Add(req.requestId, req);
                    _requestIndex++;
                }

                if (a_message.onMessageSent != null)
                    a_message.onMessageSent();

                _mirror.Read(a_message);
            }
        }

        protected void Read(AMessage a_message)
        {
            AMessage messageRead = a_message;
            FFLog.Log(EDbgCat.Networking, "Reading new message : " + messageRead.ToString());

            messageRead.Client = this;
            if (messageRead is ARequest)
            {
                ARequest request = messageRead as ARequest;
                _pendingReadRequest.Add(request.requestId, request);
            }

            foreach (BaseReceiver each in Engine.Receiver.ReceiversForType(messageRead.Type))
            {
                each.Read(messageRead);
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