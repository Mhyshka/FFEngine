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
        #endregion

        #region Properties
        protected GenericMessageReceiver _targetRatioReceiver = null;
        protected GenericMessageReceiver _trySmashReceiver = null;
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

        protected void OnTargetRatioReceived(ReadMessage a_message)
        {
            MessageFloatData data = a_message.Data as MessageFloatData;
            motor.TargetRatio = data.Data;
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