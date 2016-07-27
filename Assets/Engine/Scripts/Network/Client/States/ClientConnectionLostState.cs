using UnityEngine;
using System.Collections;

namespace FF.Network
{
    internal class ClientConnectionLostState : ConnectionLostState
    {
        FFClientWrapper _client;

        internal ClientConnectionLostState(FFClientWrapper a_client) : base()
        {
            _client = a_client;
        }

        public override void Enter(EClientConnectionState a_previousStateId)
        {
            _client.OnConnectionLostOnMt();
            _client.Connect();
        }

        public override EClientConnectionState DoUpdate()
        {
            return EClientConnectionState.Connection;
        }

        public override void Exit(EClientConnectionState a_targetStateId)
        {
        }
    }
}