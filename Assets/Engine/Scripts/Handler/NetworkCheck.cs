using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using FF.Network;
using FF.Network.Message;

namespace FF.Handler
{
    internal class NetworkCheck : BroadcastRequest
    {
        internal NetworkCheck(FFClientsBroadcastCallback a_onSuccess,
                                        FFClientsBroadcastCallback a_onFail,
                                        FFClientCallback a_onSuccessForClient,
                                        FFClientCallback a_onFailForClient) : base(a_onSuccess, a_onFail, a_onSuccessForClient, a_onFailForClient)
        {
        }

        protected override void SendRequestToClient(FFTcpClient a_client)
        {
            RequestEmpty empty = new RequestEmpty();
            FFRequestForClient rfc = new FFRequestForClient(empty, a_client);

            rfc.onSuccess += OnSuccessForClient;
            rfc.onFail += OnFailForClient;
            rfc.onTimeout += OnTimeoutForClient;
            rfc.onCancel += OnCancelForClient;

            a_client.QueueMessage(empty);
        }
    }
}