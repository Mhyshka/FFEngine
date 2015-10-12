using UnityEngine;
using System.Collections;
using System;

namespace FF.Networking
{
    internal class FFMoveToSlotHandler : FFNetworkHandler
    {
        #region Properties
        #endregion

        internal FFMoveToSlotHandler(FFTcpClient a_client, FFSlotRef a_targetSlot, SimpleCallback a_onSuccess, IntCallback a_onFail) : base(a_client)
        {
            _onSuccess = a_onSuccess;
            _onFail = a_onFail;
            _isComplete = false;

            _request = new FFMoveToSlotRequest(a_targetSlot);
            _request.onSuccess += OnSuccess;
            _request.onFail += OnFail;
            _request.onCancel += OnCancel;
            _request.onTimeout += OnTimeout;
            _client.QueueMessage(_request);
        }
    }
}