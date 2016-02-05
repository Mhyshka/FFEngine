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
        protected GenericMessageReceiver<MessagePongBallMovement, MessageHeader> _ballMovementReceiver;
        #endregion

        #region State Methods
        internal override void Enter()
        {
            base.Enter();
            if(_ballMovementReceiver == null)
                _ballMovementReceiver = new GenericMessageReceiver<MessagePongBallMovement, MessageHeader>(OnBallMovementReceived);
        }
        #endregion

        #region EventManagement
        protected override void RegisterForEvent()
        {
            base.RegisterForEvent();
            Engine.Receiver.RegisterReceiver(EDataType.M_PongBallMovement, _ballMovementReceiver);
        }

        protected override void UnregisterForEvent()
        {
            base.UnregisterForEvent();
            Engine.Receiver.UnregisterReceiver(EDataType.M_PongBallMovement, _ballMovementReceiver);
        }
        #endregion

        protected override void OnServicePlayerSmash()
        {
            Network.Message.MessageServiceRatio message = new MessageServiceRatio(_ratio);
            Engine.Network.MainClient.QueueMessage(message);
        }

        protected void OnBallMovementReceived(MessageHeader a_header, MessagePongBallMovement a_ballMovementMessage)
        {
            RequestState((int)EPongGameState.Gameplay);
        }
    }
}