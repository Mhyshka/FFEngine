using UnityEngine;
using System.Collections;
using System;

using FF.Multiplayer;

namespace FF.Network.Message
{
    internal class MessageSlotRefData : MessageData
    {
        #region Properties
        protected SlotRef _slotRef;
        internal SlotRef SlotRef
        {
            get
            {
                return _slotRef;
            }
        }

        internal override EDataType Type
        {
            get
            {
                return EDataType.SlotRef;
            }
        }
        #endregion

        #region Constructors
        public MessageSlotRefData()
        {
        }

        internal MessageSlotRefData(SlotRef a_target)
        {
            _slotRef = a_target;
        }
        #endregion

        #region Serialization
        public override void LoadFromData(FFByteReader stream)
        {
            base.LoadFromData(stream);
            _slotRef = stream.TryReadObject<SlotRef>();
        }

        public override void SerializeData(FFByteWriter stream)
        {
            base.SerializeData(stream);
            stream.Write(_slotRef);
        }
        #endregion
    }
}