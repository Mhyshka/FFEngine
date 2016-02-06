using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using FF.Network.Message;
using FF.Logic;

namespace FF.Pong
{
    internal class PongClientLoadingState : MultiClientLoadingState
    {
        #region Racket Position
        protected override void OnRacketTargetReached()
        {
            base.OnRacketTargetReached();
            SentMessage message = new SentMessage(new MessageEmptyData(),
                                                    EMessageChannel.LoadingReady.ToString());
            Engine.Network.MainClient.QueueMessage(message);
        }
        #endregion
    }
}