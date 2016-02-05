using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    internal class MessageBallMovementData : MessageData
    {
        #region Properties
        internal Vector3 velocity;
        internal Vector3 position;

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
            velocity = Vector3.zero;
            position = Vector3.zero;
        }

        internal MessageBallMovementData(Vector3 a_position, Vector3 a_velocity)
        {
            velocity = a_velocity;
            position = a_position;
        }
        #endregion

        #region
        public override void LoadFromData(FFByteReader stream)
        {
            position.x = stream.TryReadFloat();
            position.z = stream.TryReadFloat();

            velocity.x = stream.TryReadFloat();
            velocity.z = stream.TryReadFloat();
        }

        public override void SerializeData(FFByteWriter stream)
        {
            stream.Write(position.x);
            stream.Write(position.z);

            stream.Write(velocity.x);
            stream.Write(velocity.z);
        }
        #endregion
    }
}