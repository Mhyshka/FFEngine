using UnityEngine;
using System.Collections;

using FF.Logic;

namespace FF.Pong
{
    internal abstract class APongState : AGameState
    {
        #region Properties
        protected PongGameMode _pongGm;
        #endregion

        internal override void Enter()
        {
            base.Enter();
            _pongGm = _gameMode as PongGameMode;
        }
    }
}