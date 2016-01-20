using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using FF.Handler;
using FF.Network;
using FF.Network.Message;
using FF.Multiplayer;
using FF.Logic;

namespace FF.Pong
{
    internal class PongServerLoadingState : MultiServerLoadingState
    {
        #region Racket Position
        protected override void OnRacketTargetReached()
        {
            base.OnRacketTargetReached();
            Engine.Network.Server.LoopbackClient.Mirror.QueueMessage(new MessageLoadingReady());
        }
        #endregion
    }
}