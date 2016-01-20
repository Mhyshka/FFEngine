using UnityEngine;
using System.Collections;

using FF.Network;
using FF.Network.Message;

namespace FF.Handler
{
    internal class ConfirmSwap : ANetwork
    {
        #region Properties
        #endregion

        internal ConfirmSwap(FFTcpClient a_client, string a_fromUsername, SimpleCallback a_onSuccess, IntCallback a_onFail) : base(a_client)
        {
            _onSuccess = a_onSuccess;
            _onFail = a_onFail;

            _request = new RequestConfirmSwap(a_fromUsername);
            
            _request.onFail += OnFail;
            _request.onCancel += OnCancel;
            _request.onSuccess += OnSuccess;
            _request.onTimeout += OnTimeout;

            _client.QueueMessage(_request);
        }
    }
}