using UnityEngine;
using System.Collections;
using System;

using FF.Network;
using FF.Network.Message;
using FF.Multiplayer;

namespace FF.Handler
{
    internal class MoveToSlot : ANetwork
    {
        #region Properties
        #endregion

        internal MoveToSlot(FFTcpClient a_client, SlotRef a_targetSlot, SimpleCallback a_onSuccess, IntCallback a_onFail) : base(a_client)
        {
            _onSuccess = a_onSuccess;
            _onFail = a_onFail;
            _isComplete = false;

            _request = new RequestMoveToSlot(a_targetSlot);
            _request.onSuccess += OnSuccess;
            _request.onFail += OnFail;
            _request.onCancel += OnCancel;
            _request.onTimeout += OnTimeout;
            _client.QueueMessage(_request);
        }
    }
}