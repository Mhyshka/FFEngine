using UnityEngine;
using System.Collections;
using System;

namespace FF.Networking
{
    internal class FFSlotSwapHandler : FFNetworkHandler
    {
        #region Properties
        #endregion

        internal FFSlotSwapHandler(FFTcpClient a_client, FFSlotRef a_slotRef, SimpleCallback a_onSuccess, IntCallback a_onFail) : base(a_client)
        {
            FFEngine.Network.Player.busyCount++;

            _onSuccess = a_onSuccess;
            _onFail = a_onFail;

            _request = new FFSlotSwapRequest(a_slotRef);
            _request.onFail += OnFail;
            _request.onSuccess += OnSuccess;
            _request.onTimeout += OnTimeout;
            _request.onCancel += OnCancel;
            _client.QueueMessage(_request);
        }

        internal override void OnComplete()
        {
            base.OnComplete();
            FFEngine.Network.Player.busyCount--;
        }
    }
}