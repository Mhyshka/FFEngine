using UnityEngine;
using System.Collections;
using System;

using FF.Multiplayer;
using FF.Network.Message;

namespace FF.Network.Receiver
{
    internal class RequestSlotSwap : APendingReceiver<InstanceSlotSwap>
    {

    }

    internal class InstanceSlotSwap : AReceiver<Message.RequestSlotSwap>
    {
        #region Properties
        protected FFTcpClient _targetClient = null;
        protected Handler.ConfirmSwap _confirmHandler;
        #endregion

        protected override void HandleMessage()
        {
            bool pending = false;
            AResponse answer = null;
            int errorCode = -1;
            FFNetworkPlayer source = Engine.Game.CurrentRoom.GetPlayerForId(_client.NetworkID);
            Slot ffSlot = Engine.Game.CurrentRoom.GetSlotForRef(_message.targetSlot);
            FFNetworkPlayer target = ffSlot.netPlayer;
            _targetClient = null;
            if (target != null)
                _targetClient = Engine.Network.Server.ClientForEP(target.IpEndPoint);

            if (!_client.IsConnected)
            {
                errorCode = (int)ESimpleRequestErrorCode.Disconnected;
                answer = new Message.ResponseFail(errorCode);
            }
            else if (source == null)
            {
                errorCode = (int)Message.RequestSlotSwap.EErrorCode.PlayerNotFound;
                answer = new Message.ResponseFail(errorCode);
            }
            else if (target == null)
            {
                //No player in the targeted slot -> Move the requesting player to the slot.
                Engine.Game.CurrentRoom.MovePlayer(source.SlotRef, _message.targetSlot);
                answer = new Message.ResponseSuccess();
            }
            else if (_targetClient == null)
            {
                errorCode = (int)ESimpleRequestErrorCode.Disconnected;
                answer = new Message.ResponseFail(errorCode);
            }
            else
            {
                _message.onCancel = OnCancelReceived;
                _client.onConnectionLost += ServerOnConnectionLost;
                _targetClient.onConnectionLost += ServerOnConnectionLost;
                _confirmHandler = new Handler.ConfirmSwap(_targetClient, source.player.username, OnSuccess, OnFail);
                pending = true;
            }

            if (!pending)
            {
                answer.requestId = _message.requestId;
                _client.QueueMessage(answer);
            }
        }

        protected void ServerOnConnectionLost(FFTcpClient a_client)
        {
            _client.onConnectionLost -= ServerOnConnectionLost;
            _targetClient.onConnectionLost -= ServerOnConnectionLost;
            if (!_confirmHandler.IsComplete())
                _confirmHandler.Cancel();

            int errorCode;
            if (a_client == _client)
            {
                errorCode = (int)ESimpleRequestErrorCode.Disconnected;
            }
            else
            {
                errorCode = (int)ESimpleRequestErrorCode.Disconnected;
            }

            AResponse answer = new Message.ResponseFail(errorCode);
            answer.requestId = _message.requestId;
            if (a_client == _client)
            {
                _targetClient.QueueMessage(answer);
            }
            else
            {
                _client.QueueMessage(answer);
            }
        }

        #region Callbacks
        protected void OnSuccess()
        {
            FFNetworkPlayer source = Engine.Game.CurrentRoom.GetPlayerForId(_client.NetworkID);
            Engine.Game.CurrentRoom.SwapPlayers(source.SlotRef, _message.targetSlot);

            AResponse answer = new Message.ResponseSuccess();
            answer.requestId = _message.requestId;
            _client.QueueMessage(answer);
        }

        protected void OnFail(int a_errorCode)
        {
            int errorCodeToReturn = a_errorCode;
            switch (a_errorCode)
            {
                case (int)Message.RequestConfirmSwap.EErrorCode.PlayerRefused:
                    errorCodeToReturn = (int)Message.RequestSlotSwap.EErrorCode.TargetRefused;
                    break;

                case (int)Message.RequestConfirmSwap.EErrorCode.TargetNotFound:
                    errorCodeToReturn = (int)Message.RequestSlotSwap.EErrorCode.PlayerNotFound;
                    break;

                case (int)Message.RequestConfirmSwap.EErrorCode.PlayerIsBusy:
                    errorCodeToReturn = (int)Message.RequestSlotSwap.EErrorCode.TargetIsBusy;
                    break;
            }

            AResponse answer = new Message.ResponseFail(errorCodeToReturn);
            answer.requestId = _message.requestId;
            _client.QueueMessage(answer);
        }

        protected void OnCancelReceived()
        {
            if (!_confirmHandler.IsComplete())
                _confirmHandler.Cancel();
        }
        #endregion
    }
}