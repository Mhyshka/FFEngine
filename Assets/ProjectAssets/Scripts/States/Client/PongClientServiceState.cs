using UnityEngine;
using System.Collections;

using FF.Logic;
using System;

using FF.Network.Receiver;
using FF.Network.Message;

namespace FF.Pong
{
    internal class PongClientServiceState : PongServiceState
    {
        #region Inspector Properties
        #endregion

        #region properties
        protected GenericMessageReceiver _startGameReceiver;
        #endregion

        #region State Methods
        internal override void Enter()
        {
            base.Enter();
            _timeOffset = (float)new TimeSpan(_pongGm.ServiceTimestamp).TotalSeconds;
            if(_startGameReceiver == null)
                _startGameReceiver = new GenericMessageReceiver(OnStartGame);
        }
        #endregion

        #region EventManagement
        protected override void RegisterForEvent()
        {
            base.RegisterForEvent();
            Engine.Receiver.RegisterReceiver(EMessageChannel.BallMovement.ToString(), _startGameReceiver);//When we detect a ball movement !
        }

        protected override void UnregisterForEvent()
        {
            base.UnregisterForEvent();
            Engine.Receiver.UnregisterReceiver(EMessageChannel.BallMovement.ToString(), _startGameReceiver);
        }
        #endregion

        protected override void OnServicePlayerSmash()
        {
            MessageFloatData data = new MessageFloatData(_ratio);
            SentMessage message = new SentMessage(data,
                                                  EMessageChannel.ServiceRatio.ToString());
            Engine.Network.MainClient.QueueMessage(message);
        }

        protected void OnStartGame(ReadMessage a_message)
        {
            RequestState((int)EPongGameState.Gameplay);
        }
    }
}