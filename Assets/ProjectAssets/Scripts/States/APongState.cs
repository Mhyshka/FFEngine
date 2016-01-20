using UnityEngine;
using System.Collections;

using FF.Logic;

namespace FF.Pong
{
    internal abstract class APongServerState : AGameState
    {
        #region Properties
        protected PongServerGameMode _pongServerGm;
        #endregion

        internal override void Enter()
        {
            base.Enter();
            _pongServerGm = _gameMode as PongServerGameMode;
        }
    }
}