using UnityEngine;
using System.Collections;

namespace FF.Network.Message
{
    internal class SentRequest : SentMessage
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

        protected float _timeoutDuration;
        internal float TimeoutDuration
        {
            get
            {
                return _timeoutDuration;
            }
        }

        internal override EHeaderType HeaderType
        {
            get
            {
                return EHeaderType.Request;
            }
        }

        protected override bool IsCompleteOnSent
        {
            get
            {
                return false;
            }
        }

        protected float _timeElapsed = 0f;


        protected bool _isComplete = false;
        internal bool IsComplete
        {
            get
            {
                return _isComplete;
            }
        }
        #endregion

        #region Constructors
        internal SentRequest(MessageData a_data,
                            string a_channel,
                            long a_requestId,
                            float a_timeoutDuration = 5f,
                            bool a_isMandatory = true,
                            bool a_isHandleByMock = false) : base(a_data, a_channel, a_isMandatory, a_isHandleByMock)
        {
            _requestId = a_requestId;
            _timeoutDuration = a_timeoutDuration;
        }
        #endregion

        #region Timeout
        internal bool CheckForTimeout(float a_delta)
        {
            _timeElapsed += a_delta;
            return _timeElapsed >= TimeoutDuration;
        }
        #endregion

        #region Timeout
        internal void Timeout()
        {
            OnFail(ERequestErrorCode.Timeout, null);
        }
        #endregion

        #region Cancel
        internal void Cancel(bool a_shouldNotifyOther = false)
        {
            if (a_shouldNotifyOther)
            {
                SentMessage cancel = new SentMessage(new MessageLongData(RequestId),
                                                    EMessageChannel.CancelRequest.ToString(),
                                                    true,
                                                    true);
            }

            OnFail(ERequestErrorCode.LocalCanceled, null);
        }
        #endregion

        #region Success
        internal ReadResponseCallback onSucces = null;
        internal void OnSuccess(ReadResponse a_response)
        {
            if (onSucces != null)
                onSucces(a_response);
            OnComplete();
        }
        #endregion

        #region Fail
        internal RequestFailCallback onFail = null;
        internal void OnFail(ERequestErrorCode a_errorCode, ReadResponse a_response)
        {
            if (onFail != null)
                onFail(a_errorCode, a_response);

            OnComplete();
        }
        #endregion

        #region Connection Lost
        internal override void PostWrite()
        {
            _client.onConnectionLost += OnConnectionLost;
            base.PostWrite();
        }

        protected virtual void OnConnectionLost(FFTcpClient a_client)
        {
            OnFail(ERequestErrorCode.LocalConnectionIssue, null);
        }
        #endregion

        #region Complete
        protected override void OnComplete()
        {
            _isComplete = true;
            base.OnComplete();

            onSucces = null;
            onFail = null;

            _client.onConnectionLost -= OnConnectionLost;
            _client.RemoveSentRequest(_requestId);
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