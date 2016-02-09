using UnityEngine;
using System.Collections;

namespace FF.Network.Message
{
    internal class ReadRequest : ReadMessage
    {
        #region Properties
        protected long _requestId;
        internal long RequestId
        {
            get
            {
                return _requestId;
            }
        }

        internal override EHeaderType HeaderType
        {
            get
            {
                return EHeaderType.Request;
            }
        }
        #endregion

        #region Constructors
        internal ReadRequest() : base()
        {
        }

        internal ReadRequest(MessageData a_data,
                                long a_timestamp,
                                long a_requestId,
                                int a_channel)
        {
            _data = a_data;
            _timestamp = a_timestamp;
            _channel = a_channel;
            _requestId = a_requestId;
        }
        #endregion

        #region Callbacks
        internal SimpleCallback onCanceled = null;

        internal void OnCanceled()
        {
            if (onCanceled != null)
                onCanceled();

            OnComplete();
        }
        #endregion

        #region Controls
        internal void Success(MessageData a_data)
        {
            SentResponse response = new SentResponse(a_data, _requestId, ERequestErrorCode.Success);
            _client.QueueResponse(response);

            OnComplete();
        }

        internal void Success(SentResponse a_response)
        {
            _client.QueueResponse(a_response);

            OnComplete();
        }

        internal void FailWithResponse(ERequestErrorCode a_errorCode, MessageData a_data)
        {
            SentResponse response = new SentResponse(a_data, _requestId, a_errorCode);
            _client.QueueResponse(response);

            OnComplete();
        }

        internal void FailWithResponse(SentResponse a_response)
        {
            _client.QueueResponse(a_response);
            
            OnComplete();
        }

        internal void FailWithoutResponse(ERequestErrorCode a_errorCode)
        {
            OnComplete();
        }
        #endregion

        #region OnComplete
        protected override void OnComplete()
        {
            onCanceled = null;
            base.OnComplete();
        }
        #endregion

        #region Serialization
        public override void SerializeData(FFByteWriter stream)
        {
            base.SerializeData(stream);
            stream.Write(_requestId);
        }

        public override void LoadFromData(FFByteReader stream)
        {
            base.LoadFromData(stream);
            _requestId = stream.TryReadLong();
        }
        #endregion
    }
}
