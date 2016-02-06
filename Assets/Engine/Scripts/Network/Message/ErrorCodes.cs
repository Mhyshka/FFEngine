using UnityEngine;
using System.Collections;

using FF.Multiplayer;

namespace FF.Network.Message
{
    internal enum ERequestErrorCode
    {
        Success,
        Failed,
        Canceled,
        Timeout
    }

    internal enum EErrorCodeMoveToSlot
    {
        PlayerNotfound,
        PlayerDisconnected,
        SlotIsUsed
    }

    internal enum EErrorCodeSwapSlot
    {
        PlayerNotfound,
        PlayerDisconnected,
        TargetDisconnected,
        PlayerRefused,
        PlayerIsBusy,
    }

    internal enum EErrorCodeJoinRoom
    {
        PlayerNotfound,
        PlayerDisconnected,
        PlayerIsBannedFromRoom,
        RoomIsFull,
        ToServerOnly
    }
}