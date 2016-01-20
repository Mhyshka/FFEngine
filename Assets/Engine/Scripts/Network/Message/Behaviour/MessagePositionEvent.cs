using UnityEngine;
using System.Collections;

namespace FF.Network.Message
{
    internal class MessagePositionEvent : AMessage
    {
        #region Properties
        internal NetworkPositionEvent positionEvent;

        internal override EMessageType Type
        {
            get
            {
                return EMessageType.PositionEvent;
            }
        }
        #endregion

        #region Constructors
        public MessagePositionEvent()
        {
            positionEvent = new NetworkPositionEvent();
        }

        internal MessagePositionEvent(NetworkPositionEvent a_positionEvent)
        {
            positionEvent = a_positionEvent;
        }
        #endregion

        #region
        public override void LoadFromData(FFByteReader stream)
        {
            positionEvent = stream.TryReadObject<NetworkPositionEvent>();
        }

        public override void SerializeData(FFByteWriter stream)
        {
            stream.Write(positionEvent);
        }
        #endregion
    }
}