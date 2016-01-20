using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace FF.Network
{
    internal class NetworkPositionEvent : IByteStreamSerialized
    {
        internal float timestamp = 0f;
        internal Vector3 position = Vector3.zero;
        internal Vector3 speed = Vector3.zero;

        public NetworkPositionEvent() : this(0f, Vector3.zero, Vector3.zero)
        {
        }

        internal NetworkPositionEvent(float a_timestamp, Vector3 a_position, Vector3 a_speed)
        {
            timestamp = a_timestamp;
            position = a_position;
            speed = a_speed;
        }

        public void SerializeData(FFByteWriter stream)
        {
            stream.Write(timestamp);
            stream.Write(position);
            //stream.Write(speed);
        }

        public void LoadFromData(FFByteReader stream)
        {
            timestamp = stream.TryReadFloat();
            position = stream.TryReadVector3();
            //speed = stream.TryReadVector3();
        }
    }
}