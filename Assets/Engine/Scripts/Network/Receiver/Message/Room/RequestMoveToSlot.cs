using UnityEngine;
using System.Collections;
using System;

using FF.Multiplayer;
using FF.Network.Message;

namespace FF.Network.Receiver
{
    internal class RequestMoveToSlot : AReceiver<Message.RequestMoveToSlot>
    {
        #region Properties
        #endregion

        protected override void HandleMessage()
        {
            AResponse answer = null;
            FFNetworkPlayer source = Engine.Game.CurrentRoom.GetPlayerForId(_client.NetworkID);

            int errorCode = -1;

            if (!_client.IsConnected)
            {
                errorCode = (int)Message.RequestMoveToSlot.EErrorCode.PlayerDisconnected;
                answer = new Message.ResponseFail(errorCode);
            }
            else if (source == null)
            {
                errorCode = (int)Message.RequestMoveToSlot.EErrorCode.PlayerNotfound;
                answer = new Message.ResponseFail(errorCode);
            }
            else if (Engine.Game.CurrentRoom.teams[_message.slotRef.teamIndex].Slots[_message.slotRef.slotIndex].netPlayer != null)
            {
                errorCode = (int)Message.RequestMoveToSlot.EErrorCode.SlotIsUsed;
                answer = new Message.ResponseFail(errorCode);
            }
            else
            {
                Engine.Game.CurrentRoom.MovePlayer(source.SlotRef, _message.slotRef);
                answer = new Message.ResponseSuccess();
            }

            answer.requestId = _message.requestId;
            _client.QueueMessage(answer);
        }
    }
}