using UnityEngine;
using System.Collections;
using System;

using FF.Multiplayer;
using FF.Network.Message;

namespace FF.Network.Receiver
{
    internal class JoinRoomReceiver : BaseMessageReceiver
    {
        #region Properties
        #endregion

        protected override void HandleMessage()
        {
            if (_message.HeaderType == EHeaderType.Request)
            {
                ReadRequest request = _message as ReadRequest;
                if (_message.Data.Type == EDataType.Player)
                {
                    MessagePlayerData data = _message.Data as MessagePlayerData;

                    SentResponse answer = null;
                    ERequestErrorCode errorCode = ERequestErrorCode.Canceled;
                    int detailErrorCode = -1;
                    Room room = Engine.Game.CurrentRoom;

                    if (!_client.IsConnected)
                    {
                        errorCode = ERequestErrorCode.Failed;
                        detailErrorCode = (int)EErrorCodeJoinRoom.PlayerDisconnected;
                        answer = new SentResponse(new MessageIntegerData(detailErrorCode),
                                                    request.RequestId,
                                                    errorCode);
                    }
                    else if (Engine.Network.Server == null)
                    {
                        errorCode = ERequestErrorCode.Failed;
                        detailErrorCode = (int)EErrorCodeJoinRoom.ToServerOnly;
                        answer = new SentResponse(new MessageIntegerData(detailErrorCode),
                                                    request.RequestId,
                                                    errorCode);
                    }
                    else if (room.IsBanned(_client.NetworkID))
                    {
                        errorCode = ERequestErrorCode.Failed;
                        detailErrorCode = (int)EErrorCodeJoinRoom.PlayerIsBannedFromRoom;
                        answer = new SentResponse(new MessageIntegerData(detailErrorCode),
                                                    request.RequestId,
                                                    errorCode);
                    }
                    else if (room.IsFull)
                    {
                        errorCode = ERequestErrorCode.Failed;
                        detailErrorCode = (int)EErrorCodeJoinRoom.RoomIsFull;
                        answer = new SentResponse(new MessageIntegerData(detailErrorCode),
                                                    request.RequestId,
                                                    errorCode);
                    }
                    else
                    {
                        Slot nextSlot = room.NextAvailableSlot();
                        data.Player.IpEndPoint = _client.Remote;
                        Engine.Game.CurrentRoom.SetPlayer(nextSlot.team.teamIndex, nextSlot.slotIndex, data.Player);
                        answer = new SentResponse(new MessageEmptyData(),
                                                    request.RequestId,
                                                    ERequestErrorCode.Success);
                    }

                    _client.QueueResponse(answer);
                }
            }
        }
    }
}