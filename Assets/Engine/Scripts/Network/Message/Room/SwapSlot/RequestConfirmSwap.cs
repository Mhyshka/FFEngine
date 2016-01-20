using UnityEngine;
using System.Collections;
using System;

using FF.UI;

namespace FF.Network.Message
{
    internal class RequestConfirmSwap : ARequest
    {
        protected static RequestConfirmSwap s_CurrentRequest = null;
        internal enum EErrorCode
        {
            PlayerRefused,
            PlayerIsBusy,
            TargetNotFound
        }

        #region Properties
        internal string fromUsername;
        
        internal override EMessageType Type
        {
            get
            {
                return EMessageType.ConfirmSwapRequest;
            }
        }

        internal override bool HandleByMock
        {
            get
            {
                return true;
            }
        }
        #endregion

        #region Constructors
        public RequestConfirmSwap()
        {
        }

        internal RequestConfirmSwap(string a_fromUsername)
        {
            fromUsername = a_fromUsername;
        }
        #endregion

        protected override float TimeoutDuration
        {
            get
            {
                return float.MaxValue;
            }
        }

        #region Serialization
        public override void LoadFromData(FFByteReader stream)
        {
            base.LoadFromData(stream);
            fromUsername = stream.TryReadString();
        }

        public override void SerializeData(FFByteWriter stream)
        {
            base.SerializeData(stream);
            stream.Write(fromUsername);
        }
        #endregion
    }
}
