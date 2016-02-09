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
            Engine.Receiver.RegisterReceiver(EMessageChannel.Next.ToString(), _completeReceiver);
        }

        protected override void UnregisterForEvent()
        {
            base.UnregisterForEvent();
            Engine.Receiver.UnregisterReceiver(EMessageChannel.Next.ToString(), _completeReceiver);
        }

        protected void OnChallengeCompleteReceived(ReadMessage a_message)
        {
            _pongGm.ServiceTimestamp = a_message.Client.ConvertToLocalTime(a_message.Timestamp);
            RequestState(outState.ID);
        }
        #endregion

        protected override void OnAnimationComplete()
        {
            base.OnAnimationComplete();
            SentMessage message = new SentMessage(new MessageEmptyData(),
                                                    EMessageChannel.Ready.ToString());
            Engine.Network.MainClient.QueueMessage(message);
        }


        internal void SetTargetBounces(int a_target)
        {
            _bounceCount = a_target;
        }
    }
}