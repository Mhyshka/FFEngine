using UnityEngine;
using System.Collections;
using System;
using FF.UI;

using FF.Network.Message;

namespace FF.Network.Receiver
{
    internal class InstanceConfirmSwapReceiver : BaseMessageReceiver
    {
        #region Properties
        protected int _popupId;

        protected ReadRequest _request;
        protected MessageStringData _data;
        #endregion

        public InstanceConfirmSwapReceiver()
        {
        }

        protected override void HandleMessage()
        {
            if (_message.HeaderType == EHeaderType.Request)
            {
                _request = _message as ReadRequest;
                if (_message.Data.Type == EDataType.String)
                {
                    _data = _message.Data as MessageStringData;
                    SentResponse answer = null;

                    int detailErrorCode = -1;
                    ERequestErrorCode errorCode = ERequestErrorCode.Canceled;

                    bool pending = false;

                    if (!_client.IsConnected)
                    {
                        errorCode = ERequestErrorCode.Failed;
                        detailErrorCode = (int)EErrorCodeSwapSlot.PlayerDisconnected;
                        answer = new SentResponse(new MessageIntegerData(detailErrorCode),
                                                   _request.RequestId,
                                                   errorCode);
                    }
                    else if (Engine.Game.NetPlayer.IsBusy)
                    {
                        errorCode = ERequestErrorCode.Failed;
                        detailErrorCode = (int)EErrorCodeSwapSlot.PlayerIsBusy;
                        answer = new SentResponse(new MessageIntegerData(detailErrorCode),
                                                   _request.RequestId,
                                                   errorCode);
                    }
                    else
                    {
                        Engine.Game.NetPlayer.busyCount++;
                        _client.onConnectionLost += OnConnectionLostWaiting;
                        _popupId = FFYesNoPopup.RequestDisplay(_data.StringData + " would like to swap position with you.",
                                                                "Accept",
                                                                "Decline",
                                                                OnYesPressed,
                                                                OnNoPressed);
                        _request.onCanceled += OnCancelReceived;
                        pending = true;
                    }

                    if (!pending)
                    {
                        _client.QueueResponse(answer);
                    }
                }
            }
        }

        protected void OnConnectionLostWaiting(FFTcpClient a_client)
        {
            _request.FailWithoutResponse(ERequestErrorCode.Canceled);
            OnReceiverComplete();
        }

        #region Receiver
        protected void OnNoPressed()
        {
            OnReceiverComplete();

            ERequestErrorCode errorCode = ERequestErrorCode.Failed;
            int detailErrorCode = (int)EErrorCodeSwapSlot.PlayerRefused;
            SentResponse answer = new SentResponse(new MessageIntegerData(detailErrorCode),
                                                   _request.RequestId,
                                                   errorCode);
            _client.QueueResponse(answer);
        }

        protected void OnYesPressed()
        {
            OnReceiverComplete();

            ERequestErrorCode errorCode = ERequestErrorCode.Success;
            SentResponse answer = new SentResponse(new MessageEmptyData(),
                                                   _request.RequestId,
                                                   errorCode);
            _client.QueueResponse(answer);
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