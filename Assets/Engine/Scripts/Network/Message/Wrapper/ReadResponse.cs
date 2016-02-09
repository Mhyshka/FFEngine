using UnityEngine;
using System.Collections;

namespace FF.Network.Message
{
    internal class ReadResponse : ReadMessage
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

        protected ERequestErrorCode _errorCode;
        internal ERequestErrorCode ErrorCode
        {
            get
            {
                return _errorCode;
            }
        }

        internal override EHeaderType HeaderType
        {
            get
            {
                return EHeaderType.Response;
            }
        }
        #endregion

        #region Constructors
        internal ReadResponse() : base()
        {
        }

        internal ReadResponse(MessageData a_data,
                                long a_timestamp,
                                long a_requestId,
                                ERequestErrorCode a_errorCode,
                                int a_channel)
        {
            _data = a_data;
            _timestamp = a_timestamp;
            _channel = a_channel;
            _requestId = a_requestId;
            _errorCode = a_errorCode;
        }
        #endregion

        #region Serialization
        public override void SerializeData(FFByteWriter stream)
        {
            base.SerializeData(stream);
            stream.Write(_requestId);
            stream.Write((short)_errorCode);
        }

        public override void LoadFromData(FFByteReader stream)
        {
            base.LoadFromData(stream);
            _requestId = stream.TryReadLong();
            _errorCode = (ERequestErrorCode)stream.TryReadShort();
        }
        #endregion
    }
}
