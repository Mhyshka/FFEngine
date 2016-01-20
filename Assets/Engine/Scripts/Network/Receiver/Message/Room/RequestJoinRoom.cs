using UnityEngine;
using System.Collections;
using System;

using FF.Multiplayer;
using FF.Network.Message;

namespace FF.Network.Receiver
{
    internal class RequestJoinRoom : AReceiver<Message.RequestJoinRoom>
    {
        #region Properties
        #endregion

        protected override void HandleMessage()
        {
            AResponse answer = null;
            int errorCode = -1;
            Room room = Engine.Game.CurrentRoom;

            if (!_client.IsConnected)
            {
                errorCode = (int)ESimpleRequestErrorCode.Disconnected;
                answer = new Message.ResponseFail(errorCode);
            }
            else if (Engine.Network.Server == null)
            {
                errorCode = (int)Message.RequestJoinRoom.EErrorCode.ServerOnly;
                answer = new Message.ResponseFail(errorCode);
            }
            else if (room.IsBanned(_client.NetworkID))
            {
                errorCode = (int)Message.RequestJoinRoom.EErrorCode.Banned;
                answer = new Message.ResponseFail(errorCode);
            }
            else if (room.IsFull)
            {
                errorCode = (int)Message.RequestJoinRoom.EErrorCode.RoomIsFull;
                answer = new Message.ResponseFail(errorCode);
            }
            else
            {
                Slot nextSlot = room.NextAvailableSlot();
                _message.player.IpEndPoint = _client.Remote;
                Engine.Game.CurrentRoom.SetPlayer(nextSlot.team.teamIndex, nextSlot.slotIndex, _message.player);
                answer = new Message.ResponseSuccess();
            }

            answer.requestId = _message.requestId;
            _client.QueueMessage(answer);
        }
    }
}