using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    internal class MessagePongBallPosition : AMessage
    {
        #region Properties
        internal Vector3 position;

        internal override EMessageType Type
        {
            get
            {
                return EMessageType.PongBallPosition;
            }
        }
        #endregion

        #region Constructors
        public MessagePongBallPosition()
        {
            position = Vector3.zero;
        }

        internal MessagePongBallPosition(Vector3 a_position)
        {
            position = a_position;
            //rotation = a_rotation;
        }
        #endregion

        #region
        public override void LoadFromData(FFByteReader stream)
        {
            position.x = stream.TryReadFloat();
            position.z = stream.TryReadFloat();
            //rotation = stream.TryReadQuaternion();
        }

        public override void SerializeData(FFByteWriter stream)
        {
            stream.Write(position.x);
            stream.Write(position.z);
            //stream.Write(rotation);
        }
        #endregion
    }
}