using UnityEngine;
using System.Collections;

using FF.Network.Receiver;
using FF.Network.Message;
using System;

namespace FF.Pong
{
    internal class ClientBall : ABall
    {
        #region Inspector Properties
        #endregion

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
                        _lastLocalHitInfo.timestamp = System.DateTime.Now.Ticks;
                    }

                    /*if (racket != null)
                        FFLog.LogError("Last ID : " + _lastRacketId + " / current : " + racket.motor.PlayerId);*/

                    //Is Local Racket
                    if (racket != null &&
                        racket.motor.PlayerId == Engine.Network.NetPlayer.ID)
                    {
                        //Debug.LogError("Sending collision" + velocity.ToString());

                        SendNetworkMovementMessage();
                    }
                }
            }
        }


        protected void SendNetworkMovementMessage()
        {
            MessageBallMovementData collisionData = new MessageBallMovementData(Engine.Network.NetPlayer.ID,
                                                                                transform.position,
                                                                                ballRigidbody.velocity);
            SentMessage message = new SentMessage(collisionData,
                                                    EMessageChannel.BallMovement.ToString(),
                                                    true);
            Engine.Network.MainClient.QueueMessage(message);
        }

        #region Network
        protected GenericMessageReceiver _goalHitReceiver;

        internal override void NetworkInit()
        {
            base.NetworkInit();
            if(_goalHitReceiver == null)
                _goalHitReceiver = new GenericMessageReceiver(OnGoalHitReceived);
            
            Engine.Receiver.RegisterReceiver(EMessageChannel.GoalHit.ToString(), _goalHitReceiver);
        }

        internal override void ForceNetworkMovementSync()
        {
            SendNetworkMovementMessage();
        }

        internal override void NetworkTearDown()
        {
            base.NetworkTearDown();
            Engine.Receiver.UnregisterReceiver(EMessageChannel.GoalHit.ToString(), _goalHitReceiver);
        }

        protected void OnGoalHitReceived(ReadMessage a_message)
        {
            MessageIntegerData data = a_message.Data as MessageIntegerData;//ESide
            ESide side = (ESide)data.Data;
            if(onGoal != null)
                onGoal(side);
        }
        #endregion
    }
}