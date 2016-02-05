using UnityEngine;
using System.Collections;
using System;

using FF.UI;

namespace FF.Network.Message
{
    internal class RequestConfirmSwap : MessageData
    {
        internal enum EErrorCode
        {
            PlayerRefused,
            PlayerIsBusy,
            TargetNotFound
        }

        #region Properties
        internal string fromUsername;
        
        internal override EDataType Type
        {
            get
            {
                return EDataType.Q_ConfirmSwap;
            }
        }
        #endregion

        #region Constructors
        internal RequestConfirmSwap(string a_fromUsername)
        {
            fromUsername = a_fromUsername;
        }
        #endregion

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
