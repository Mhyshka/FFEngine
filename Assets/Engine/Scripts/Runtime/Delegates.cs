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
    internal delegate void IntIntCallback(int a_val1, int a_val2);

    internal delegate void FFVersionCalback(FFVersion a_version);


    internal delegate void PlayerDictionaryLoadingCallback(PlayerDictionary<PlayerLoadingWrapper> a_loadingState);

    internal delegate void RequestFailCallback(ERequestErrorCode a_errorCode, ReadResponse a_readResponse);
    internal delegate void ReadMessageCallback(ReadMessage a_readMessage);
    internal delegate void ReadResponseCallback(ReadResponse a_readMessage);

    internal delegate void MessageSentCallback(SentMessage a_message);
    internal delegate void RequestSuccessForMessageCallback(ReadResponse a_readResponse, SentMessage a_message);
    internal delegate void RequestFailForMessageCallback(ERequestErrorCode a_errorCode, ReadResponse a_readResponse, SentMessage a_message);

    internal delegate void FFIntClientCallback(FFNetworkClient a_client, int a_value);
    internal delegate void FFFloatClientCallback(FFNetworkClient a_client, float a_val);
    internal delegate void FFDoubleClientCallback(FFNetworkClient a_client, double a_val);
    internal delegate void FFIdCheckClientCallback(FFNetworkClient a_client, int a_serverId, int a_playerId);
    internal delegate void FFVersionClientCallback(FFNetworkClient a_client, FFVersion a_serverVersion, FFVersion a_playerVersion);
    internal delegate void FFClientCallback(FFNetworkClient a_client);
    internal delegate void FFRequestForClientCallback(FFNetworkClient a_client, ReadResponse a_response);
    internal delegate void FFDisconnectedCallback(FFNetworkClient a_client, string a_reason);

    internal delegate void FFClientsCallback(List<FFNetworkClient> a_clients);
    internal delegate void FFBoolByClientsCallback(Dictionary<FFNetworkClient, bool> a_result);
    internal delegate void FFClientsBroadcastCallback(Dictionary<FFNetworkClient, ReadResponse> a_successClients, Dictionary<FFNetworkClient, ReadResponse> a_failClients);
    #endregion
}