using UnityEngine;
using System.Collections;
using System;

namespace FF.Network
{
    internal abstract class ConnectionLostState : IFsmState<EClientConnectionState>
    {

        public EClientConnectionState ID
        {
            get
            {
                return EClientConnectionState.ConnectionLost;
            }
        }

        public virtual EClientConnectionState DoUpdate()
        {
            return ID;
        }

        public virtual void Enter(EClientConnectionState a_previousStateId)
        {
        }

        public virtual void Exit(EClientConnectionState a_targetStateId)
        {
        }
    }
}
