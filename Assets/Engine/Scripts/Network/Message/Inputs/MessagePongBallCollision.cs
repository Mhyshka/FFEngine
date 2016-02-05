using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    internal class MessagePongBallCollision : MessageData
    {
        #region Properties
        internal Vector3 position;
        internal Vector3 normal;

        internal override EDataType Type
        {
            get
            {
                return EDataType.M_PongBallCollision;
            }
        }
        #endregion

        #region Constructors
        public MessagePongBallCollision()
        {
        }

        public MessagePongBallCollision(Vector3 a_position, Vector3 a_normal)
        {
            position = a_position;
            normal = a_normal;
        }
        #endregion

        #region
        public override void LoadFromData(FFByteReader stream)
        {
            position = stream.TryReadVector3();
            normal = stream.TryReadVector3();
        }

        public override void SerializeData(FFByteWriter stream)
        {
            stream.Write(position);
            stream.Write(normal);
        }
        #endregion
    }
}