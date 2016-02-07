using UnityEngine;
using System.Collections.Generic;

using FF.Network;
using FF.Multiplayer;
using FF.Logic;
using FF.Network.Message;

namespace FF
{
	#region Callbacks
	internal delegate void SimpleCallback();
    internal delegate void BoolCallback(bool a_val);
    internal delegate void FloatCallback(float a_val);
    internal delegate void DoubleCallback(double a_val);
    internal delegate void IntCallback(int a_val);
    internal delegate void StringCallback(string a_string);
    internal delegate void ListIntCallback(List<int> a_progress);


    internal delegate void PlayerDictionaryLoadingCallback(PlayerDictionary<PlayerLoadingWrapper> a_loadingState);

    internal delegate void RequestFailCallback(ERequestErrorCode a_errorCode, ReadResponse a_readResponse);
    internal delegate void ReadMessageCallback(ReadMessage a_readMessage);
    internal delegate void ReadResponseCallback(ReadResponse a_readMessage);

    internal delegate void FFIntClientCallback(FFTcpClient a_client, int a_value);
    internal delegate void FFDoubleClientCallback(FFTcpClient a_client, double a_val);
    internal delegate void FFClientCallback(FFTcpClient a_client);
    internal delegate void FFRequestForClientCallback(FFTcpClient a_client, ReadResponse a_response);
    internal delegate void FFDisconnectedCallback(FFTcpClient a_client, string a_reason);

    internal delegate void FFClientsCallback(List<FFTcpClient> a_clients);
    internal delegate void FFBoolByClientsCallback(Dictionary<FFTcpClient, bool> a_result);
    internal delegate void FFClientsBroadcastCallback(Dictionary<FFTcpClient, ReadResponse> a_successClients, Dictionary<FFTcpClient, ReadResponse> a_failClients);
    #endregion
}