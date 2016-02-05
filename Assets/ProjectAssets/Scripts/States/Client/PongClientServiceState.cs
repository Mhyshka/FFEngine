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
        protected GenericMessageReceiver _ballMovementReceiver;
        #endregion

        #region State Methods
        internal override void Enter()
        {
            base.Enter();
            if(_ballMovementReceiver == null)
                _ballMovementReceiver = new GenericMessageReceiver(OnStartGame);
        }
        #endregion

        #region EventManagement
        protected override void RegisterForEvent()
        {
            base.RegisterForEvent();
            Engine.Receiver.RegisterReceiver(EDataType.Empty, _ballMovementReceiver);
        }

        protected override void UnregisterForEvent()
        {
            base.UnregisterForEvent();
            Engine.Receiver.UnregisterReceiver(EDataType.Empty, _ballMovementReceiver);
        }
        #endregion

        protected override void OnServicePlayerSmash()
        {
            Network.Message.MessageServiceRatio message = new MessageServiceRatio(_ratio);
            Engine.Network.MainClient.QueueMessage(message);
        }

        protected void OnStartGame(ReadMessage a_message)
        {
            RequestState((int)EPongGameState.Gameplay);
        }
    }
}