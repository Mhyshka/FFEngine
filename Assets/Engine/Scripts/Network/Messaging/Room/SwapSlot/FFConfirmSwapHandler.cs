using UnityEngine;
using System.Collections;

namespace FF.Networking
{
    internal class FFConfirmSwapHandler : FFNetworkHandler
    {
        #region Properties
        #endregion

        internal FFConfirmSwapHandler(FFTcpClient a_client, string a_fromUsername, SimpleCallback a_onSuccess, IntCallback a_onFail) : base(a_client)
        {
            _onSuccess = a_onSuccess;
            _onFail = a_onFail;

            _request = new FFConfirmSwapRequest(a_fromUsername);
            
            _request.onFail += OnFail;
            _request.onCancel += OnCancel;
            _request.onSuccess += OnSuccess;
            _request.onTimeout += OnTimeout;

            _client.QueueMessage(_request);
        }
    }
}