using UnityEngine;
using System.Collections;

using FF.Network.Message;
using FF.Network.Receiver;

namespace FF.Pong
{
    internal class PongClientGameMode : PongGameMode
    {
        #region Properties
        protected GenericMessageReceiver _challengeInfosReceiver = null;
        #endregion

        #region State Methods
        protected override void Enter()
        {
            _challengeInfosReceiver = new GenericMessageReceiver(OnChallengeInfosReceived);
            base.Enter();
        }
        #endregion

        #region Event Management
        protected override void RegisterForEvent()
        {
            base.RegisterForEvent();
            Engine.Receiver.RegisterReceiver(EMessageChannel.ChallengeInfos.ToString(), _challengeInfosReceiver);
        }

        protected override void UnregisterForEvent()
        {
            base.UnregisterForEvent();
            Engine.Receiver.UnregisterReceiver(EMessageChannel.ChallengeInfos.ToString(), _challengeInfosReceiver);
        }

        protected void OnChallengeInfosReceived(ReadMessage a_message)
        {
            MessageIntegerData data = a_message.Data as MessageIntegerData;//Ball bounce count
            PongClientServiceChallengeState servChal = _states[(int)EPongGameState.ServiceChallenge] as PongClientServiceChallengeState;
            servChal.SetTargetBounces(data.Data);
        }
        #endregion
    }
}