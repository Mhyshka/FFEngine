using UnityEngine;

using FF.Multiplayer;

using FF.Network.Message;
using FF.Network.Receiver;

namespace FF.Pong
{
    internal class PongServerServiceChallengeState : PongServiceChallengeState
    {
        #region Inspector Properties
        public int minBounces = 20;
        public int maxBounces = 29;
        #endregion

        #region Properties
        protected PlayerDictionary<bool> _playersReady = null;

        protected GenericMessageReceiver<MessageLoadingReady, MessageHeader> _readyReceiver = null;
        #endregion

        #region State Methods
        internal override void Enter()
        {
            base.Enter();
            if (_readyReceiver == null)
                _readyReceiver = new GenericMessageReceiver<MessageLoadingReady, MessageHeader>(OnReadyReceived);

            _playersReady = new PlayerDictionary<bool>();
            _bounceCount = Random.Range(minBounces, maxBounces);

            MessageServiceChallengeInfo serviceInfo = new MessageServiceChallengeInfo(_bounceCount);
            Engine.Network.Server.BroadcastMessage(serviceInfo);
        }

        internal override int Manage()
        {
            bool isEveryoneReady = true;
            foreach (bool each in _playersReady.Values)
            {
                isEveryoneReady = isEveryoneReady && each;
            }

            if (isEveryoneReady)
            {
                MessageLoadingComplete completeMessage = new MessageLoadingComplete();
                Engine.Network.Server.BroadcastMessage(completeMessage);
                RequestState(outState.ID);
            }

            return base.Manage();
        }

        internal override void Exit()
        {
            base.Exit();
        }
        #endregion

        #region Event Management
        protected override void RegisterForEvent()
        {
            base.RegisterForEvent();
            Engine.Receiver.RegisterReceiver(EDataType.M_LoadingReady, _readyReceiver);
        }

        protected override void UnregisterForEvent()
        {
            base.UnregisterForEvent();
            Engine.Receiver.UnregisterReceiver(EDataType.M_LoadingReady, _readyReceiver);
        }

        protected void OnReadyReceived(MessageHeader a_header, MessageLoadingReady a_readyMessage)
        {
            _playersReady[a_header.Client.NetworkID] = true;
        }
        #endregion

        protected override void OnAnimationComplete()
        {
            base.OnAnimationComplete();
            _playersReady[Engine.Network.NetworkID] = true;
        }
    }
}