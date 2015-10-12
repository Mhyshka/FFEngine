using UnityEngine;
using System.Collections;
using System;

namespace FF.Networking
{
    internal class FFJoinRoomHandler : FFNetworkHandler
    {
        #region Properties
        #endregion

        internal FFJoinRoomHandler(FFTcpClient a_client, FFNetworkPlayer a_player, SimpleCallback a_onSuccess, IntCallback a_onFail) : base(a_client)
        {
            _isComplete = false;
            _onSuccess = a_onSuccess;
            _onFail = a_onFail;

            _request = new FFJoinRoomRequest(a_player);
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