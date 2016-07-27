using UnityEngine;
using System.Collections;

namespace FF.Network
{
    internal class ServerConnectionLostState : ConnectionLostState
    {
        internal ServerConnectionLostState() : base()
        {

        }

        public override void Enter(EClientConnectionState a_previousStateId)
        {
        }

        public override EClientConnectionState DoUpdate()
        {
            return EClientConnectionState.Disconnected;
        }

        public override void Exit(EClientConnectionState a_targetStateId)
        {
        }
    }
}