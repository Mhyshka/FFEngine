using UnityEngine;
using System.Collections;
using System;

namespace FF.Network
{
    internal class EmptyNetworkState : IFsmState<EClientConnectionState>
    {
        public EClientConnectionState ID
        {
            get
            {
                return EClientConnectionState.Connected;
            }
        }

        public EClientConnectionState DoUpdate()
        {
            return ID;
        }

        public void Enter(EClientConnectionState a_previousStateId)
        {
        }

        public void Exit(EClientConnectionState a_targetStateId)
        {
        }
    }
}