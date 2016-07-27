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
    internal class FFLoopbackClient : FFNetworkClient
	{
        #region Message Read properties
        protected FFLoopbackClient _mirror;
        internal FFLoopbackClient Mirror
        {
            get
            {
                return _mirror;
            }
        }

		internal override void QueueReadMessage(ReadMessage a_message)
		{
		}
        #endregion

        #region Constructors
        internal FFLoopbackClient(int a_networkId, IPEndPoint a_local, IPEndPoint a_remote)
        {
            _networkID = a_networkId;
            _local = a_local;
            _remote = a_remote;

            _writtenMessages = new Queue<SentMessage>();
            _pendingSentRequest = new Dictionary<long, SentRequest>();
            _pendingReadRequest = new Dictionary<long, ReadRequest>();
            _readMessages = new Queue<ReadMessage>();
            _readRequestToCancel = new Queue<long>();
            _sentRequestToRemove = new Queue<long>();

            //No state necessarry
            _currentState = null;
            _targetState = null;
        }

        internal void GenereateMirror()
        {
            _mirror = new FFLoopbackClient(NetworkID, _remote, _local);
            _mirror.SetMirror(this);
        }

        protected void SetMirror(FFLoopbackClient a_mirror)
        {
            _mirror = a_mirror;
        }
		#endregion
		
		#region Interface	
		internal override void Close()
		{
            //Clearing callbacks
            onVersionCompatibilityVerificationSuccess = null;
            onIdCheckCompleted = null;
            onConnectionEnded = null;
        }
		
		internal override void StartWorkers()
		{
		}

        internal override void QueueMessage(SentMessage a_message)
        {
            if (a_message.IsHandleByMock)
            {
                a_message.Client = this;
                a_message.PostWrite();

                ReadMessage readMessage = new ReadMessage(a_message.Data, a_message.Timestamp, a_message.Channel.GetHashCode());
                _mirror.Read(readMessage);
            }
        }

        internal override void QueueRequest(SentRequest a_request)
        {
            if (a_request.IsHandleByMock)
            {
                lock (_pendingSentRequest)
                {
                    _pendingSentRequest.Add(a_request.RequestId, a_request);
                }
                a_request.Client = this;
                a_request.PostWrite();
                ReadRequest readRequest = new ReadRequest(a_request.Data, a_request.Timestamp, a_request.RequestId, a_request.Channel.GetHashCode());

                _mirror.Read(readRequest);
            }
        }

        internal override void QueueResponse(SentResponse a_response)
        {
            if (a_response.IsHandleByMock)
            {
                lock (_pendingReadRequest)
                {
                    _pendingReadRequest.Remove(a_response.RequestId);
                }
                a_response.Client = this;
                a_response.PostWrite();

                ReadResponse readMessage = new ReadResponse(a_response.Data, a_response.Timestamp, a_response.RequestId, a_response.ErrorCode, a_response.Channel.GetHashCode());
                _mirror.Read(readMessage);
            }
        }

        internal override void QueueFinalMessage(SentMessage a_message)
        {
            QueueMessage(a_message);
        }

        protected void Read(ReadMessage a_message)
        {
            FFLog.Log(EDbgCat.ServerMock, "Reading new message : " + a_message.ToString());

            a_message.Client = this;

            if (a_message is ReadRequest)// Request
            {
                ReadRequest request = a_message as ReadRequest;
                _pendingReadRequest.Add(request.RequestId, request);
            }

            List<BaseReceiver> receivers = Engine.Receiver.ReceiversForChannel(a_message.Channel);
            if (receivers != null)
            {
                receivers = new List<BaseReceiver>();
                foreach (BaseReceiver each in Engine.Receiver.ReceiversForChannel(a_message.Channel))
                {
                    receivers.Add(each);
                }

                foreach (BaseReceiver each in receivers)
                {
                    each.Read(a_message);
                }
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
		#endregion
	}
}