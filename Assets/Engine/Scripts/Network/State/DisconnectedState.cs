using UnityEngine;
using System.Collections;

namespace FF.Network
{
    internal class DisconnectedState : IFsmState<EClientConnectionState>
    {
        internal FFNetworkClient _client;

        internal DisconnectedState(FFNetworkClient a_client)
        {
            _client = a_client;
        }

        public EClientConnectionState ID
        {
            get
            {
                return EClientConnectionState.Disconnected;
            }
        }

        public virtual EClientConnectionState DoUpdate()
        {
            return ID;
        }

        public virtual void Enter(EClientConnectionState a_previousStateId)
        {
            _client.Close();
        }

        public virtual void Exit(EClientConnectionState a_targetStateId)
        {
        }
    }
}
