using UnityEngine;
using System.Collections;

using FF.Logic;
using System;

using FF.Handler;
using FF.Network.Receiver;
using FF.Network.Message;

namespace FF.Pong
{
    internal class PongServerServiceState : PongServiceState
    {
        #region Inspector Properties
        #endregion

        #region properties
        #endregion

        #region State Methods
        internal override void Enter()
        {
            base.Enter();
            SentBroadcastMessage nextMessage = new SentBroadcastMessage(Engine.Network.CurrentRoom.GetPlayersIds(),
                                                                        new MessageEmptyData(),
                                                                        EMessageChannel.Next.ToString());
            nextMessage.Broadcast();
        }
        #endregion


        protected override void OnServicePlayerSmash()
        {
            base.OnServicePlayerSmash();
            BroadcastServiceToPlayers(DateTime.Now.Ticks);
        }

        protected override void OnServiceLaunchReceived(ReadMessage a_message)
        {
            base.OnServiceLaunchReceived(a_message);
            BroadcastServiceToPlayers(a_message.Timestamp);
        }

        protected void BroadcastServiceToPlayers(long a_timestamp)
        {
            //Broadcast to players.
            MessageBallMovementData movementData = new MessageBallMovementData(_pongGm.serverPlayerId,
                                                                                _pongGm.ball.transform.position,
                                                                                _pongGm.ball.ballRigidbody.velocity);
            SentBroadcastMessage message = new SentBroadcastMessage(Engine.Network.CurrentRoom.GetPlayersIds(),
                                                                    movementData,
                                                                    EMessageChannel.ServiceLaunch.ToString(),
                                                                    false,
                                                                    false,
                                                                    5f,
                                                                    a_timestamp);
            message.Broadcast();
        }
    }
}