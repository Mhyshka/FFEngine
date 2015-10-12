using UnityEngine;
using System.Collections;

namespace FF.Networking
{
    internal enum EMessageType
    {
        Heartbeat,
        NetworkID,

        RoomInfos,

        RemovedFromRoom,
        Ban,

        RequestSuccess,
        RequestFail,
        RequestCancel,

        JoinRoomRequest,
        MoveToSlotRequest,
        SwapSlotClientRequest,
        SwapSlotServerRequest,
        SwapConfirmRequest,

        Farewell,
        LeavingRoom
    }

    internal class FFMessageFactory
    {
        internal static FFMessage CreateMessage(EMessageType a_type)
        {
            FFMessage message = null;

            switch (a_type)
            {
                case EMessageType.Heartbeat:
                    message = new FFMessageHeartBeat();
                    break;
                case EMessageType.NetworkID:
                    message = new FFMessageNetworkID();
                    break;

                case EMessageType.RoomInfos:
                    message = new FFMessageRoomInfo();
                    break;

                case EMessageType.RequestSuccess:
                    message = new FFRequestSuccess();
                    break;
                case EMessageType.RequestFail:
                    message = new FFRequestFail();
                    break;
                case EMessageType.RequestCancel:
                    message = new FFRequestCancel();
                    break;

                case EMessageType.JoinRoomRequest:
                    message = new FFJoinRoomRequest();
                    break;
                case EMessageType.MoveToSlotRequest:
                    message = new FFMoveToSlotRequest();
                    break;

                case EMessageType.SwapSlotClientRequest:
                    message = new FFSlotSwapRequest();
                    break;
                case EMessageType.SwapConfirmRequest:
                    message = new FFConfirmSwapRequest();
                    break;

                case EMessageType.RemovedFromRoom:
                    message = new FFMessageRemovedFromRoom();
                    break;

                case EMessageType.Farewell:
                    message = new FFMessageFarewell();
                    break;

                case EMessageType.LeavingRoom:
                    message = new FFMessageLeavingRoom();
                    break;
            }

            return message;
        }
    }
}