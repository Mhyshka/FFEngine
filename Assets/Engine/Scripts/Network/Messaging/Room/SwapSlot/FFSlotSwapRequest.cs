using UnityEngine;
using System.Collections;
using System;

namespace FF.Networking
{
    internal class FFSlotSwapRequest : FFRequestMessage
    {
        internal enum EErrorCode
        {
            PlayerDisconnected,
            PlayerNotFound,
            TargetDisconnected,
            TargetRefused,
            TargetIsBusy
        }

        #region Properties
        internal FFSlotRef targetSlot;

        protected FFTcpClient _targetClient = null;
        protected FFConfirmSwapHandler _confirmHandler;

        internal override EMessageType Type
        {
            get
            {
                return EMessageType.SwapSlotClientRequest;
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
        public FFSlotSwapRequest()
        {
        }

        internal FFSlotSwapRequest(FFSlotRef a_target)
        {
            targetSlot = a_target;
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
            bool pending = false;
            FFResponseMessage answer = null;
            int errorCode = -1;
            FFNetworkPlayer source = FFEngine.Network.CurrentRoom.GetPlayerForId(_client.NetworkID);
            FFSlot ffSlot = FFEngine.Network.CurrentRoom.GetSlotForRef(targetSlot);
            FFNetworkPlayer target = ffSlot.netPlayer;
            _targetClient = null;
            if(target != null)
                _targetClient = FFEngine.Network.Server.ClientForEP(target.IpEndPoint);

            if (!_client.IsConnected)
            {
                errorCode = (int)EErrorCode.PlayerDisconnected;
                answer = new FFRequestFail(errorCode);
            }
            else if (source == null)
            {
                errorCode = (int)EErrorCode.PlayerNotFound;
                answer = new FFRequestFail(errorCode);
            }
            else if (target == null)
            {
                //No player in the targeted slot -> Move the requesting player to the slot.
                FFEngine.Network.CurrentRoom.MovePlayer(source.SlotRef, targetSlot);
                answer = new FFRequestSuccess();
            }
            else if (_targetClient == null)
            {
                errorCode = (int)EErrorCode.TargetDisconnected;
                answer = new FFRequestFail(errorCode);
            }
            else
            {
                onCancel = OnCancelReceived;
                _client.onConnectionLost += ServerOnConnectionLost;
                _targetClient.onConnectionLost += ServerOnConnectionLost;
                _confirmHandler = new FFConfirmSwapHandler(_targetClient, source.player.username, OnSuccess, OnFail);
                pending = true;
            }

            if (!pending)
            {
                answer.requestId = requestId;
                _client.QueueMessage(answer);
            }
        }

        protected void ServerOnConnectionLost(FFTcpClient a_client)
        {
            _client.onConnectionLost -= ServerOnConnectionLost;
            _targetClient.onConnectionLost -= ServerOnConnectionLost;
            _confirmHandler.Cancel();

            int errorCode;
            if (a_client == _client)
            {
                errorCode = (int)EErrorCode.PlayerDisconnected;
            }
            else
            {
                errorCode = (int)EErrorCode.TargetDisconnected;
            }

            FFResponseMessage answer = new FFRequestFail(errorCode);
            answer.requestId = requestId;
            if (a_client == _client)
            {
                _targetClient.QueueMessage(answer);
            }
            else
            {
                _client.QueueMessage(answer);
            }
        }

        protected void OnSuccess()
        {
            FFNetworkPlayer source = FFEngine.Network.CurrentRoom.GetPlayerForId(_client.NetworkID);
            FFEngine.Network.CurrentRoom.SwapPlayers(source.SlotRef, targetSlot);

            FFResponseMessage answer = new FFRequestSuccess();
            answer.requestId = requestId;
            _client.QueueMessage(answer);
        }

        protected void OnFail(int a_errorCode)
        {
            FFLog.LogError("On fail request swap.");

            FFConfirmSwapRequest.EErrorCode errorCode = (FFConfirmSwapRequest.EErrorCode)a_errorCode;

            int errorCodeToReturn = 0;
            switch (errorCode)
            {
                case FFConfirmSwapRequest.EErrorCode.PlayerDisconnected:
                    errorCodeToReturn = (int)EErrorCode.TargetDisconnected;
                    break;

                case FFConfirmSwapRequest.EErrorCode.PlayerRefused:
                    errorCodeToReturn = (int)EErrorCode.TargetRefused;
                    break;

                case FFConfirmSwapRequest.EErrorCode.TargetNotFound:
                    errorCodeToReturn = (int)EErrorCode.PlayerNotFound;
                    break;

                case FFConfirmSwapRequest.EErrorCode.PlayerIsBusy:
                    errorCodeToReturn = (int)EErrorCode.TargetIsBusy;
                    break;
            }

            FFResponseMessage answer = new FFRequestFail(errorCodeToReturn);
            answer.requestId = requestId;
            _client.QueueMessage(answer);
        }

        protected void OnCancelReceived()
        {
            FFLog.LogError("On Cancel Received.");
            _confirmHandler.Cancel();
        }

        #region Serialization
        public override void LoadFromData(FFByteReader stream)
        {
            base.LoadFromData(stream);
            targetSlot = stream.TryReadObject<FFSlotRef>();
        }

        public override void SerializeData(FFByteWriter stream)
        {
            base.SerializeData(stream);
            stream.Write(targetSlot);
        }
        #endregion


        internal static string MessageForCode(int a_errorCode)
        {
            string errorMessage = "";

            if (a_errorCode == -2)
                errorMessage = "Unkown";
            else if (a_errorCode == -1)
                errorMessage = "Timedout";
            else
            {
                EErrorCode errorCode = (EErrorCode)a_errorCode;

                switch (errorCode)
                {
                    case EErrorCode.PlayerDisconnected:
                        errorMessage = "Connection issues.";
                        break;

                    case EErrorCode.PlayerNotFound:
                        errorMessage = "Player not found.";
                        break;

                    case EErrorCode.TargetRefused:
                        errorMessage = "Target refused.";
                        break;

                    case EErrorCode.TargetDisconnected:
                        errorMessage = "Target is disconnected.";
                        break;
                }
            }
            return errorMessage;
        }
    }
}