using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    internal class MessageInputEventData : MessageData
    {
        #region Properties
        internal string eventKey;
        internal bool isDown;

        internal override EDataType Type
        {
            get
            {
                return EDataType.InputEvent;
            }
        }
        #endregion

        #region Constructors
        public MessageInputEventData()
        {
            eventKey = "";
            isDown = false;
        }

        internal MessageInputEventData(EInputEventKey a_eventKey, bool a_isDown) : this(a_eventKey.ToString(), a_isDown)
        {
        }

        internal MessageInputEventData(string a_eventKey, bool a_isDown)
        {
            eventKey = a_eventKey;
            isDown = a_isDown;
        }
        #endregion

        #region
        public override void LoadFromData(FFByteReader stream)
        {
            eventKey = stream.TryReadString();
            isDown = stream.TryReadBool();
        }

        public override void SerializeData(FFByteWriter stream)
        {
            stream.Write(eventKey);
            stream.Write(isDown);
        }
        #endregion
    }
}