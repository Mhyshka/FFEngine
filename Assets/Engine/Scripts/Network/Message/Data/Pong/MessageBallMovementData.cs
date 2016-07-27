using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    internal class MessageBallMovementData : MessageData
    {
        #region Properties
        internal int playerId;
        internal Vector3 position;
        internal Vector3 velocity;

        internal override EDataType Type
        {
            get
            {
                return EDataType.BallMovement;
            }
        }
        #endregion

        #region Constructors
        public MessageBallMovementData()
        {
        }

        public MessageBallMovementData(int a_playerId, Vector3 a_position, Vector3 a_newVelocity)
        {
            playerId = a_playerId;
            position = a_position;
            velocity = a_newVelocity;
        }
        #endregion

        #region
        public override void LoadFromData(FFByteReader stream)
        {
            playerId = stream.TryReadInt();
            position = stream.TryReadVector3();
            velocity = stream.TryReadVector3();
        }

        public override void SerializeData(FFByteWriter stream)
        {
            stream.Write(playerId);
            stream.Write(position);
            stream.Write(velocity);
        }
        #endregion
    }
}