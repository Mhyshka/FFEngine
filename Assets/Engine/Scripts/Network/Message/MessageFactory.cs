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

        BallMovement,
        BallCollision,
        RacketHit,
        RacketMove,
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
            MessageData data = null;
            switch (a_type)
            {
                case EDataType.Empty:
                    data = new MessageEmptyData();
                    break;
                case EDataType.Integer:
                    data = new MessageIntegerData();
                    break;
                case EDataType.Long:
                    data = new MessageLongData();
                    break;
                case EDataType.Float:
                    data = new MessageFloatData();
                    break;
                case EDataType.String:
                    data = new MessageStringData();
                    break;
                case EDataType.Bool:
                    data = new MessageBoolData();
                    break;

                case EDataType.InputEvent:
                    data = new MessageInputEventData();
                    break;

                case EDataType.Room:
                    data = new MessageRoomData();
                    break;

                case EDataType.Player:
                    data = new MessagePlayerData();
                    break;

                case EDataType.SlotRef:
                    data = new MessageSlotRefData();
                    break;

                case EDataType.LoadingProgress:
                    data = new MessageLoadingProgressData();
                    break;


                case EDataType.BallMovement:
                    data = new MessageBallMovementData();
                    break;
                case EDataType.BallCollision:
                    data = new MessageBallCollisionData();
                    break;
                case EDataType.RacketMove:
                    data = new MessageRacketMovementData();
                    break;
            }

            return data;
        }
    }
}