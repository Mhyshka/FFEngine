﻿using UnityEngine;
using System.Collections;

namespace FF.Network.Message
{
    internal class FFRequestForClient
    {
        /*protected FFTcpClient _client;
        protected RequestHeader _request;

        internal FFClientCallback onSuccess = null;
        internal FFClientCallback onFail = null;
        internal FFClientCallback onTimeout = null;
        internal FFClientCallback onCancel = null;

        internal FFRequestForClient(RequestHeader a_request, FFTcpClient a_client)
        {
            _client = a_client;
            _request = a_request;

            _request.onSuccess += OnSuccess;
            _request.onFail += OnFail;
            _request.onTimeout += OnTimeout;
            _request.onCancel += OnCancel;
        }

        protected void OnSuccess()
        {
            if (onSuccess != null)
                onSuccess(_client);

            TearDown();
        }

        protected void OnFail()
        {
            if (onFail != null)
                onFail(_client);

            TearDown();
        }

        protected void OnTimeout()
        {
            if (onTimeout != null)
                onTimeout(_client);

            TearDown();
        }

        protected void OnCancel()
        {
            if (onCancel != null)
                onCancel(_client);

            TearDown();
        }

        protected void TearDown()
        {
            _request.onSuccess -= OnSuccess;
            _request.onFail -= OnFail;
            _request.onTimeout -= OnTimeout;
            _request.onCancel -= OnCancel;

            onSuccess = null;
            onFail = null;
            onCancel = null;
            onTimeout = null;

            _request = null;
            _client = null;
        }*/
    }
}