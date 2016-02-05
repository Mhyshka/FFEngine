using UnityEngine;
using System.Collections;

using FF.Network.Message;
using FF.Network.Receiver;

namespace FF.Pong
{
    internal class PongClientGameMode : PongGameMode
    {
        #region Properties
        protected GenericMessageReceiver<MessageServiceChallengeInfo, MessageHeader> _challengeInfosReceiver = null;
        #endregion

        #region State Methods
        protected override void Enter()
        {
            _challengeInfosReceiver = new GenericMessageReceiver<MessageServiceChallengeInfo, MessageHeader>(OnChallengeInfosReceived);
            base.Enter();
        }
        #endregion

        #region Event Management
        protected override void RegisterForEvent()
        {
            base.RegisterForEvent();
            Engine.Receiver.RegisterReceiver(EDataType.M_ServiceChallengeInfo, _challengeInfosReceiver);
        }

        protected override void UnregisterForEvent()
        {
            base.UnregisterForEvent();
            Engine.Receiver.UnregisterReceiver(EDataType.M_ServiceChallengeInfo, _challengeInfosReceiver);
        }

        protected void OnChallengeInfosReceived(MessageHeader a_header, MessageServiceChallengeInfo a_message)
        {
            PongClientServiceChallengeState servChal = _states[(int)EPongGameState.ServiceChallenge] as PongClientServiceChallengeState;
            servChal.SetTargetBounces(a_message.bounceCount);
        }
        #endregion
    }
}