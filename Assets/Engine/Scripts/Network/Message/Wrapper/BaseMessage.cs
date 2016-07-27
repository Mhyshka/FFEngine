using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

internal enum EMessageChannel
{
    CancelRequest,
    Response,

    InputEvent,
    IsIdle,

    Next,
    Ready,

    StartGame,
    LoadingComplete, //Clinet : Assets loading is complete.  Server : loadingstate is finished
    LoadingReady, //eg : Called on click ( press any key )
    LoadingProgress,

    Farewell,
    VersionCompatibility,
    NetworkId,
    Heartbeat,
    IsAlive,
    ClockSync,

    RoomInfos,
    JoinRoom,
    RemovedFromRoom,
    LeavingRoom,
    MoveToSlot,
    SwapSlot,
    SwapConfirm,

    ChallengeInfos,
    ServiceLaunch,

    RacketPosition,
    BallMovement,
    RacketHit,
    GoalHit,
    TrySmash,
    Smash
}

namespace FF.Network.Message
{
    internal abstract class BaseMessage : IByteStreamSerialized
    {
        #region Properties
        protected long _timestamp;
        internal long Timestamp
        {
            get
            {
                return _timestamp;
            }
            set
            {
                _timestamp = value;
            }
        }

        protected FFNetworkClient _client;
        internal FFNetworkClient Client
        {
            get
            {
                return _client;
            }
            set
            {
                _client = value;
            }
        }

        protected MessageData _data = null;
        internal MessageData Data
        {
            get
            {
                return _data;
            }
        }

        internal virtual EHeaderType HeaderType
        {
            get
            {
                return EHeaderType.Message;
            }
        }
        #endregion

        protected virtual void OnComplete()
        {
        }

        public virtual void SerializeData(FFByteWriter stream)
        {
            stream.Write(_timestamp);
        }

        public virtual void LoadFromData(FFByteReader stream)
        {
            _timestamp = stream.TryReadLong();
        }
    }
}