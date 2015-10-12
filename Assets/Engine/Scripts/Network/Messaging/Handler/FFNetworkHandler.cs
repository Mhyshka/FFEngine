using UnityEngine;
using System.Collections;

namespace FF.Networking
{
    internal abstract class FFNetworkHandler : FFHandler
    {
        #region Properties
        protected FFTcpClient _client;
        protected SimpleCallback _onCancel = null;
        protected IntCallback _onFail = null;
        protected FFRequestMessage _request = null;
        #endregion

        #region Constructor
        internal FFNetworkHandler(FFTcpClient a_client)
        {
            _client = a_client;
        }
        #endregion
        internal virtual void Cancel()
        {
            FFLog.LogError("Cancel Handler.");
            FFRequestCancel cancel = new FFRequestCancel(_request);
            cancel.requestId = _request.requestId;
            _client.QueueMessage(cancel);
        }

        protected virtual void OnCancel()
        {
            FFLog.LogError("On Cancel Handler.");
            _isComplete = true;
            if (_onCancel != null)
                _onCancel();
        }

        protected virtual void OnFail(int a_errorCode)
        {
            _isComplete = true;
            if (_onFail != null)
                _onFail(a_errorCode);
        }

        protected virtual void OnTimeout()
        {
            _isComplete = true;
            if (_onFail != null)
                _onFail(-1);
        }

        internal override void OnComplete()
        {
            _request.onCancel -= OnCancel;
            _request.onFail -= OnFail;
            _request.onSuccess -= OnSuccess;
            _request.onTimeout -= OnTimeout;
            _request.Unregister();
            _request = null;
            
            _onFail = null;
            _onCancel = null;
            base.OnComplete();
        }
    }
}