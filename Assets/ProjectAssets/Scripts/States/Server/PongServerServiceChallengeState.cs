using UnityEngine;

using FF.Multiplayer;

using FF.Handler;
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

        protected GenericMessageReceiver _readyReceiver = null;
        #endregion

        #region State Methods
        internal override void Enter()
        {
            base.Enter();
            if (_readyReceiver == null)
                _readyReceiver = new GenericMessageReceiver(OnReadyReceived);

            _playersReady = new PlayerDictionary<bool>();
            _bounceCount = 1;//Random.Range(minBounces, maxBounces);

            MessageIntegerData data = new MessageIntegerData(_bounceCount);
            SentBroadcastMessage message = new SentBroadcastMessage(Engine.Network.CurrentRoom.GetPlayersIds(),
                                                                    data,
                                                                    EMessageChannel.ChallengeInfos.ToString());
            message.Broadcast();
        }
        #endregion

        #region Event Management
        protected override void RegisterForEvent()
        {
            base.RegisterForEvent();
            Engine.Receiver.RegisterReceiver(EMessageChannel.Ready.ToString(), _readyReceiver);
        }

        protected override void UnregisterForEvent()
        {
            base.UnregisterForEvent();
            Engine.Receiver.UnregisterReceiver(EMessageChannel.Ready.ToString(), _readyReceiver);
        }

        protected void OnReadyReceived(ReadMessage a_message)
        {
            _playersReady[a_message.Client.NetworkID] = true;

            bool isEveryoneReady = true;
            foreach (bool each in _playersReady.Values)
            {
                isEveryoneReady = isEveryoneReady && each;
            }

            if (isEveryoneReady)
            {
                RequestState(outState.ID);
            }
        }
        #endregion

        protected override void OnAnimationComplete()
        {
            base.OnAnimationComplete();
            _playersReady[Engine.Network.NetworkId] = true;

            bool isEveryoneReady = true;
            foreach (bool each in _playersReady.Values)
            {
                isEveryoneReady = isEveryoneReady && each;
            }

            if (isEveryoneReady)
            {
                RequestState(outState.ID);
            }
        }
    }
}