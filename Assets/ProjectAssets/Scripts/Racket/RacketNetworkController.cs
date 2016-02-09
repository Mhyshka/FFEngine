using UnityEngine;
using System.Collections;
using System;

using FF.Network.Receiver;
using FF.Network.Message;

namespace FF.Pong
{
    internal class RacketNetworkController : ARacketComponent
    {
        #region Inspector Properties
        public float lerpSmoothFactor = 1f;
        #endregion

        #region Properties
        protected GenericMessageReceiver _targetRatioReceiver = null;
        protected GenericMessageReceiver _trySmashReceiver = null;

        protected float _displayRatio = 0f;
        #endregion

        #region Init & Destroy
        internal override void Activate()
        {
            if(_targetRatioReceiver == null)
                _targetRatioReceiver = new GenericMessageReceiver(OnTargetRatioReceived);
            if(_trySmashReceiver == null)
                _trySmashReceiver = new GenericMessageReceiver(OnTrySmashReceived);

            //TODO smash
            //Engine.Receiver.RegisterReceiver(EDataType.M_TrySmash, _trySmashReceiver);
            Engine.Receiver.RegisterReceiver(EMessageChannel.RacketPosition.ToString() + motor.channelSuffix, _targetRatioReceiver);
        }

        internal override void TearDown()
        {
            //TODO smash
            //Engine.Receiver.UnregisterReceiver(EDataType.M_TrySmash, _trySmashReceiver);
            Engine.Receiver.UnregisterReceiver(EMessageChannel.RacketPosition.ToString() + motor.channelSuffix, _targetRatioReceiver);
        }

        internal override Vector3 UpdatePosition(float a_minPositionX, float a_maxPositionX, float a_currentRatio)
        {
            Vector3 position = transform.localPosition;
            _displayRatio = Mathf.Lerp(_displayRatio, a_currentRatio, lerpSmoothFactor * Time.deltaTime);
            position.x = Mathf.Lerp(a_minPositionX, a_maxPositionX, _displayRatio);
            return position;
        }

        internal override void ForceRatio(float a_currentRatio)
        {
            base.ForceRatio(a_currentRatio);
            _displayRatio = a_currentRatio;
        }

        protected void OnTargetRatioReceived(ReadMessage a_readMessage)
        {
            MessageRacketMovementData data = a_readMessage.Data as MessageRacketMovementData;
            

            float deltatime = (float)a_readMessage.Client.TimeOffset(a_readMessage.Timestamp).TotalSeconds;
            //Debug.LogError("Racket deltatime : " + deltatime);
            float curRatio = Mathf.MoveTowards(data.CurrentRatio,
                                                data.TargetRatio,
                                                deltatime * motor.moveTowardSpeed);
            motor.CurrentRatio = curRatio;
            motor.TargetRatio = data.TargetRatio;

            if (Engine.Network.IsServer)
            {
                SentMessage message = new SentMessage(data,
                                                    EMessageChannel.RacketPosition.ToString() + motor.channelSuffix,
                                                    false,
                                                    false);
                message.Timestamp = a_readMessage.Client.ConvertToLocalTime(a_readMessage.Timestamp);
                Engine.Network.Server.BroadcastMessage(message);
            }
        }

        protected void OnTrySmashReceived(ReadMessage a_message)
        {
            /*if (a_message.racketId == motor.clientId)
            {*/
                motor.TrySmash();
            //}
        }
        #endregion
    }
}