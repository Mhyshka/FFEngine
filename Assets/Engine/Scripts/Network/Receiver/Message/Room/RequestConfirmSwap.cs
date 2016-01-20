using UnityEngine;
using System.Collections;
using System;
using FF.UI;

using FF.Network.Message;

namespace FF.Network.Receiver
{
    internal class RequestConfirmSwap : APendingReceiver<InstanceConfirmSwap>
    {

    }

    internal class InstanceConfirmSwap : AReceiver<Message.RequestConfirmSwap>
    {
        #region Properties
        protected int _popupId;
        #endregion

        protected override void HandleMessage()
        {
            AResponse answer = null;
            int errorCode = -1;
            bool pending = false;

            if (!_client.IsConnected)
            {
                errorCode = (int)ESimpleRequestErrorCode.Disconnected;
                answer = new Message.ResponseFail(errorCode);
            }
            else if (Engine.Game.NetPlayer.IsBusy)
            {
                errorCode = (int)Message.RequestConfirmSwap.EErrorCode.PlayerIsBusy;
                answer = new Message.ResponseFail(errorCode);
            }
            else
            {
                Engine.Game.NetPlayer.busyCount++;
                _client.onConnectionLost += OnConnectionLostWaiting;
                _popupId = FFYesNoPopup.RequestDisplay(_message.fromUsername + " would like to swap position with you.", "Accept", "Decline", OnYesPressed, OnNoPressed);
                _message.onCancel += OnCancelReceived;
                pending = true;
            }

            if (!pending)
            {
                answer.requestId = _message.requestId;
                _client.QueueMessage(answer);
            }
        }

        protected void OnConnectionLostWaiting(FFTcpClient a_client)
        {
            AResponse answer = new Message.ResponseCancel();
            answer.requestId = _message.requestId;
            _client.QueueReadMessage(answer);
        }

        #region Receiver
        protected void OnNoPressed()
        {
            OnReceiverComplete();

            int errorCode = (int)Message.RequestConfirmSwap.EErrorCode.PlayerRefused;

            Message.ResponseFail answer = new Message.ResponseFail(errorCode);
            answer.requestId = _message.requestId;
            _client.QueueMessage(answer);
        }

        protected void OnYesPressed()
        {
            OnReceiverComplete();

            Message.ResponseSuccess answer = new Message.ResponseSuccess();
            answer.requestId = _message.requestId;
            _client.QueueMessage(answer);
        }

        protected void OnCancelReceived()
        {
            OnReceiverComplete();
        }

        protected void OnReceiverComplete()
        {
            _client.onConnectionLost -= OnConnectionLostWaiting;
            Engine.UI.DismissPopup(_popupId);

            Engine.Game.NetPlayer.busyCount--;
        }
        #endregion
    }
}