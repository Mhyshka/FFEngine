using UnityEngine;
using System.Collections;
using System;

using FF.UI;

namespace FF.Networking
{
    internal class FFConfirmSwapRequest : FFRequestMessage
    {
        protected static FFConfirmSwapRequest s_CurrentRequest = null;
        internal enum EErrorCode
        {
            PlayerDisconnected,
            PlayerRefused,
            PlayerIsBusy,
            TargetNotFound
        }

        #region Properties
        protected string _fromUsername;
        protected int _popupId;
        internal override EMessageType Type
        {
            get
            {
                return EMessageType.SwapConfirmRequest;
            }
        }

        internal override bool HandleByMock
        {
            get
            {
                return true;
            }
        }

        protected override int DisconnectErrorCode
        {
            get
            {
                return (int)EErrorCode.PlayerDisconnected;
            }
        }
        #endregion

        #region Constructors
        public FFConfirmSwapRequest()
        {
        }

        internal FFConfirmSwapRequest(string a_fromUsername)
        {
            _fromUsername = a_fromUsername;
        }
        #endregion

        protected override float TimeoutDuration
        {
            get
            {
                return float.MaxValue;
            }
        }

        internal override void Read()
        {
            FFResponseMessage answer = null;
            int errorCode = -1;
            bool pending = false;

            if (!_client.IsConnected)
            {
                errorCode = (int)EErrorCode.PlayerDisconnected;
                answer = new FFRequestFail(errorCode);
            }
            else if (FFEngine.Network.Player.IsBusy)
            {
                errorCode = (int)EErrorCode.PlayerIsBusy;
                answer = new FFRequestFail(errorCode);
            }
            else
            {
                FFEngine.Network.Player.busyCount++;
                _client.onConnectionLost += OnConnectionLostWaiting;
                _popupId = FFYesNoPopup.RequestDisplay(_fromUsername + " would like to swap position with you.", "Accept", "Decline", OnYesPressed, OnNoPressed);
                onCancel += OnCancelReceived;
                pending = true;
            }

            if (!pending)
            {
                answer.requestId = requestId;
                _client.QueueMessage(answer);
            }
        }

        //Exectued on receiver ( Popup confirm callbacks )
        #region Receiver
        protected void OnNoPressed()
        {
            OnReceiverComplete();

            int errorCode = (int)EErrorCode.PlayerRefused;
            
            FFRequestFail answer = new FFRequestFail(errorCode);
            answer.requestId = requestId;
            _client.QueueMessage(answer);
        }

        protected void OnYesPressed()
        {
            OnReceiverComplete();

            FFRequestSuccess answer = new FFRequestSuccess();
            answer.requestId = requestId;
            _client.QueueMessage(answer);
        }

        protected void OnCancelReceived()
        {
            OnReceiverComplete();
        }

        protected void OnConnectionLostWaiting(FFTcpClient a_client)
        {
            FFResponseMessage answer = new FFRequestCancel();
            answer.requestId = requestId;
            _client.QueueReadMessage(answer);
        }

        protected void OnReceiverComplete()
        {
            _client.onConnectionLost -= OnConnectionLostWaiting;
            FFEngine.UI.DismissPopup(_popupId);

            FFEngine.Network.Player.busyCount--;
        }
        #endregion

        #region Serialization
        public override void LoadFromData(FFByteReader stream)
        {
            base.LoadFromData(stream);
            _fromUsername = stream.TryReadString();
        }

        public override void SerializeData(FFByteWriter stream)
        {
            base.SerializeData(stream);
            stream.Write(_fromUsername);
        }
        #endregion
    }
}
