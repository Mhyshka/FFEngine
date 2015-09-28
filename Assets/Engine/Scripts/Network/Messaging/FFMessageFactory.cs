using UnityEngine;
using System.Collections;

namespace FF.Networking
{
    internal enum EMessageType
    {
        Heartbeat,

        RoomInfos,

        JoinRoomRequest,
        JoinRoomFail,
        JoinRoomSuccess,

        MoveToSlotRequest,
        MoveToSlotSuccess,
        MoveToSlotFail,

        Kick,
        Ban,
        Swap,

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

                case EMessageType.RoomInfos:
                    message = new FFMessageRoomInfo();
                    break;

                case EMessageType.JoinRoomRequest:
                    message = new FFJoinRoomRequest();
                    break;
                case EMessageType.JoinRoomSuccess:
                    message = new FFJoinRoomSuccess();
                    break;
                case EMessageType.JoinRoomFail:
                    message = new FFJoinRoomFail();
                    break;

                case EMessageType.MoveToSlotRequest:
                    message = new FFMoveToSlotRequest();
                    break;
                case EMessageType.MoveToSlotSuccess:
                    message = new FFMoveToSlotSuccess();
                    break;
                case EMessageType.MoveToSlotFail:
                    message = new FFMoveToSlotFail();
                    break;

                case EMessageType.Kick:
                    message = new FFMessageKick();
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