using UnityEngine;
using System.Collections;

namespace FF.Network.Message
{
    internal enum EMessageType
    {
        Heartbeat,
        NetworkID,

        InputEvent,

        RoomInfos,

        RemovedFromRoom,
        Ban,

        ResponseSuccess,
        ResponseFail,
        ResponseCancel,

        RequestEmpty,
        IsIdleRequest,

        JoinRoomRequest,
        MoveToSlotRequest,
        SlotSwapRequest,
        ConfirmSwapRequest,

        Farewell,
        LeavingRoom,

        RequestGameMode,
        LoadingStarted,
        LoadingProgress,
        LoadingComplete,
        LoadingReady,
        LoadingEveryoneReady,

        PongTargetRatio,
        PongBallPosition,
        PongBallMovement,
        PongBallCollision,

        PositionEvent
    }

    internal class MessageFactory
    {
        internal static AMessage CreateMessage(EMessageType a_type)
        {
            AMessage message = null;

            switch (a_type)
            {
                case EMessageType.Heartbeat:
                    message = new MessageHeartBeat();
                    break;
                case EMessageType.NetworkID:
                    message = new MessageNetworkID();
                    break;

                case EMessageType.InputEvent:
                    message = new MessageInputEvent();
                    break;

                case EMessageType.RoomInfos:
                    message = new MessageRoomInfos();
                    break;

                case EMessageType.ResponseSuccess:
                    message = new ResponseSuccess();
                    break;
                case EMessageType.ResponseFail:
                    message = new ResponseFail();
                    break;
                case EMessageType.ResponseCancel:
                    message = new ResponseCancel();
                    break;

                case EMessageType.RequestEmpty:
                    message = new RequestEmpty();
                    break;
                case EMessageType.IsIdleRequest:
                    message = new RequestIsIdle();
                    break;

                case EMessageType.JoinRoomRequest:
                    message = new RequestJoinRoom();
                    break;
                case EMessageType.MoveToSlotRequest:
                    message = new RequestMoveToSlot();
                    break;

                case EMessageType.SlotSwapRequest:
                    message = new RequestSlotSwap();
                    break;
                case EMessageType.ConfirmSwapRequest:
                    message = new RequestConfirmSwap();
                    break;

                case EMessageType.RemovedFromRoom:
                    message = new MessageRemovedFromRoom();
                    break;

                case EMessageType.Farewell:
                    message = new MessageFarewell();
                    break;

                case EMessageType.LeavingRoom:
                    message = new MessageLeavingRoom();
                    break;


                case EMessageType.RequestGameMode:
                    message = new MessageRequestGameMode();
                    break;
                case EMessageType.LoadingStarted:
                    message = new MessageLoadingStarted();
                    break;
                case EMessageType.LoadingProgress:
                    message = new MessageLoadingProgress();
                    break;
                case EMessageType.LoadingComplete:
                    message = new MessageLoadingComplete();
                    break;
                case EMessageType.LoadingReady:
                    message = new MessageLoadingReady();
                    break;
                case EMessageType.LoadingEveryoneReady:
                    message = new MessageLoadingEveryoneReady();
                    break;

                case EMessageType.PongTargetRatio:
                    message = new MessagePongTargetRatio();
                    break;
                case EMessageType.PongBallPosition:
                    message = new MessagePongBallPosition();
                    break;
                case EMessageType.PongBallCollision:
                    message = new MessagePongBallCollision();
                    break;
                case EMessageType.PongBallMovement:
                    message = new MessagePongBallMovement();
                    break;

                case EMessageType.PositionEvent:
                    message = new MessagePositionEvent();
                    break;
            }

            return message;
        }
    }
}