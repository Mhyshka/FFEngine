using UnityEngine;

using FF.Multiplayer;

using FF.Network.Message;
using FF.Network.Receiver;

namespace FF.Pong
{
    internal class PongClientServiceChallengeState : PongServiceChallengeState
    {
        #region Inspector Properties
        #endregion

        #region Properties
        protected GenericMessageReceiver _completeReceiver = null;
        #endregion

        #region State Methods
        internal override void Enter()
        {
            base.Enter();
            if (_completeReceiver == null)
                _completeReceiver = new GenericMessageReceiver(OnChallengeCompleteReceived);
        }

        internal override void Exit()
        {
            base.Exit();
            _bounceCount = -1;
        }
        #endregion

        #region Event Management
        protected override void RegisterForEvent()
        {
            base.RegisterForEvent();
            Engine.Receiver.RegisterReceiver(EDataType.M_LoadingComplete, _completeReceiver);
        }

        protected override void UnregisterForEvent()
        {
            base.UnregisterForEvent();
            Engine.Receiver.UnregisterReceiver(EDataType.M_LoadingComplete, _completeReceiver);
        }

        protected void OnChallengeCompleteReceived(ReadMessage a_message)
        {
            MessageLoadingComplete data;
            RequestState(outState.ID);
        }
        #endregion

        protected override void OnAnimationComplete()
        {
            base.OnAnimationComplete();
            Network.Message.MessageLoadingReady readyMessage = new Network.Message.MessageLoadingReady();
            Engine.Network.MainClient.QueueMessage(readyMessage);
        }


        internal void SetTargetBounces(int a_target)
        {
            _bounceCount = a_target;
        }
    }
}