using UnityEngine;
using System.Collections;
using FF.Network;

using FF.Handler;
using FF.Network.Message;
using System;

namespace FF.Pong
{
    internal class ServerBall : ABall
    {
        #region Properties

        #endregion

        internal override void OnTriggerEnter(Collider a_other)
        {
            IBallContact contact = a_other.GetComponent<IBallContact>();
            if (contact != null)
            {
                color.RandomizeNewColor();
                hitFx.Play(color.CurrentColor,
                            transform.position,
                            a_other.transform.forward);
                lightManager.OnHit();
                
                RacketCollider racket = contact as RacketCollider;

                //FFLog.LogError("Contact GO : " + contact.ToString());
                if (racket == null ||
                    _lastLocalHitInfo.playerId != racket.motor.PlayerId)
                {
                    Vector3 velocity = contact.BounceOff(transform.position, ballRigidbody.velocity);
                    SetVelocity(velocity);

                    if (racket != null)
                    {
                        _lastLocalHitInfo.playerId = racket.motor.PlayerId;
                        _lastLocalHitInfo.position = transform.position;
                        _lastLocalHitInfo.velocity = velocity;
                        _lastLocalHitInfo.timestamp = DateTime.Now.Ticks;
                    }

                    //Is Local Racket
                    if (racket != null &&
                        racket.motor.PlayerId == Engine.Network.NetPlayer.ID)
                    {
                        BroadcastNetworkMovementMessage();
                    }
                }

                contact.OnCollision(this);
            }
        }

        internal override void ForceNetworkMovementSync()
        {
            BroadcastNetworkMovementMessage();
        }

        protected void BroadcastNetworkMovementMessage()
        {
            MessageBallMovementData collisionData = new MessageBallMovementData(Engine.Network.NetPlayer.ID,
                                                                                transform.position,
                                                                                ballRigidbody.velocity);
            SentBroadcastMessage message = new SentBroadcastMessage(Engine.Network.CurrentRoom.GetPlayersIds(),
                                                                    collisionData,
                                                                    EMessageChannel.BallMovement.ToString(),
                                                                    true);
            message.Broadcast();
        }

        internal void OnGoal(ESide a_side)
        {
            if (onGoal != null)
                onGoal(a_side);

            MessageIntegerData data = new MessageIntegerData((int)a_side);
            SentBroadcastMessage message = new SentBroadcastMessage(Engine.Network.CurrentRoom.GetPlayersIds(),
                                                                    data,
                                                                    EMessageChannel.GoalHit.ToString(),
                                                                    true);
            message.Broadcast();
        }

        internal override void PostSmash()
        {
            base.PostSmash();

            /*_smashCount++;
            SetVelocity(ballRigidbody.velocity * (1 + _smashCount * smashSpeedMultiplier));

            MessageIntegerData data = new MessageIntegerData(_smashCount);
            SentBroadcastMessage message = new SentBroadcastMessage(Engine.Network.CurrentRoom.GetPlayersIds(),
                                                    data,
                                                    EMessageChannel.Smash.ToString(),
                                                    true);
            message.Broadcast();*/
        }

        /*protected override void OnRacketHitReceived(ReadMessage a_message)
        {
            throw new NotImplementedException();
        }*/
    }
}