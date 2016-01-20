using UnityEngine;
using System.Collections;
using System;

using FF.Network;
using FF.Network.Message;
using FF.Multiplayer;

namespace FF.Handler
{
    internal class SlotSwap : ANetwork
    {
        #region Properties
        #endregion

        internal SlotSwap(FFTcpClient a_client, SlotRef a_slotRef, SimpleCallback a_onSuccess, IntCallback a_onFail) : base(a_client)
        {
            Engine.Game.NetPlayer.busyCount++;

            _onSuccess = a_onSuccess;
            _onFail = a_onFail;

            _request = new RequestSlotSwap(a_slotRef);
            _request.onFail += OnFail;
            _request.onSuccess += OnSuccess;
            _request.onTimeout += OnTimeout;
            _request.onCancel += OnCancel;
            _client.QueueMessage(_request);
        }

        internal override void OnComplete()
        {
            base.OnComplete();
            Engine.Game.NetPlayer.busyCount--;
        }
    }
}