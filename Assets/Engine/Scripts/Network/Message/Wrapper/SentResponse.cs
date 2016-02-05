using UnityEngine;
using System.Collections;

namespace FF.Network.Message
{
    internal class SentResponse : SentMessage
    {
        #region properties
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
        internal SentResponse(MessageData a_data,
                                string a_channel,
                                long a_requestId,
                                ERequestErrorCode a_errorCode,
                                bool a_isMandatory = true,
                                bool a_isHandleByMock = false) : base(a_data, a_channel, a_isMandatory, a_isHandleByMock)
        {
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