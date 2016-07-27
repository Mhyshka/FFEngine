using UnityEngine;
using System.Collections;
using System;

using FF.Multiplayer;
using FF.Network.Message;

namespace FF.Network.Receiver
{
    internal class MoveToSlotReceiver : BaseMessageReceiver
    {
        #region Properties
        #endregion

        protected override void HandleMessage()
        {
            if (_message.HeaderType == EHeaderType.Request)
            {
                ReadRequest request = _message as ReadRequest;
                if (_message.Data.Type == EDataType.SlotRef)
                {
                    MessageSlotRefData data = _message.Data as MessageSlotRefData;

                    SentResponse answer = null;
                    FFNetworkPlayer source = Engine.Network.CurrentRoom.PlayerForId(_client.NetworkID);

                    ERequestErrorCode errorCode = ERequestErrorCode.Canceled;
                    int detailErrorCode = -1;

                    if (!_client.IsConnected)
                    {
                        errorCode = ERequestErrorCode.Failed;
                        detailErrorCode = (int)EErrorCodeMoveToSlot.PlayerDisconnected;
                        answer = new SentResponse(new MessageIntegerData(detailErrorCode),
                                                    request.RequestId,
                                                    errorCode);
                    }
                    else if (source == null)
                    {
                        errorCode = ERequestErrorCode.Failed;
                        detailErrorCode = (int)EErrorCodeMoveToSlot.PlayerNotfound;
                        answer = new SentResponse(new MessageIntegerData(detailErrorCode),
                                                    request.RequestId,
                                                    errorCode);
                    }
                    else if (Engine.Network.CurrentRoom.GetPlayerForSlot(data.SlotRef) != null)
                    {
                        errorCode = ERequestErrorCode.Failed;
                        detailErrorCode = (int)EErrorCodeMoveToSlot.SlotIsUsed;
                        answer = new SentResponse(new MessageIntegerData(detailErrorCode),
                                                    request.RequestId,
                                                    errorCode);
                    }
                    else
                    {
                        Engine.Network.CurrentRoom.MovePlayer(source.SlotRef, data.SlotRef);
                        errorCode = ERequestErrorCode.Success;
                        answer = new SentResponse(new MessageEmptyData(),
                                                    request.RequestId,
                                                    errorCode);
                    }

                    _client.QueueResponse(answer);
                }
            }
        }
    }
}