using UnityEngine;
using System.Collections;

namespace FF.Network.Message
{
    internal enum EHeaderType
    {
        Message,
        Request,
        Response
    }

    internal enum EDataType
    {
        Empty,
        Long,
        Integer,
        Float,
        Bool,
        String,

        InputEvent,
        Room,
        Player,
        SlotRef,

        LoadingProgress,

        M_RequestGameMode,
        M_LoadingStarted,
        
        M_LoadingComplete,
        M_LoadingReady,
        M_LoadingEveryoneReady,


        BallMovement,
        

        M_PositionEvent,

        M_ServiceChallengeInfo,
        M_ServiceRatio,
        M_TrySmash,
        M_DidSmash,

        BallCollision,
        RacketHit,
        GoalHit
    }

    internal class MessageFactory
    {
        internal static ReadMessage CreateMessage(EHeaderType a_type)
        {
            ReadMessage message = null;
            switch (a_type)
            {
                case EHeaderType.Message:
                    message = new ReadMessage();
                    break;

                case EHeaderType.Response:
                    message = new ReadResponse();
                    break;

                case EHeaderType.Request:
                    message = new ReadRequest();
                    break;
            }
            return message;
        }

        internal static MessageData CreateData(EDataType a_type)
        {
            MessageData message = null;
            switch (a_type)
            {
                case EDataType.Long:
                    //message = new RequestHeartBeat();
                    break;
                /*case EDataType.NetworkID:
                    message = new MessageNetworkID();
                    break;

                case EDataType.InputEvent:
                    message = new MessageInputEvent();
                    break;

                case EDataType.RoomInfos:
                    message = new MessageRoomInfos();
                    break;

                case EDataType.ResponseSuccess:
                    message = new ResponseSuccess();
                    break;
                case EDataType.ResponseFail:
                    message = new ResponseFail();
                    break;
                case EDataType.ResponseCancel:
                    message = new ResponseCancel();
                    break;

                case EDataType.RequestEmpty:
                    message = new RequestEmpty();
                    break;
                case EDataType.IsIdleRequest:
                    message = new RequestIsIdle();
                    break;

                case EDataType.JoinRoomRequest:
                    message = new RequestJoinRoom();
                    break;
                case EDataType.MoveToSlotRequest:
                    message = new RequestMoveToSlot();
                    break;

                case EDataType.SlotSwapRequest:
                    message = new RequestSlotSwap();
                    break;
                case EDataType.ConfirmSwapRequest:
                    message = new RequestConfirmSwap();
                    break;

                case EDataType.RemovedFromRoom:
                    message = new MessageRemovedFromRoom();
                    break;

                case EDataType.Farewell:
                    message = new MessageFarewell();
                    break;

                case EDataType.LeavingRoom:
                    message = new MessageLeavingRoom();
                    break;


                case EDataType.RequestGameMode:
                    message = new MessageRequestGameMode();
                    break;
                case EDataType.LoadingStarted:
                    message = new MessageLoadingStarted();
                    break;
                case EDataType.LoadingProgress:
                    message = new MessageLoadingProgress();
                    break;
                case EDataType.LoadingComplete:
                    message = new MessageLoadingComplete();
                    break;
                case EDataType.LoadingReady:
                    message = new MessageLoadingReady();
                    break;
                case EDataType.LoadingEveryoneReady:
                    message = new MessageLoadingEveryoneReady();
                    break;

                case EDataType.PongTargetRatio:
                    message = new MessagePongTargetRatio();
                    break;
                case EDataType.PongBallCollision:
                    message = new MessagePongBallCollision();
                    break;
                case EDataType.PongBallMovement:
                    message = new MessagePongBallMovement();
                    break;

                case EDataType.PositionEvent:
                    message = new MessagePositionEvent();
                    break;

                case EDataType.ServiceChallengeInfo:
                    message = new MessageServiceChallengeInfo();
                    break;

                case EDataType.ServiceRatio:
                    message = new MessageServiceRatio();
                    break;

                case EDataType.TrySmash:
                    message = new MessageTrySmash();
                    break;

                case EDataType.RacketHit:
                    message = new MessageRacketHit();
                    break;

                case EDataType.GoalHit:
                    message = new MessageGoalHit();
                    break;*/
            }

            return message;
        }
    }
}