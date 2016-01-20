using UnityEngine;
using System.Collections;
using System;

using FF.Network;
using FF.Network.Message;
using FF.Multiplayer;

namespace FF.Handler
{
    internal class JoinRoom : ANetwork
    {
        #region Properties
        #endregion

        internal JoinRoom(FFTcpClient a_client, FFNetworkPlayer a_player, SimpleCallback a_onSuccess, IntCallback a_onFail) : base(a_client)
        {
            _isComplete = false;
            _onSuccess = a_onSuccess;
            _onFail = a_onFail;

            _request = new RequestJoinRoom(a_player);
            _request.onFail += OnFail;
            _request.onCancel += OnCancel;
            _request.onSuccess += OnSuccess;
            _request.onTimeout += OnTimeout;
            _client.QueueMessage(_request);
        }

        #region Callbacks
        #endregion
    }
}