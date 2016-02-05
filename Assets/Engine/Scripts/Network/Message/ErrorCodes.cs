using UnityEngine;
using System.Collections;

using FF.Multiplayer;

namespace FF.Network.Message
{
    internal enum ERequestErrorCode
    {
        Success,
        Failed,

        Unknown,

        LocalCanceled,
        RemoteCanceled,

        IllegalArgument,
        IllegalState,
        Forbidden,
        Timeout,

        LocalConnectionIssue,
        RemoteConnectionIssue
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
        TargetDisconnected
    }

    internal enum EErrorCodeJoinRoom
    {
        PlayerNotfound,
        PlayerDisconnected,
        PlayerIsBannedFromRoom,
        RoomIsFull,
        ToServerOnly
    }

    internal enum EErrorCodeSwapConfirm
    {
        PlayerDisconnected,
        PlayerRefused,
        PlayerIsBusy,
        TargetNotFound
    }
}