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
    ServiceRatio,

    RacketPosition,
    BallMovement,
    BallCollision,
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
        protected string _channel;
        internal string Channel
        {
            get
            {
                return _channel;
            }
        }

        protected long _timestamp;
        internal long Timestamp
        {
            get
            {
                return _timestamp;
            }
        }

        protected FFTcpClient _client;
        internal FFTcpClient Client
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
            stream.Write(_channel);
            stream.Write(_timestamp);
        }

        public virtual void LoadFromData(FFByteReader stream)
        {
            _channel = stream.TryReadString();
            _timestamp = stream.TryReadLong();
        }
    }
}