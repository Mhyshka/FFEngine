using UnityEngine;
using System.Collections;

using FF.Network.Message;

namespace FF.Handler
{
    internal class SentRequestForClient : ABaseHandler
    {
        #region Properties
        internal MessageSentCallback onMessageSent = null;
        internal RequestSuccessForMessageCallback onSuccess = null;
        internal RequestFailForMessageCallback onFail = null;

        protected SentRequest _request = null;
        #endregion

        internal SentRequestForClient(SentRequest a_request)
        {
            _request = a_request;
            _request.onMessageSent += OnMessageSent;
            _request.onSucces += OnSuccess;
            _request.onFail += OnFail;
        }

        protected void OnMessageSent()
        {
            if (onMessageSent != null)
                onMessageSent(_request);
        }

        protected void OnSuccess(ReadResponse a_response)
        {
            if (onSuccess != null)
                onSuccess(a_response, _request);

            Complete();
        }

        protected void OnFail(ERequestErrorCode a_errorCode, ReadResponse a_response)
        {
            if (onFail != null)
                onFail(a_errorCode, a_response, _request);
            Complete();
        }

        internal override void Complete()
        {
            base.Complete();

            _request.onMessageSent -= OnMessageSent;
            _request.onSucces -= OnSuccess;
            _request.onFail -= OnFail;

            onFail = null;
            onSuccess = null;
            onMessageSent = null;
        }
    }
}
