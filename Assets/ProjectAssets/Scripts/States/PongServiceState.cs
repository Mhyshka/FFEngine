using UnityEngine;
using System.Collections;

using FF.Logic;
using System;

namespace FF.Pong
{
    internal class PongServiceState : APongServerState
    {
        #region properties
        #endregion

        internal override int ID
        {
            get
            {
                return (int)EPongGameState.Service;
            }
        }

        #region State Methods
        internal override void Enter()
        {
            base.Enter();
            /*SnapBallToRacket();
            Engine.Inputs.ManagerForClient(_pongServerGm.serviceClientId).EventForKey(EInputEventKey.Action).onDown += OnActionEvent;*/
        }

        internal override void Exit()
        {
            base.Exit();
            //Engine.Inputs.ManagerForClient(_pongServerGm.serviceClientId).EventForKey(EInputEventKey.Action).onDown -= OnActionEvent;
        }
        #endregion

        protected void OnActionEvent()
        {

        }

        protected void SnapBallToRacket()
        {
        }
    }
}