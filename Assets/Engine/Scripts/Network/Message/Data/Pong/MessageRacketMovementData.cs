using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    internal class MessageRacketMovementData : MessageData
    {
        #region Properties
        protected float _currentRatio;
        internal float CurrentRatio
        {
            get
            {
                return _currentRatio;
            }
        }

        protected float _targetRatio;
        internal float TargetRatio
        {
            get
            {
                return _targetRatio;
            }
        }
        internal override EDataType Type
        {
            get
            {
                return EDataType.RacketMove;
            }
        }
        #endregion

        public MessageRacketMovementData()
        {
        }

        internal MessageRacketMovementData(float a_currentRatio, float a_targetRatio)
        {
            _currentRatio = a_currentRatio;
            _targetRatio = a_targetRatio;
        }

        #region Serialization
        public override void SerializeData(FFByteWriter stream)
        {
            base.SerializeData(stream);
            stream.Write(_currentRatio);
            stream.Write(_targetRatio);
        }

        public override void LoadFromData(FFByteReader stream)
        {
            base.LoadFromData(stream);
            _currentRatio = stream.TryReadFloat();
            _targetRatio = stream.TryReadFloat();
        }
        #endregion
    }
}
