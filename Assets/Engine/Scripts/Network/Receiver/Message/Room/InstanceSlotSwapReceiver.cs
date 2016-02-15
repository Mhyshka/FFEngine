using UnityEngine;
using System.Collections;
using System;

using FF.Multiplayer;
using FF.Network.Message;

namespace FF.Network.Receiver
{
    internal class InstanceSlotSwapReceiver : BaseMessageReceiver
    {
        #region Properties
        protected FFTcpClient _targetClient = null;

        protected ReadRequest _request;
        protected MessageSlotRefData _slotRefData;

        protected SentRequest _confirmRequest;
        #endregion

        public InstanceSlotSwapReceiver()
        {
        }

        protected override void HandleMessage()
        {
            if (_message.HeaderType == EHeaderType.Request)
            {
                _request = _message as ReadRequest;
                if (_message.Data.Type == EDataType.SlotRef)
                {
                    _slotRefData = _message.Data as MessageSlotRefData;
                    bool pending = false;

                    SentResponse answer = null;
                    ERequestErrorCode errorCode = ERequestErrorCode.Canceled;
                    int detailErrorCode = -1;

                    FFNetworkPlayer source = Engine.Game.CurrentRoom.GetPlayerForId(_client.NetworkID);
                    Slot ffSlot = Engine.Game.CurrentRoom.GetSlotForRef(_slotRefData.SlotRef);
                    FFNetworkPlayer target = ffSlot.netPlayer;
                    _targetClient = null;
                    if (target != null)
                        _targetClient = Engine.Network.Server.ClientForEP(target.IpEndPoint);

                    if (!_client.IsConnected)
                    {
                        errorCode = ERequestErrorCode.Failed;
                        detailErrorCode = (int)EErrorCodeSwapSlot.PlayerDisconnected;
                        answer = new SentResponse(new MessageIntegerData(detailErrorCode),
                                                    _request.RequestId,
                                                    errorCode);
                    }
                    else if (source == null)
                    {
                        errorCode = ERequestErrorCode.Failed;
                        detailErrorCode = (int)EErrorCodeSwapSlot.PlayerNotfound;
                        answer = new SentResponse(new MessageIntegerData(detailErrorCode),
                                                    _request.RequestId,
                                                    errorCode);
                    }
                    else if (target == null)
                    {
                        //No player in the targeted slot -> Move the requesting player to the slot.
                        Engine.Game.CurrentRoom.MovePlayer(source.SlotRef, _slotRefData.SlotRef);
                        errorCode = ERequestErrorCode.Success;
                        answer = new SentResponse(new MessageEmptyData(),
                                                    _request.RequestId,
                                                    errorCode);
                    }
                    else if (_targetClient == null)
                    {
                        errorCode = ERequestErrorCode.Failed;
                        detailErrorCode = (int)EErrorCodeSwapSlot.TargetDisconnected;
                        answer = new SentResponse(new MessageIntegerData(detailErrorCode),
                                                    _request.RequestId,
                                                    errorCode);
                    }
                    else
                    {
                        _request.onCanceled += OnCancelReceived;
                        _client.onConnectionLost += ServerOnConnectionLost;
                        _targetClient.onConnectionLost += ServerOnConnectionLost;
                        _confirmRequest = new SentRequest(new MessageStringData(source.player.username),
                                                                    EMessageChannel.SwapConfirm.ToString(),
                                                                    Engine.Network.NextRequestId,
                                                                    float.MaxValue,
                                                                    true,
                                                                    true);
                        _confirmRequest.onSucces += OnSuccess;
                        _confirmRequest.onFail += OnFail;
                        _targetClient.QueueRequest(_confirmRequest);
                        pending = true;
                    }

                    if (!pending)
                    {
                        _client.QueueMessage(answer);
                    }
                }
            }
        }

        protected void ServerOnConnectionLost(FFTcpClient a_client)
        {
            _client.onConnectionLost -= ServerOnConnectionLost;
            _targetClient.onConnectionLost -= ServerOnConnectionLost;
            
            if (a_client == _client)//Cancel for target
            {
                if (!_confirmRequest.IsComplete)
                    _confirmRequest.Cancel(true);
            }
            else//Fail for sender
            {
                SentResponse message = new SentResponse(new MessageEmptyData(),
                                                        _request.RequestId,
                                                        ERequestErrorCode.Failed,
                                                        true);
                _client.QueueResponse(message);
            }
        }

        #region Callbacks
        protected void OnSuccess(ReadResponse a_response)
        {
            FFNetworkPlayer source = Engine.Game.CurrentRoom.GetPlayerForId(_client.NetworkID);
            Engine.Game.CurrentRoom.SwapPlayers(source.SlotRef, _slotRefData.SlotRef);

            SentResponse response = new SentResponse(a_response.Data,
                                                     _request.RequestId,
                                                     a_response.ErrorCode,
                                                     true);
            _client.QueueResponse(response);
        }

        protected void OnFail(ERequestErrorCode a_errorCode, ReadResponse a_response)
        {
            SentResponse response = new SentResponse(a_response != null ? a_response.Data : new MessageEmptyData(),
                                                    _request.RequestId,
                                                    a_errorCode,
                                                    true);
            _client.QueueResponse(response);
        }

        protected void OnCancelReceived()
        {
            if(!_confirmRequest.IsComplete)
                _confirmRequest.Cancel(true);
        }
        #endregion
    }
}